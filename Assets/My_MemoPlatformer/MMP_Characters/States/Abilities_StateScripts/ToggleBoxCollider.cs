//using System.Diagnostics.Eventing.Reader;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/ToggleBoxCollider")]
    public class ToggleBoxCollider : StateData
    {
        public bool on;
        public bool onStart;
        public bool onEnd;

        [Space(10)]
        public bool repositionSpheres;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (onStart)
            {
                
                ToggleBoxCol(characterState.characterControl);
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (onEnd)
            {
                
                ToggleBoxCol(characterState.characterControl);
            }
        }

        private void ToggleBoxCol(CharacterControl control)
        {
            control.Rigid_Body.velocity = Vector3.zero;
            control.GetComponent<BoxCollider>().enabled= on;

            if (repositionSpheres)
            {
                control.collisionSpheres.Reposition_FrontSpheres();
                control.collisionSpheres.Reposition_BottomSpheres();
                control.collisionSpheres.Reposition_BackSpheres();
            }
        }
    }
}
