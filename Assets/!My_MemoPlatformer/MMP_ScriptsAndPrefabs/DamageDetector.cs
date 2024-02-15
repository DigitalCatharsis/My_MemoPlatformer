using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class DamageDetector : SubComponent  //Compare collision info versus attack input that is being registered
    {
        [SerializeField] private Damage_Data _damage_Data;

        [Header("HP Setup")]
        [SerializeField] private float _hp;

        private static string _VFX_Prefix = "VFX";

        private void Start()
        {
            _damage_Data = new Damage_Data
            {
                blockedAttack = null,
                currentHp = _hp,

                damageTaken = new DamageTaken(attacker: null, attack: null, damaged_TG: null, damagerPart: null, incomingVelocity: Vector3.zero),

                IsDead = IsCharacterDead,
                TakeDamage = ProcessDamage,
                TakeCollateralDamage = ProcessCollateralDamage,

                AddCollidersToDictionary = AddCollidersToDictionary,
                RemoveCollidersFromDictionary = RemoveCollidersFromDictionary,
            };

            subComponentProcessor.damage_Data = _damage_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.DAMAGE_DETECTOR] = this;
        }

        public override void OnFixedUpdate()
        {
            if (AttackManager.Instance.currentAttacks.Count > 0)
            {
                CheckAttack();
            }
        }
        private void CheckAttack()
        {
            foreach (var attackCondition_info in AttackManager.Instance.currentAttacks)
            {
                if (IsAttackValid(attackCondition_info))
                {
                    if (attackCondition_info.mustCollide)
                    {
                        if (Control.DAMAGE_DATA.collidingBodyParts_Dictionary.Count != 0)
                        {
                            if (CheckForCollisionAndCreacteDamageTaken(attackCondition_info))
                            {
                                ProcessDamage(attackCondition_info);
                            }
                        }
                    }
                    else
                    {
                        if (CheckForLethalRangeAndCreateDamageTaken(attackCondition_info))
                        {
                            ProcessDamage(attackCondition_info);
                        }
                    }
                }
            }
        }
        private bool IsAttackValid(AttackCondition attackCondition_Info)
        {
            if (attackCondition_Info == null)
            {
                return false; ;
            }

            if (!attackCondition_Info.isRegistered)
            {
                return false;
            }

            if (attackCondition_Info.isFinished)
            {
                return false;
            }

            if (attackCondition_Info.currentHits >= attackCondition_Info.maxHits)
            {
                return false;
            }

            if (attackCondition_Info.attacker == Control)
            {
                return false;
            }

            if (attackCondition_Info.mustFaceAttacker)
            {
                var vec = this.transform.position - attackCondition_Info.attacker.transform.position;  
                if (vec.z * attackCondition_Info.attacker.transform.forward.z < 0f)
                {
                    return false;
                }
            }

            if (attackCondition_Info.registeredTargets.Contains(this.Control))  //prevent several times damage from one attack
            {
                return false;
            }

            return true;
        }

        public override void OnUpdate()
        {
        }

        private bool CheckForCollisionAndCreacteDamageTaken(AttackCondition attackCondition_Info)
        {
            foreach (KeyValuePair<TriggerDetector, List<Collider>> data in Control.DAMAGE_DATA.collidingBodyParts_Dictionary)
            {
                foreach (var collider in data.Value)
                {
                    foreach (var attackPart in attackCondition_Info.attackParts)
                    {
                        if (attackCondition_Info.attacker.GetAttackingPart(attackPart) == collider.gameObject)
                        {
                            _damage_Data.damageTaken = new DamageTaken(
                                attacker: attackCondition_Info.attacker,
                                attack: attackCondition_Info.attackAbility,
                                damaged_TG: data.Key,
                                damagerPart: attackCondition_Info.attacker.GetAttackingPart(attackPart),
                                incomingVelocity: Vector3.zero);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool CheckForLethalRangeAndCreateDamageTaken(AttackCondition attackCondition_info)
        {
            for (int i = 0; i < Control.RAGDOLL_DATA.arrBodyPartsColliders.Length; i++)
            {
                var dist = Vector3.SqrMagnitude(Control.RAGDOLL_DATA.arrBodyPartsColliders[i].transform.position - attackCondition_info.attacker.transform.position);

                if (dist <= attackCondition_info.lethalRange)
                {
                    int index = Random.Range(0, Control.RAGDOLL_DATA.arrBodyPartsColliders.Length);
                    var triggerDetector = Control.RAGDOLL_DATA.arrBodyPartsColliders[index].GetComponent<TriggerDetector>();

                    _damage_Data.damageTaken = new DamageTaken(
                        attackCondition_info.attacker,
                        attackCondition_info.attackAbility,
                        triggerDetector,
                        damagerPart: null,
                        incomingVelocity: Vector3.zero);

                    return true;
                }
            }
            return false;
        }

        private bool AttackIsBlocked(AttackCondition info)
        {
            if (info == _damage_Data.blockedAttack && _damage_Data.blockedAttack != null)
            {
                return true;
            }

            if (Control.PLAYER_ANIMATION_DATA.IsRunning(typeof(Block)))
            {
                var dir = info.attacker.transform.position - Control.transform.position;

                if (dir.z > 0f)
                {
                    if (Control.ROTATION_DATA.IsFacingForward())
                    {
                        return true;
                    }
                }
                else if (dir.z < 0f)
                {
                    if (!Control.ROTATION_DATA.IsFacingForward())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void ProcessDamage(AttackCondition info)
        {
            if (IsCharacterDead())
            {
                PushThisDeadBody(info);
            }
            else
            {
                if (!AttackIsBlocked(info))
                {
                    OnTakeDamage(info);
                }
                else
                {
                    _damage_Data.blockedAttack = info;
                    return;
                }
            }
        }

        private void PushThisDeadBody(AttackCondition attackCondition_Info)
        {
            if (!attackCondition_Info.registeredTargets.Contains(this.Control))
            {
                if (attackCondition_Info.attackAbility.collateralDamageInfo.createCollateral)
                {
                    ShowHitParticles(attackCondition_Info.attacker, attackCondition_Info.attackAbility.particleType);
                    ProcessFlyingRagdoll(attackCondition_Info);
                }

                attackCondition_Info.registeredTargets.Add(this.Control);
                Control.RAGDOLL_DATA.ClearExistingVelocity();
                Control.RAGDOLL_DATA.AddForceToDamagedPart(RagdollPushType.DEAD_BODY);
            }

            return;
        }

        private void OnTakeDamage(AttackCondition attackCondition_Info)
        {
            ProcessHitParticles(attackCondition_Info);

            attackCondition_Info.currentHits++;
            _damage_Data.currentHp -= attackCondition_Info.attackAbility.damage;

            AttackManager.Instance.ForceDeregister(Control);
            Control.PLAYER_ANIMATION_DATA.currentRunningAbilities_Dictionary.Clear();

            if (IsCharacterDead())
            {
                Control.RAGDOLL_DATA.ragdollTriggered = true;
                ProcessFlyingRagdoll(attackCondition_Info);
            }
            else
            {
                var randomIndex = Random.Range(0, (int)Hit_Reaction_States.COUNT);
                Control.skinnedMeshAnimator.Play(HashManager.Instance.dicHitReactionStates[(Hit_Reaction_States)randomIndex], 0, 0f);
            }

            if (!attackCondition_Info.registeredTargets.Contains(this.Control))
            {
                attackCondition_Info.registeredTargets.Add(this.Control);
            }
        }

        private void ProcessCollateralDamage(CharacterControl attacker, Collider col, TriggerDetector triggerDetector)
        {
            if (attacker.RAGDOLL_DATA.flyingRagdollData.isTriggered)
            {
                if (attacker.RAGDOLL_DATA.flyingRagdollData.attacker != triggerDetector.control)
                {
                    var mag = Vector3.SqrMagnitude(col.attachedRigidbody.velocity);

                    if (DebugContainer_Data.Instance.debug_TriggerDetector)
                    {
                        Debug.Log("incoming ragdoll: " + attacker.gameObject.name + " to "+ triggerDetector.control.gameObject.name + "\n" + "Velocity: " + mag);
                    }

                    if (mag >= 10f)
                    {
                        triggerDetector.control.DAMAGE_DATA.damageTaken = new DamageTaken(
                            attacker: null,
                            attack: null,
                            damaged_TG: triggerDetector,
                            damagerPart: null,
                            incomingVelocity: col.attachedRigidbody.velocity);

                        triggerDetector.control.DAMAGE_DATA.currentHp = 0;
                        triggerDetector.control.RAGDOLL_DATA.ragdollTriggered = true;
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
                    if (info.attackAbility.particleType.ToString().Contains(_VFX_Prefix))
                    {
                        ShowHitParticles(info.attacker, info.attackAbility.particleType);
                    }
                }
            }
        }

        private void ShowHitParticles(CharacterControl attacker, PoolObjectType effectsType)
        {
            var vfx = PoolManager.Instance.GetObject(effectsType);

            vfx.transform.position = Control.DAMAGE_DATA.damageTaken.DAMAGED_TG.triggerCollider.bounds.center;

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

        private bool IsCharacterDead()
        {
            if (_damage_Data.currentHp <= 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ProcessFlyingRagdoll(AttackCondition attackCondition_Info)
        {
            if (attackCondition_Info.attackAbility.collateralDamageInfo.createCollateral)
            {
                Control.RAGDOLL_DATA.flyingRagdollData.isTriggered = true;
                Control.RAGDOLL_DATA.flyingRagdollData.attacker = attackCondition_Info.attacker;
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