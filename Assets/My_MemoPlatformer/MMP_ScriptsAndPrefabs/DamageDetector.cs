using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class DamageDetector : MonoBehaviour  //Compare collision info versus attack input that is being registered
    {

        [SerializeField] private bool _debug;
        private CharacterControl _control;

        [SerializeField] private float hp;

        [SerializeField] private List<RuntimeAnimatorController> _hitReactionList = new List<RuntimeAnimatorController>();

        [Header("Damage Info")]
        public Attack attack;
        public CharacterControl attacker;
        public TriggerDetector damagedTrigger;
        public GameObject attackingPart;

        private void Awake()
        {
            _control = GetComponent<CharacterControl>();
        }

        private void Update()
        {
            if (AttackManager.Instance.currentAttacks.Count > 0)
            {
                CheckAttack();
            }
        }

        private bool AttackIsValid(AttackInfo info)
        {
            if (info == null)
            {
                return false; ;
            }

            if (!info.isRegistered)
            {
                return false;
            }

            if (info.isFinished)
            {
                return false;
            }

            if (info.currentHits >= info.maxHits)
            {
                return false;
            }

            if (info.attacker == _control)
            {
                return false;
            }

            if (info.mustFaceAttacker)
            {
                Vector3 vec = this.transform.position - info.attacker.transform.position;  //Вектор от жертвы до нападающего
                if (vec.z * info.attacker.transform.forward.z < 0f) //Мы сравниваем 2 вектора, если они смотрят в разные стороны, со один из них отрицательный, следовательно, перменожение решает смотрят ли они друг на друга
                {
                    return false;
                }
            }

            if (info.registeredTargets.Contains(this._control))  //prevent several times damage from one attack
            {
                return false;
            }

            return true;
        }

        private void CheckAttack()
        {
            foreach (AttackInfo info in AttackManager.Instance.currentAttacks)
            {
                if (AttackIsValid(info))
                {
                    if (info.mustCollide)
                    {
                        if (_control.animationProgress.collidingBodyParts.Count != 0)
                        {
                            if (IsCollided(info))
                            {
                                TakeDamage(info);
                            }
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
                            _control.damageDetector.attack = info.attackAbility;
                            _control.damageDetector.attacker = info.attacker;

                            _control.damageDetector.damagedTrigger = data.Key;
                            _control.damageDetector.attackingPart = info.attacker.GetAttackingPart(part);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool IsInLethalRange(AttackInfo info)
        {
            foreach (var c in _control.ragdollParts)
            {
                float dist = Vector3.SqrMagnitude(c.transform.position - info.attacker.transform.position); //distance between target and attacker
                                                                                                                          //Debug.Log(this.gameObject.name + "dist: "+ dist.ToString() );
                if (dist <= info.lethalRange)
                {
                    _control.damageDetector.attack = info.attackAbility;
                    _control.damageDetector.attacker = info.attacker;

                    int index = Random.Range(0, _control.ragdollParts.Count);
                    _control.damageDetector.damagedTrigger = _control.ragdollParts[index].GetComponent<TriggerDetector>();
                    return true;
                }
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

        public void TakeDamage(AttackInfo info)
        {
            if (IsDead())  //templory fix for hitting dead enemy
            {
                if (!info.registeredTargets.Contains(this._control))
                {
                    info.registeredTargets.Add(this._control);
                    _control.AddForceToDamagedPart(true);
                }
                return;
            }

            if (_control.animationProgress.IsRunning(typeof(Block)))
            {
                var dir = info.attacker.transform.position - _control.transform.position;

                if (dir.z > 0f)
                {
                    if (_control.IsFacingForward())
                    {
                        return;
                    }
                }
                else if (dir.z < 0f)
                {
                    if (!_control.IsFacingForward())
                    {
                        return;
                    }
                }
            }

            if (info.mustCollide)
            {
                CameraManager.Instance.ShakeCamera(0.3f);

                if (info.attackAbility.useDeathParticles)
                {
                    if (info.attackAbility.ParticleType.ToString().Contains("VFX"))
                    {
                        GameObject vfx = PoolManager.Instance.GetObject(info.attackAbility.ParticleType);

                        vfx.transform.position = _control.damageDetector.attackingPart.transform.position;

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
            _control.animationProgress.currentRunningAbilities.Clear();
            

            if (IsDead())
            {
                _control.animationProgress.ragdollTriggered = true;
                _control.GetComponent<BoxCollider>().enabled = false;
                _control.ledgeChecker.collider1.GetComponent<BoxCollider>().enabled = false;
                _control.Rigid_Body.useGravity = false;

                if (_control.aiController != null)
                {
                    _control.aiController.gameObject.SetActive(false);
                    _control.navMeshObstacle.carving = false; //we dont need carving when enemy is dead
                }
            }
            else
            {
                var rand = Random.Range(0, _hitReactionList.Count);

                _control.skinnedMeshAnimator.runtimeAnimatorController = null; //need this to restart the animation if get several hits in a short period of time
                _control.skinnedMeshAnimator.runtimeAnimatorController = _hitReactionList[rand];
            }


            if (!info.registeredTargets.Contains(this._control))
            {
                info.registeredTargets.Add(this._control);
            }
        }

        public void TriggerSpikeDeath(RuntimeAnimatorController animator)
        {
            _control.skinnedMeshAnimator.runtimeAnimatorController = animator;
        }

        public void DeathBySpikes()
        {
            _control.damageDetector.damagedTrigger = null;
            hp = 0f;
        }
    }
}