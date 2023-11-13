using UnityEditor;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/MoveUp")]
    public class MoveUp : StateData
    {
        public AnimationCurve speedGraph;
        public float speed;


        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!characterState.characterControl.Rigid_Body.useGravity)
            {
                if (!UpIsBlocked(characterState.characterControl))
                {
                    characterState.characterControl.transform.Translate(Vector3.up * speed * speedGraph.Evaluate(stateInfo.normalizedTime) * Time.deltaTime);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        private bool UpIsBlocked(CharacterControl control)
        {
            foreach (GameObject o in control.collisionSpheres.upSpheres)
            {
                Debug.DrawRay(o.transform.position, control.transform.up * 0.3f, Color.yellow);

                RaycastHit hit;
                if (Physics.Raycast(o.transform.position, control.transform.up, out hit, 0.125f))
                {
                    if (hit.collider.transform.root.gameObject != control.gameObject && !Ledge.IsLedge(hit.collider.gameObject))   //not a character or a ledge
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
