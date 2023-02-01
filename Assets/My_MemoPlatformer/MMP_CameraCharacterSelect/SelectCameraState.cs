using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{



    public class SelectCameraState : StateMachineBehaviour
    {

        // Обнуляет параметр выбранного игрока в аниматоре по окончании анимации
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PlayableCharacterType[] arr = System.Enum.GetValues(typeof(PlayableCharacterType)) as PlayableCharacterType[];
        

        foreach (PlayableCharacterType p in arr)
            {
                animator.SetBool(p.ToString(), false);
            }
        }
    }
}