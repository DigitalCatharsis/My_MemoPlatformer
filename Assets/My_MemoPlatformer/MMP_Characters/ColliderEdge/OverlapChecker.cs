using UnityEngine;

namespace My_MemoPlatformer
{
    public class OverlapChecker : MonoBehaviour
    {
        private CharacterControl _control;
        public Collider[] arrColliders;
        public bool objIsOverlapping;

        private void Start()
        {
            _control = this.transform.root.gameObject.GetComponent<CharacterControl>();
        }
        private void FixedUpdate()
        {
            if (_control.PlayerJump_Data.checkWallBlock)
            {
                if (_control.collisionSpheres.frontOverlapCheckers.Contains(this))
                {
                    objIsOverlapping = CheckObj();
                }
            }
            else
            {
                objIsOverlapping = false;
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

