#if UNITY_EDITOR
using UnityEditor.Animations;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New State", menuName = "My_MemoPlatformer/Helper/StateMachine_AbilityListToArray")]
    public class AbilityListToArray : ScriptableObject
    {
        public AnimatorController targetAnimator;

        public void Convert()
        {
            try
            {
                CharacterState[] arr = targetAnimator.GetBehaviours<CharacterState>();

                foreach (CharacterState state in arr)
                {
                    if (state.ListAbilityData.Count != 0)
                    {
                        Debug.Log("List to array: " + state.name);
                        state.PutStatesInArray();
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("Convert failed: " + e);
            }
        }

        public void ClearLists()
        {
            CharacterState[] arr = targetAnimator.GetBehaviours<CharacterState>();

            foreach (CharacterState state in arr)
            {
                Debug.Log("Clearing list: " + state.name);
                state.ListAbilityData.Clear();
            }
        }
    }
}
#endif