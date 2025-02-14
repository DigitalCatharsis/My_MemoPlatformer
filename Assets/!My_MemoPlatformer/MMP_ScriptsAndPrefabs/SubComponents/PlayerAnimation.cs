using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class PlayerAnimation : SubComponent
    {
        public PlayerAnimation_Data animation_Data;

        private List<string> _currentList = new List<string> {};
        private List<string> _previousList = new List<string> {};
        private string _currentState;

        public override void OnComponentEnabled()
        {
            animation_Data = new PlayerAnimation_Data
            {
                animator = control.GetComponentInChildren<Animator>(),
                currentState = null,
                previousState = null,
                lockTransition = false,
                instantTransitionMade = false,
                currentRunningAbilities_Dictionary = new Dictionary<CharacterAbility, int>(),
                currentRunningAbilities_PreviewList = new List<string>(),
                PreviousRunningAbilities_PreviewList = new List<string>(),
                IsRunning = IsRunning,
                StateNameContains = StateNameContains,
            };

            subComponentProcessor.animation_Data = animation_Data;
        }

        public override void OnFixedUpdate()
        {
            if (_currentState != animation_Data.currentState)
            {
                if (_previousList != null)
                {
                    animation_Data.PreviousRunningAbilities_PreviewList.Clear();
                    animation_Data.PreviousRunningAbilities_PreviewList.AddRange(_previousList);
                }                

                animation_Data.previousState = _currentState;
                _previousList = _currentList;


                _currentState = animation_Data.currentState;
                _currentList.Clear();
                _currentList.AddRange(animation_Data.currentRunningAbilities_PreviewList);
            }

            animation_Data.currentState = SetCurrentState();
            SetListsOfCurrentAbilities_Previews(animation_Data.currentRunningAbilities_PreviewList);
            //Debug.Log(animation_Data.currentState);
        }

        private string SetCurrentState()
        {
            return HashManager.Instance.GetStateNameByHash<StatesList>(control.skinnedMeshAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash);
        }

        public override void OnUpdate()
        {
            if (IsRunning(typeof(LockTransition)))
            {
                if (control.PLAYER_ANIMATION_DATA.lockTransition)
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
            foreach (KeyValuePair<CharacterAbility, int> data in animation_Data.currentRunningAbilities_Dictionary)
            {
                if (data.Key.GetType() == type)
                {
                    return true;
                }
            }
            return false;
        }

        private void SetListsOfCurrentAbilities_Previews(List<string> currentList)
        {
            currentList.Clear();

            foreach (KeyValuePair<CharacterAbility, int> data in animation_Data.currentRunningAbilities_Dictionary)
            {
                currentList.Add(data.Key.ToString());
            }
        }
        public bool StateNameContains(string str)
        {
            AnimatorClipInfo[] arr = control.skinnedMeshAnimator.GetCurrentAnimatorClipInfo(0); //have only one layer which is zero

            foreach (var clipinfo in arr)
            {
                if (clipinfo.clip.name.Contains(str))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

