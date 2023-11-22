using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.VFX;

namespace My_MemoPlatformer
{

    public class DamageDetector : MonoBehaviour  //Compare collision info versus attack input that is being registered
    {

        [SerializeField] private bool _debug;
        private CharacterControl _control;

        [SerializeField] private float hp;

        [SerializeField] private List<RuntimeAnimatorController> _hitReactionList = new List<RuntimeAnimatorController>();

        private void Awake()
        {
            _control = GetComponent<CharacterControl>();
        }

        private void Update()
        {
            if (AttackManager.Instance.currentAttacks.Count > 0)
            {
                if (_control.animationProgress.collidingBodyParts.Count != 0)
                {
                    CheckAttack();
                }
            }
        }

        private void CheckAttack()
        {
            foreach (AttackInfo info in AttackManager.Instance.currentAttacks)
            {
                if (info == null)
                {
                    continue;
                }

                if (!info.isRegistered)
                {
                    continue;
                }

                if (info.isFinished)
                {
                    continue;
                }

                if (info.currentHits >= info.maxHits)
                {
                    continue;
                }

                if (info.attacker == _control)
                {
                    continue;
                }

                if (info.mustFaceAttacker)
                {
                    Vector3 vec = this.transform.position - info.attacker.transform.position;  //Вектор от жертвы до нападающего
                    if (vec.z * info.attacker.transform.forward.z < 0f) //Мы сравниваем 2 вектора, если они смотрят в разные стороны, со один из них отрицательный, следовательно, перменожение решает смотрят ли они друг на друга
                    {
                        continue;
                    }
                }

                if (info.registeredTargets.Contains(this._control))  //prevent several times damage from one attack
                {
                    continue;
                }

                if (info.mustCollide)
                {
                    if (IsCollided(info))
                    {
                        TakeDamage(info);
                    }
                }
                else
                {
                    if (IsInLethalRange(info))
                    {
                        TakeDamage(info);
                    }
                }
            }
        }

        private bool IsCollided(AttackInfo info)
        {
            foreach (KeyValuePair<TriggerDetector, List<Collider>> data in _control.animationProgress.collidingBodyParts)
            {
                foreach (Collider collider in data.Value)
                {
                    foreach (AttackPartType part in info.attackParts)  //Имена атакующих коллайдеров
                    {
                        if (info.attacker.GetAttackingPart(part) == collider.gameObject)
                        {
                            _control.animationProgress.attack = info.attackAbility;
                            _control.animationProgress.attacker = info.attacker;

                            _control.animationProgress.damagedTrigger = data.Key;
                            _control.animationProgress.attackingPart = info.attacker.GetAttackingPart(part);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool IsInLethalRange(AttackInfo info)
        {
            float dist = Vector3.SqrMagnitude(this.gameObject.transform.position - info.attacker.transform.position); //distance between target and attacker
                                                                                                                      //Debug.Log(this.gameObject.name + "dist: "+ dist.ToString() );
            if (dist <= info.lethalRange)
            {
                _control.animationProgress.attack = info.attackAbility;
                _control.animationProgress.attacker = info.attacker;

                int index = Random.Range(0, _control.ragdollParts.Count);
                _control.animationProgress.damagedTrigger = _control.ragdollParts[index].GetComponent<TriggerDetector>();
                return true;
            }
            return false;
        }

        public bool IsDead()
        {
            if (hp <= 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void TakeDamage(AttackInfo info)
        {
            if (IsDead())  //templory fix for hitting dead enemy
            {
                return;
            }

            if (info.mustCollide)
            {
                CameraManager.Instance.ShakeCamera(0.3f);

                if (info.attackAbility.useDeathParticles)
                {
                    if (info.attackAbility.ParticleType.ToString().Contains("VFX"))
                    {
                        GameObject vfx = PoolManager.Instance.GetObject(info.attackAbility.ParticleType);

                        vfx.transform.position = _control.animationProgress.attackingPart.transform.position;

                        vfx.SetActive(true);

                        if (info.attacker.IsFacingForward())
                        {
                            vfx.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        }
                        else
                        {
                            vfx.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                        }
                    }
                }
            }

            if (_debug)
            {
                Debug.Log(info.attacker.gameObject.name + " hits " + this.gameObject.name);
            }

            info.currentHits++;
            hp -= info.attackAbility.damage;

            AttackManager.Instance.ForceDeregester(_control);

            if (IsDead())
            {
                _control.animationProgress.ragdollTriggered = true;
                _control.GetComponent<BoxCollider>().enabled = false;
                _control.ledgeChecker.GetComponent<BoxCollider>().enabled = false;
                _control.Rigid_Body.useGravity = false;

                if (_control.aiController != null)
                {
                    _control.aiController.gameObject.SetActive(false);
                    _control.navMeshObstacle.carving = false; //we dont need carving when enemy is dead
                }
            }
            else
            {
                var rand = Random.RandomRange(0, _hitReactionList.Count);

                _control.skinnedMeshAnimator.runtimeAnimatorController = null; //need this to restart the animation if get several hits in a short period of time
                _control.skinnedMeshAnimator.runtimeAnimatorController = _hitReactionList[rand];
            }

            //register damaged target
            if (!info.registeredTargets.Contains(this._control))
            {
                info.registeredTargets.Add(this._control);
            }
        }

        public void TriggerSpikeDeath(RuntimeAnimatorController animator)
        {
            _control.skinnedMeshAnimator.runtimeAnimatorController = animator;
        }

        public void TakeTotalDamage()
        {
            hp = 0f;
        }
    }
}