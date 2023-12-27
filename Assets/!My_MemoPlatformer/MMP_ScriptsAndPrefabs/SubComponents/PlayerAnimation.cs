using My_MemoPlatformer;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class PlayerAnimation : SubComponent
    {
        PlayerAnimation_Data playerAnimation_Data;
        void Start()
        {
            playerAnimation_Data = new PlayerAnimation_Data
            {
                currentRunningAbilities = new Dictionary<StateData, int>(),
                IsRunning = IsRunning,
            };

            subComponentProcessor.playerAnimation_Data = playerAnimation_Data;
            subComponentProcessor.subcomponentsDictionary.Add(SubComponentType.PLAYER_ANIMATION, this);
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
                    control.skinnedMeshAnimator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.LockTransition], true);
                }
                else
                {
                    control.skinnedMeshAnimator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.LockTransition], false);
                }
            }
            else
            {
                control.skinnedMeshAnimator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.LockTransition], false);
            }
        }
        private bool IsRunning(System.Type type) //ability is running now?
        {
            foreach (KeyValuePair<StateData, int> data in playerAnimation_Data.currentRunningAbilities)
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

