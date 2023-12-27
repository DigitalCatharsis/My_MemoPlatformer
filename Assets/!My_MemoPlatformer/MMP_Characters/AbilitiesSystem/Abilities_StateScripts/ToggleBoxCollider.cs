//using System.Diagnostics.Eventing.Reader;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/ToggleBoxCollider")]
    public class ToggleBoxCollider : CharacterAbility
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
            control.RIGID_BODY.velocity = Vector3.zero;
            control.GetComponent<BoxCollider>().enabled= on;

            if (repositionSpheres)
            {
                control.COLLISION_SPHERES_DATA.Reposition_FrontSpheres();
                control.COLLISION_SPHERES_DATA.Reposition_BottomSpheres();
                control.COLLISION_SPHERES_DATA.Reposition_BackSpheres();
                control.COLLISION_SPHERES_DATA.Reposition_UpSpheres();
            }
        }
    }
}
