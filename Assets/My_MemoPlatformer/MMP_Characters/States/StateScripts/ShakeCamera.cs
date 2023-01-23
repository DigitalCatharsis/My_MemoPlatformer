using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/ShakeCamera")]
    public class ShakeCamera : StateData
    {
        [Range(0f, 0.99f)][SerializeField] private float _shakeTiming;
        private bool _isShaken  = false;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (_shakeTiming == 0f) 
            {
                CameraManager.Instance.ShakeCamera(0.2f);
                _isShaken= true;
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!_isShaken)
            {
                if (stateInfo.normalizedTime >= _shakeTiming)
                {
                    _isShaken = true;
                    CameraManager.Instance.ShakeCamera(0.2f);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            _isShaken = false;
        }
    }
}
