using My_MemoPlatformer;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class PlayerAnimation : SubComponent
    {
        Animation_Data playerAnimation_Data;
        void Start()
        {
            playerAnimation_Data = new Animation_Data
            {
                instantTransitionMade = false,
                currentRunningAbilities = new Dictionary<CharacterAbility, int>(),
                IsRunning = IsRunning,
            };

            subComponentProcessor.animation_Data = playerAnimation_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.PLAYER_ANIMATION] = this;
        }
        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {
            if (IsRunning(typeof(LockTransition)))
            {
                if (control.animationProgress.lockTransition)
                {
                    control.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.LockTransition], true);
                }
                else
                {
                    control.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.LockTransition], false);
                }
            }
            else
            {
                control.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.LockTransition], false);
            }
        }
        private bool IsRunning(System.Type type) //ability is running now?
        {
            foreach (KeyValuePair<CharacterAbility, int> data in playerAnimation_Data.currentRunningAbilities)
            {
                if (data.Key.GetType() == type)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

