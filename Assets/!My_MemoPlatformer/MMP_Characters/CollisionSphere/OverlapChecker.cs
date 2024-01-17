using UnityEngine;

namespace My_MemoPlatformer
{
    public class OverlapChecker : MonoBehaviour
    {
        private CharacterControl _control;
        public Collider[] arrColliders;
        public bool isObjOverlapping;

        private void Start()
        {
            _control = this.transform.root.gameObject.GetComponent<CharacterControl>();
        }

        public void UpdateChecker()
        {
            if (_control == null)
            {
                return;
            }

            if (_control.JUMP_DATA.checkWallBlock)  //if transition condition
            {
                if (_control.COLLISION_SPHERE_DATA.IsFrontSphereContainsOverlapChecker(this))
                {
                    isObjOverlapping = CheckObj();
                }
            }
            else
            {
                isObjOverlapping = false;
            }
        }

        private bool CheckObj()
        {
            arrColliders = Physics.OverlapSphere(this.transform.position, 0.13f); //less than colliderEdge radius

            foreach (var collider in arrColliders)
            {
                if (CharacterManager.Instance.GetCharacter(collider.transform.root.gameObject) == null)  //non character
                {
                    return true;
                }
            }
            return false;
        }
    }
}

