using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyMemoPlatformer
{

    public class CharacterStateBase : StateMachineBehaviour
    {
        private CharacterControl _characterControl;


        public CharacterControl GetCharacterControl(Animator animator)
        {
            if (_characterControl == null)
            {
                _characterControl = animator.GetComponentInParent<CharacterControl>();  //Находит у владельца контреллер
            }
            return _characterControl;
        }

    }
}