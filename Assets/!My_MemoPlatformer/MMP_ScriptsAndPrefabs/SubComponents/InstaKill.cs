using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class InstaKill : SubComponent
    {
        public InstaKill_Data instaKill_Data;

        [Header("Setup")]
        [Range(0,1)]
        [SerializeField] private float _instakillChance;

        [Header("Setup RuntimeAnimatorController")]
        [SerializeField] private RuntimeAnimatorController Assassination_Assassin;
        [SerializeField] private RuntimeAnimatorController Assassination_Victim;

        private void OnEnable()
        {
            instaKill_Data = new InstaKill_Data
            {
                Animation_Assassin = Assassination_Assassin,
                Animation_Victim = Assassination_Victim,
                ProcessInstakill = ProcessInstaKill,
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
            //not a player
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
                foreach (var collider in data.Value)
                {
                    var control = CharacterManager.Instance.GetCharacter(collider.transform.root.gameObject);

                    if (control == Control) //if self, check next
                    {
                        continue;
                    }

                    if (control.subComponentProcessor.arrSubComponents[(int)SubComponentType.MANUAL_INPUT] == null) //has to be a player, if not - next
                    {
                        continue;
                    }

                    if (!control.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
                    {
                        continue;
                    }

                    if (control.PLAYER_ANIMATION_DATA.IsRunning(typeof(Attack)))
                    {
                        continue;
                    }

                    if (Control.PLAYER_ANIMATION_DATA.IsRunning(typeof(Attack)))
                    {
                        continue;
                    }

                    if (control.PLAYER_ANIMATION_DATA.StateNameContains("RunningSlide"))
                    {
                        continue;
                    }

                    if (control.DAMAGE_DATA.IsDead())
                    {
                        continue;
                    }

                    if (Control.DAMAGE_DATA.IsDead())
                    {
                        continue;
                    }

                    if (InstakillChanceRandomizer())
                    {
                        if (DebugContainer_Data.Instance.debug_Instakill)
                        {
                            Debug.Log("InstaKill");
                        }

                        control.INSTA_KILL_DATA.ProcessInstakill(Control);
                    }

                    return;
                }
            }
        }

        private void ProcessInstaKill(CharacterControl assassin)
        {
            Control.PLAYER_ANIMATION_DATA.currentRunningAbilities_Dictionary.Clear();
            assassin.PLAYER_ANIMATION_DATA.currentRunningAbilities_Dictionary.Clear();

            Control.rigidBody.useGravity = false;
            Control.boxCollider.enabled = false;
            Control.skinnedMeshAnimator.runtimeAnimatorController = Control.INSTA_KILL_DATA.Animation_Victim;

            assassin.rigidBody.useGravity = false;
            assassin.boxCollider.enabled = false;
            assassin.skinnedMeshAnimator.runtimeAnimatorController = assassin.INSTA_KILL_DATA.Animation_Assassin;

            var dir = Control.transform.position - assassin.transform.position;

            if (dir.z < 0f)
            {
                assassin.ROTATION_DATA.FaceForward(false);
            }
            else if (dir.z > 0f)
            {
                assassin.ROTATION_DATA.FaceForward(true);
            }

            Control.transform.LookAt(Control.transform.position + (assassin.transform.forward * 5f), Vector3.up);
            Control.transform.position = assassin.transform.position + (assassin.transform.forward * 0.45f);

            Control.DAMAGE_DATA.currentHp = 0f;
        }
        public bool InstakillChanceRandomizer()
        {
            if (Random.Range(0f, 1f) < _instakillChance )
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