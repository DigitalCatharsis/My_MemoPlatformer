using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class CharacterState : StateMachineBehaviour
    {
        private CharacterControl _characterControl;

        public List<StateData> ListAbilityData = new List<StateData>();    

        public void UpdateAll(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            foreach (StateData d in ListAbilityData)
            {
                d.UpdateAbility(characterState, animator, stateInfo);
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (StateData d in ListAbilityData)
            {
                d.OnEnter(this, animator, stateInfo);
            }
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (StateData d in ListAbilityData)
            {
                UpdateAll(this, animator, stateInfo);
            }
            
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (StateData d in ListAbilityData)
            {
                d.OnExit(this, animator, stateInfo);
            }
        }

        public CharacterControl GetCharacterControl(Animator animator) //Глобальный метод для поиска аниматора у родителя. Используется в дочерних стейтах
        {
            if (_characterControl == null)
            {
                _characterControl = animator.transform.root.GetComponent<CharacterControl>();  
               // _characterControl = animator.GetComponentInParent<CharacterControl>();  
            }
            return _characterControl;
        }

    }
}