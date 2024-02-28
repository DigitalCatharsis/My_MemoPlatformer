using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public override void OnComponentEnabled()
        {
            instaKill_Data = new InstaKill_Data
            {
                Animation_Assassin = Assassination_Assassin,
                Animation_Victim = Assassination_Victim,
                ProcessInstakill = ProcessInstaKill,
            };

            subComponentProcessor.instaKill_Data = instaKill_Data;
        }
        public override void OnUpdate()
        {
        }

        public override void OnFixedUpdate()
        {
            //not a player
            if (control.AICONTROLLER_DATA.aiType == AI_Type.Player)
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
                foreach (var collider in data.Value)
                {
                    var control = CharacterManager.Instance.GetCharacter(collider.transform.root.gameObject);

                    if (control == base.control) //if self, check next
                    {
                        continue;
                    }

                    if (control.AICONTROLLER_DATA.aiType != AI_Type.Player) //has to be a player, if not - next
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

                    if (base.control.PLAYER_ANIMATION_DATA.IsRunning(typeof(Attack)))
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

                    if (base.control.DAMAGE_DATA.IsDead())
                    {
                        continue;
                    }

                    if (InstakillChanceRandomizer())
                    {
                        if (DebugContainer_Data.Instance.debug_Instakill)
                        {
                            Debug.Log("InstaKill");
                        }

                        control.INSTA_KILL_DATA.ProcessInstakill(base.control);
                    }

                    return;
                }
            }
        }

        private void ProcessInstaKill(CharacterControl assassin)
        {
            control.PLAYER_ANIMATION_DATA.currentRunningAbilities_Dictionary.Clear();
            assassin.PLAYER_ANIMATION_DATA.currentRunningAbilities_Dictionary.Clear();

            control.rigidBody.useGravity = false;
            control.boxCollider.enabled = false;
            control.skinnedMeshAnimator.runtimeAnimatorController = control.INSTA_KILL_DATA.Animation_Victim;

            assassin.rigidBody.useGravity = false;
            assassin.boxCollider.enabled = false;
            assassin.skinnedMeshAnimator.runtimeAnimatorController = assassin.INSTA_KILL_DATA.Animation_Assassin;

            var dir = control.transform.position - assassin.transform.position;

            if (dir.z < 0f)
            {
                assassin.ROTATION_DATA.FaceForward(false);
            }
            else if (dir.z > 0f)
            {
                assassin.ROTATION_DATA.FaceForward(true);
            }

            control.transform.LookAt(control.transform.position + (assassin.transform.forward * 5f), Vector3.up);
            control.transform.position = assassin.transform.position + (assassin.transform.forward * 0.45f);

            control.DAMAGE_DATA.currentHp = 0f;
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