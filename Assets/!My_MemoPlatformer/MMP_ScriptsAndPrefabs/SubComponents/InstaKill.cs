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
            if (Control.subComponentProcessor.arrSubComponents[(int)SubComponentType.MANUAL_INPUT] != null)
            {
                return;
            }

            if (!Control.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
            {
                return;
            }

            //if one of bodypart is player, there gonna be instakill
            foreach (KeyValuePair<TriggerDetector, List<Collider>> data in Control.DAMAGE_DATA.collidingBodyParts_Dictionary)
            {
                foreach (var col in data.Value)
                {
                    var c = CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);

                    if (c == Control)
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

                    if (Control.ANIMATION_DATA.IsRunning(typeof(Attack)))
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

                    if (Control.DAMAGE_DATA.IsDead())
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
            Control.ANIMATION_DATA.currentRunningAbilities.Clear();
            attacker.ANIMATION_DATA.currentRunningAbilities.Clear();

            Control.RIGID_BODY.useGravity = false;
            Control.boxCollider.enabled = false;
            Control.skinnedMeshAnimator.runtimeAnimatorController = Control.INSTA_KILL_DATA.Animation_Victim;

            attacker.RIGID_BODY.useGravity = false;
            attacker.boxCollider.enabled = false;
            attacker.skinnedMeshAnimator.runtimeAnimatorController = Control.INSTA_KILL_DATA.Animation_Assassin;

            Vector3 dir = Control.transform.position - attacker.transform.position;

            if (dir.z < 0f)
            {
                attacker.ROTATION_DATA.FaceForward(false);
            }
            else if (dir.z > 0f)
            {
                attacker.ROTATION_DATA.FaceForward(true);
            }

            Control.transform.LookAt(Control.transform.position + (attacker.transform.forward * 5f), Vector3.up);
            Control.transform.position = attacker.transform.position + (attacker.transform.forward * 0.45f);

            Control.DAMAGE_DATA.currentHp = 0f;
        }
    }
}