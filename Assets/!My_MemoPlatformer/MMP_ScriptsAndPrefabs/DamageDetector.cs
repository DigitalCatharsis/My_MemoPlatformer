using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class DamageDetector : SubComponent  //Compare collision info versus attack input that is being registered
    {
        public Damage_Data damage_Data;

        [Header("Damage Setup")]
        [SerializeField] private Attack _weaponThrow;
        [SerializeField] private Attack _airStompAttack;

        [Header("HP Setup")]
        [SerializeField] private float _hp;

        static string VFX_Prefix = "VFX";

        private void Start()
        {
            damage_Data = new Damage_Data
            {
                blockedAttack = null,
                hp = _hp,
                airStompAttack = _airStompAttack,
                weaponThrow = _weaponThrow,

                damageTaken = new DamageTaken(attacker: null, attack: null, damage_TG: null, damager: null, incomingVelocity: Vector3.zero),

                IsDead = IsDead,
                TakeDamage = ProcessDamage,
                TakeCollateralDamage = TakeCollateralDamage,

                AddCollidersToDictionary = AddCollidersToDictionary,
                RemoveCollidersFromDictionary = RemoveCollidersFromDictionary,
            };

            subComponentProcessor.damage_Data = damage_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.DAMAGE_DETECTOR] = this;
        }

        public override void OnFixedUpdate()
        {
            if (AttackManager.Instance.currentAttacks.Count > 0)
            {
                CheckAttack();
            }
        }

        public override void OnUpdate()
        {
        }

        private bool AttackIsValid(AttackCondition info)
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
                var vec = this.transform.position - info.attacker.transform.position;  //Вектор от жертвы до нападающего
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
            foreach (var info in AttackManager.Instance.currentAttacks)
            {
                if (AttackIsValid(info))
                {
                    if (info.mustCollide)
                    {
                        if (control.DAMAGE_DATA.collidingBodyParts_Dictionary.Count != 0)
                        {
                            if (IsCollided(info))
                            {
                                ProcessDamage(info);
                            }
                        }
                    }
                    else
                    {
                        if (IsInLethalRange(info))
                        {
                            ProcessDamage(info);
                        }
                    }
                }
            }
        }

        private bool IsCollided(AttackCondition info)
        {
            foreach (KeyValuePair<TriggerDetector, List<Collider>> data in control.DAMAGE_DATA.collidingBodyParts_Dictionary)
            {
                foreach (var collider in data.Value)
                {
                    foreach (var part in info.attackParts)
                    {
                        if (info.attacker.GetAttackingPart(part) == collider.gameObject)
                        {
                            damage_Data.damageTaken = new DamageTaken(
                                info.attacker,
                                info.attackAbility,
                                data.Key,
                                damager: info.attacker.GetAttackingPart(part),
                                incomingVelocity: Vector3.zero);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool IsInLethalRange(AttackCondition info)
        {
            for (int i = 0; i < control.RAGDOLL_DATA.arrBodyParts.Length; i++)
            {
                float dist = Vector3.SqrMagnitude(control.RAGDOLL_DATA.arrBodyParts[i].transform.position - info.attacker.transform.position);

                if (dist <= info.lethalRange)
                {
                    int index = Random.Range(0, control.RAGDOLL_DATA.arrBodyParts.Length);
                    var triggerDetector = control.RAGDOLL_DATA.arrBodyParts[index].GetComponent<TriggerDetector>();

                    damage_Data.damageTaken = new DamageTaken(
                        info.attacker,
                        info.attackAbility,
                        triggerDetector,
                        damager: null,
                        incomingVelocity: Vector3.zero);

                    return true;
                }
            }

            return false;
        }

        private bool AttackIsBlocked(AttackCondition info)
        {
            if (info == damage_Data.blockedAttack && damage_Data.blockedAttack != null)
            {
                return true;
            }

            if (control.ANIMATION_DATA.IsRunning(typeof(Block)))
            {
                var dir = info.attacker.transform.position - control.transform.position;

                if (dir.z > 0f)
                {
                    if (control.ROTATION_DATA.IsFacingForward())
                    {
                        return true;
                    }
                }
                else if (dir.z < 0f)
                {
                    if (!control.ROTATION_DATA.IsFacingForward())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void ProcessDamage(AttackCondition info)
        {
            if (IsDead())
            {
                PushDeadBody(info);
            }
            else
            {
                if (!AttackIsBlocked(info))
                {
                    TakeDamage(info);
                }
            }
        }

        private void PushDeadBody(AttackCondition info)
        {
            if (!info.registeredTargets.Contains(this.control))
            {
                if (info.attackAbility.collateralDamageInfo.createCollateral)
                {
                    ShowHitParticles(info.attacker, info.attackAbility.particleType);
                    ProcessFlyingRagdoll(info);
                }

                info.registeredTargets.Add(this.control);
                control.RAGDOLL_DATA.ClearExistingVelocity();
                control.RAGDOLL_DATA.AddForceToDamagedPart(RagdollPushType.DEAD_BODY);
            }

            return;
        }

        private void TakeDamage(AttackCondition info)
        {
            ProcessHitParticles(info);

            info.currentHits++;
            damage_Data.hp -= info.attackAbility.damage;

            AttackManager.Instance.ForceDeregister(control);
            control.ANIMATION_DATA.currentRunningAbilities.Clear();

            if (IsDead())
            {
                control.RAGDOLL_DATA.ragdollTriggered = true;
            }
            else
            {
                var randomIndex = Random.Range(0, (int)Hit_Reaction_States.COUNT);
                control.skinnedMeshAnimator.Play(HashManager.Instance.dicHitReactionStates[(Hit_Reaction_States)randomIndex], 0, 0f);
            }

            ProcessFlyingRagdoll(info);

            if (!info.registeredTargets.Contains(this.control))
            {
                info.registeredTargets.Add(this.control);
            }
        }

        private void TakeCollateralDamage(CharacterControl attacker, Collider col, TriggerDetector triggerDetector)
        {
            if (attacker.RAGDOLL_DATA.flyingRagdollData.isTriggered)
            {
                if (attacker.RAGDOLL_DATA.flyingRagdollData.attacker != control)
                {
                    var mag = Vector3.SqrMagnitude(col.attachedRigidbody.velocity);

                    if (DebugContainer_Data.Instance.debug_TriggerDetector)
                    {
                        Debug.Log("incoming ragdoll: " + attacker.gameObject.name + "\n" + "Velocity: " + mag);
                    }

                    if (mag >= 10f && col.transform.root.gameObject.GetComponent<CharacterControl>())
                    {
                        control.DAMAGE_DATA.damageTaken = new DamageTaken(
                            attacker: null,
                            attack: null,
                            damage_TG: triggerDetector,
                            damager: null,
                            incomingVelocity: col.attachedRigidbody.velocity);

                        control.DAMAGE_DATA.hp = 0;
                        control.RAGDOLL_DATA.ragdollTriggered = true;
                    }
                }
            }
        }

        private void ProcessHitParticles(AttackCondition info)
        {
            if (info.mustCollide)
            {
                CameraManager.Instance.ShakeCamera(0.3f);

                if (info.attackAbility.useDeathParticles)
                {
                    if (info.attackAbility.particleType.ToString().Contains(VFX_Prefix))
                    {
                        ShowHitParticles(info.attacker, info.attackAbility.particleType);
                    }
                }
            }
        }

        private void ShowHitParticles(CharacterControl attacker, PoolObjectType effectsType)
        {
            var vfx = PoolManager.Instance.GetObject(effectsType);

            vfx.transform.position = control.DAMAGE_DATA.damageTaken.DAMAGE_TG.triggerCollider.bounds.center;

            vfx.SetActive(true);

            if (attacker.ROTATION_DATA.IsFacingForward())
            {
                vfx.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                vfx.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

        private bool IsDead()
        {
            if (damage_Data.hp <= 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ProcessFlyingRagdoll(AttackCondition info)
        {
            if (info.attackAbility.collateralDamageInfo.createCollateral)
            {
                control.RAGDOLL_DATA.flyingRagdollData.isTriggered = true;
                control.RAGDOLL_DATA.flyingRagdollData.attacker = info.attacker;
            }
        }

        private void AddCollidersToDictionary(Dictionary<TriggerDetector, List<Collider>> colliders_Dictionary, Collider collider, TriggerDetector triggerDetector)
        {
            if (!colliders_Dictionary.ContainsKey(triggerDetector))
            {
                colliders_Dictionary.Add(triggerDetector, new List<Collider>());
            }

            try
            {
                if (!colliders_Dictionary[triggerDetector].Contains(collider))
                {
                    colliders_Dictionary[triggerDetector].Add(collider);
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex);
            }
        }

        private void RemoveCollidersFromDictionary(Dictionary<TriggerDetector, List<Collider>> colliders_Dictionary, Collider collider, TriggerDetector triggerDetector)
        {
            if (colliders_Dictionary[triggerDetector].Contains(collider))
            {
                colliders_Dictionary[triggerDetector].Remove(collider);
            }

            if (colliders_Dictionary[triggerDetector].Count == 0)
            {
                colliders_Dictionary.Remove(triggerDetector);
            }
        }
    }
}