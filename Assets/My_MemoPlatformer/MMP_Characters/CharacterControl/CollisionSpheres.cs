using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class CollisionSpheres : MonoBehaviour
    {
        public CharacterControl owner;

        public List<GameObject> bottomSpheres = new List<GameObject>();
        public List<GameObject> frontSpheres = new List<GameObject>();
        public List<GameObject> backSpheres = new List<GameObject>();

        public List<OverlapChecker> frontOverlapCheckers = new List<OverlapChecker>();

        public void SetColliderSpheres()
        {
            //bottom
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(Resources.Load("ColliderEdge", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                bottomSpheres.Add(obj);
                obj.transform.parent = this.transform.Find("Bottom");
            }

            Reposition_BottomSpheres();

            //front
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(Resources.Load("ColliderEdge", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                frontSpheres.Add(obj);
                frontOverlapCheckers.Add(obj.GetComponent<OverlapChecker>());
                obj.transform.parent = this.transform.Find("Front");
            }

            Reposition_FrontSpheres();

            //back
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(Resources.Load("ColliderEdge", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                backSpheres.Add(obj);
                obj.transform.parent = this.transform.Find("Back");
            }

            Reposition_BackSpheres();
        }
        public void Reposition_BackSpheres()
        {
            float bottom = owner.boxCollider.bounds.center.y - owner.boxCollider.bounds.size.y / 2; // в центре внизу. 
            float top = owner.boxCollider.bounds.center.y + owner.boxCollider.bounds.size.y / 2; // в центре вверху.
            float back = owner.boxCollider.bounds.center.z - owner.boxCollider.bounds.size.z / 2; // в центре спереди.

            backSpheres[0].transform.localPosition = new Vector3(0f, bottom + 0.05f, back) - this.transform.position;
            backSpheres[1].transform.localPosition = new Vector3(0f, top, back) - this.transform.position;

            float interval = (top - bottom + 0.05f) / 9;

            for (int i = 2; i < backSpheres.Count; i++)
            {
                backSpheres[i].transform.localPosition = new Vector3(0f, bottom + (interval * (i - 1)), back)
                     - this.transform.position;
            }
        }

        public void Reposition_FrontSpheres()
        {
            float bottom = owner.boxCollider.bounds.center.y - owner.boxCollider.bounds.size.y / 2; // в центре внизу. 
            float top = owner.boxCollider.bounds.center.y + owner.boxCollider.bounds.size.y / 2; // в центре вверху. ;
            float front = owner.boxCollider.bounds.center.z + owner.boxCollider.bounds.size.z / 2; // в центре спереди. ;
            //float back = boxCollider.bounds.center.z - boxCollider.bounds.size.z; // в центре сзади. ;;

            frontSpheres[0].transform.localPosition = new Vector3(0f, bottom + 0.05f, front) - this.transform.position;
            frontSpheres[1].transform.localPosition = new Vector3(0f, top, front) - this.transform.position;

            float interval = (top - bottom + 0.05f) / 9;

            for (int i = 2; i < frontSpheres.Count; i++)
            {
                frontSpheres[i].transform.localPosition = new Vector3(0f, bottom + (interval * (i - 1)), front)
                     - this.transform.position;
            }
        }

        public void Reposition_BottomSpheres()
        {
            float bottom = owner.boxCollider.bounds.center.y - owner.boxCollider.bounds.size.y / 2; // в центре внизу. 
            //float top = boxCollider.bounds.center.y + boxCollider.bounds.size.y; // в центре вверху. ;
            float front = owner.boxCollider.bounds.center.z + owner.boxCollider.bounds.size.z / 2; // в центре спереди. ;
            float back = owner.boxCollider.bounds.center.z - owner.boxCollider.bounds.size.z / 2; // в центре сзади. ;;

            bottomSpheres[0].transform.localPosition = new Vector3(0f, bottom, back) - this.transform.position;
            bottomSpheres[1].transform.localPosition = new Vector3(0f, bottom, front) - this.transform.position;

            float interval = (front - back) / 4;

            for (int i = 2; i < bottomSpheres.Count; i++)
            {
                bottomSpheres[i].transform.localPosition = new Vector3(0f, bottom, back + (interval * (i - 1)))
                     - this.transform.position;
            }
        }


    }
}

