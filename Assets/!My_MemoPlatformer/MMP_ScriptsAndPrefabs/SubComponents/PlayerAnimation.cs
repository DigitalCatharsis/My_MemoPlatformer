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
                instantTransitionMade = false,
                currentRunningAbilities = new Dictionary<CharacterAbility, int>(),
                IsRunning = IsRunning,
            };

            subComponentProcessor.animation_Data = animation_Data;
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

