using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class CharacterState : StateMachineBehaviour
    {
        private CharacterControl _characterControl;

        public List<StateData> ListAbilityData = new List<StateData>();    

        public void UpdateAll(CharacterState characterStateBase, Animator animator)
        {
            foreach (StateData d in ListAbilityData)
            {
                d.UpdateAbility(characterStateBase, animator);
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            UpdateAll(this, animator);
        }

        public CharacterControl GetCharacterControl(Animator animator) //Глобальный метод для поиска аниматора у родителя. Используется в дочерних стейтах
        {
            if (_characterControl == null)
            {
                _characterControl = animator.GetComponentInParent<CharacterControl>();  
            }
            return _characterControl;
        }

    }
}