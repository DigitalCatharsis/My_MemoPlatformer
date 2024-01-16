using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class InstaKill : SubComponent
    {
        public InstaKill_Data instaKill_Data;

        [SerializeField] private RuntimeAnimatorController Assassination_Assassin;
        [SerializeField] private RuntimeAnimatorController Assassination_Victim;

        private void Start()
        {
            instaKill_Data = new InstaKill_Data
            {
                Animation_Assassin = Assassination_Assassin,
                Animation_Victim = Assassination_Victim,
                DeathByInstakill = DeathByInstakill,
            };

            subComponentProcessor.instaKill_Data = instaKill_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.INSTA_KILL] = this;
        }
        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnFixedUpdate()
        {
            if (control.subComponentProcessor.arrSubComponents[(int)SubComponentType.MANUAL_INPUT] != null)
            {
                return;
            }

            if (!control.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
            {
                return;
            }

            //if one of bodypart is player, there gonna be instakill
            foreach (KeyValuePair<TriggerDetector, List<Collider>> data in control.DAMAGE_DATA.collidingBodyParts_Dictionary)
            {
                foreach (var col in data.Value)
                {
                    var c = CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);

                    if (c == control)
                    {
                        continue;
                    }

                    if (c.subComponentProcessor.arrSubComponents[(int)SubComponentType.MANUAL_INPUT] == null) //has to be a player, if not - next
                    {
                        continue;
                    }

                    if (!c.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
                    {
                        continue;
                    }

                    if (c.ANIMATION_DATA.IsRunning(typeof(Attack)))
                    {
                        continue;
                    }

                    if (control.ANIMATION_DATA.IsRunning(typeof(Attack)))
                    {
                        continue;
                    }

                    if (c.animationProgress.StateNameContains("RunningSlide"))
                    {
                        continue;
                    }

                    if (c.DAMAGE_DATA.IsDead())
                    {
                        continue;
                    }

                    if (control.DAMAGE_DATA.IsDead())
                    {
                        continue;
                    }

                    if (DebugContainer_Data.Instance.debug_Instakill)
                    {
                        Debug.Log("InstaKill");
                    }

                    //c.INSTAKILL_DATA.DeathByInstakill(control);

                    return;
                }
            }
        }

        private void DeathByInstakill(CharacterControl attacker)
        {
            control.ANIMATION_DATA.currentRunningAbilities.Clear();
            attacker.ANIMATION_DATA.currentRunningAbilities.Clear();

            control.RIGID_BODY.useGravity = false;
            control.boxCollider.enabled = false;
            control.skinnedMeshAnimator.runtimeAnimatorController = control.INSTA_KILL_DATA.Animation_Victim;

            attacker.RIGID_BODY.useGravity = false;
            attacker.boxCollider.enabled = false;
            attacker.skinnedMeshAnimator.runtimeAnimatorController = control.INSTA_KILL_DATA.Animation_Assassin;

            Vector3 dir = control.transform.position - attacker.transform.position;

            if (dir.z < 0f)
            {
                attacker.ROTATION_DATA.FaceForward(false);
            }
            else if (dir.z > 0f)
            {
                attacker.ROTATION_DATA.FaceForward(true);
            }

            control.transform.LookAt(control.transform.position + (attacker.transform.forward * 5f), Vector3.up);
            control.transform.position = attacker.transform.position + (attacker.transform.forward * 0.45f);

            control.DAMAGE_DATA.hp = 0f;
        }
    }
}