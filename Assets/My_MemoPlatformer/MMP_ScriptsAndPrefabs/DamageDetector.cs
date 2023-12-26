using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class DamageDetector : SubComponent  //Compare collision info versus attack input that is being registered
    {
        public DamageDetector_Data damageDetector_Data;
        [SerializeField] private bool _debug;

        [Header("Damage Setup")]
        [SerializeField] private List<RuntimeAnimatorController> _hitReactionList = new List<RuntimeAnimatorController>();

        [SerializeField] private Attack AxeThrow;
        [SerializeField] private Attack airStompAttack;

        private void Start()
        {
            damageDetector_Data = new DamageDetector_Data
            {
                attacker = null,
                attack = null,
                damagedTrigger = null,
                attackingPart = null,
                blockedAttack = null,
                hp = 3f,
                airStompAttack = airStompAttack,
                AxeThrow = AxeThrow,
                IsDead = IsDead,
                TakeDamage = TakeDamage,
            };

            subComponentProcessor.damageDetector_Data = damageDetector_Data;
            subComponentProcessor.subcomponentsDictionary.Add(SubComponentType.DAMAGE_DETECTOR_DATA, this);
        }

        public override void OnUpdate()
        {
            if (AttackManager.Instance.currentAttacks.Count > 0)
            {
                CheckAttack();
            }
        }

        public override void OnFixedUpdate()
        {
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

            if (info.attacker == control)
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

            if (info.registeredTargets.Contains(this.control))  //prevent several times damage from one attack
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
                        if (control.animationProgress.collidingBodyParts.Count != 0)
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
            foreach (KeyValuePair<TriggerDetector, List<Collider>> data in control.animationProgress.collidingBodyParts)
            {
                foreach (Collider collider in data.Value)
                {
                    foreach (AttackPartType part in info.attackParts)  //Имена атакующих коллайдеров
                    {
                        if (info.attacker.GetAttackingPart(part) == collider.gameObject)
                        {
                            damageDetector_Data.attack = info.attackAbility;
                            damageDetector_Data.attacker = info.attacker;

                            damageDetector_Data.damagedTrigger = data.Key;
                            damageDetector_Data.attackingPart = info.attacker.GetAttackingPart(part);

                            damageDetector_Data.SetData(info.attacker,info.attackAbility,data.Key,info.attacker.GetAttackingPart(part));
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool IsInLethalRange(AttackInfo info)
        {
            foreach (var c in control.Ragdoll_Data.bodyParts)
            {
                float dist = Vector3.SqrMagnitude(c.transform.position - info.attacker.transform.position); //distance between target and attacker
                                                                                                            //Debug.Log(this.gameObject.name + "dist: "+ dist.ToString() );
                if (dist <= info.lethalRange)
                {
                    int index = Random.Range(0, control.Ragdoll_Data.bodyParts.Count);
                    var triggerDetector = control.Ragdoll_Data.bodyParts[index].GetComponent<TriggerDetector>();

                    damageDetector_Data.SetData(info.attacker, info.attackAbility, triggerDetector, null);
                    return true;
                }
            }
            return false;
        }

        private bool AttackIsBlocked(AttackInfo info)
        {
            if (info == damageDetector_Data.blockedAttack && damageDetector_Data.blockedAttack != null)
            {
                return damageDetector_Data.attack;
            }

            if (control.animationProgress.IsRunning(typeof(Block)))
            {
                var dir = info.attacker.transform.position - control.transform.position;

                if (dir.z > 0f)
                {
                    if (control.PlayerRotation_Data.IsFacingForward())
                    {
                        return true;
                    }
                }
                else if (dir.z < 0f)
                {
                    if (!control.PlayerRotation_Data.IsFacingForward())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void TakeDamage(AttackInfo info)
        {
            if (IsDead())  //templory fix for hitting dead enemy
            {
                if (!info.registeredTargets.Contains(this.control))
                {
                    info.registeredTargets.Add(this.control);
                    control.Ragdoll_Data.AddForceToDamagedPart(true);
                }
                return;
            }

            if (AttackIsBlocked(info))
            {
                damageDetector_Data.blockedAttack = info;
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

                        vfx.transform.position = damageDetector_Data.attackingPart.transform.position;

                        vfx.SetActive(true);

                        if (info.attacker.PlayerRotation_Data.IsFacingForward())
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
            damageDetector_Data.hp -= info.attackAbility.damage;

            AttackManager.Instance.ForceDeregester(control);
            control.animationProgress.currentRunningAbilities.Clear();


            if (IsDead())
            {
                control.Ragdoll_Data.ragdollTriggered = true;
            }
            else
            {
                var rand = Random.Range(0, _hitReactionList.Count);

                control.skinnedMeshAnimator.runtimeAnimatorController = null; //need this to restart the animation if get several hits in a short period of time
                control.skinnedMeshAnimator.runtimeAnimatorController = _hitReactionList[rand];
            }


            if (!info.registeredTargets.Contains(this.control))
            {
                info.registeredTargets.Add(this.control);
            }
        }

        private bool IsDead()
        {
            if (damageDetector_Data.hp <= 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}