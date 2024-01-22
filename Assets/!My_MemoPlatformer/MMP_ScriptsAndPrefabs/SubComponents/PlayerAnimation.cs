using System.Collections.Generic;

namespace My_MemoPlatformer
{
    public class PlayerAnimation : SubComponent
    {
        public Animation_Data animation_Data;
        void Start()
        {
            animation_Data = new Animation_Data
            {
                currentState = null,
                instantTransitionMade = false,
                currentRunningAbilities = new Dictionary<CharacterAbility, int>(),
                IsRunning = IsRunning,
            };

            subComponentProcessor.animation_Data = animation_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.PLAYER_ANIMATION] = this;
        }
        public override void OnFixedUpdate()
        {
            animation_Data.currentState = SetCurrentState();
        }

        private string SetCurrentState()
        {
            return HashManager.Instance.GetStateNameByHash<StatesList>(Control.skinnedMeshAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash);
        }

        public override void OnUpdate()
        {
            if (IsRunning(typeof(LockTransition)))
            {
                if (Control.animationProgress.lockTransition)
                {
                    Control.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.LockTransition], true);
                }
                else
                {
                    Control.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.LockTransition], false);
                }
            }
            else
            {
                Control.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.LockTransition], false);
            }
        }
        private bool IsRunning(System.Type type) //ability is running now?
        {
            foreach (KeyValuePair<CharacterAbility, int> data in animation_Data.currentRunningAbilities)
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

