#if UNITY_EDITOR
using UnityEditor.Animations;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New State", menuName = "My_MemoPlatformer/Helper/AbilitySearch")]
    public class AbilitySearch : ScriptableObject
    {
        public AnimatorController controllerToSearch;
        public CharacterAbility abilityToSearch;

        public void Search()
        {
            CharacterState[] arr = controllerToSearch.GetBehaviours<CharacterState>();

            bool AbilityFound = false;
            int totalUses = 0;

            foreach (CharacterState state in arr)
            {
                foreach (CharacterAbility ability in state.arrAbilities)
                {
                    if (ability == abilityToSearch)
                    {
                        controllerToSearch.GetHashCode();
                        Debug.Log("<color=yellow>===============================</color>");
                        Debug.Log("<color=yellow>---Ability Set Found---</color>");

                        foreach (CharacterAbility a in state.arrAbilities)
                        {
                            Debug.Log($"<color=magenta>{a.name}</color>");
                        }

                        totalUses++;
                        AbilityFound = true;
                        break;
                    }
                }
            }

            if (!AbilityFound)
            {
                Debug.Log("Ability NOT found!");
            }
            else
            {
                Debug.Log("Total uses: " + totalUses);
            }
        }
    }
}
#endif