using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class CollisionSpheres : SubComponent
    {
        public List<GameObject> bottomSpheres = new List<GameObject>();
        public List<GameObject> frontSpheres = new List<GameObject>();
        public List<GameObject> backSpheres = new List<GameObject>();
        public List<GameObject> upSpheres = new List<GameObject>();

        public List<OverlapChecker> frontOverlapCheckers = new List<OverlapChecker>();
        public List<OverlapChecker> allOverlapCheckers = new List<OverlapChecker>();

        private void Start()
        {
            subComponentProcessor.subcomponentsDictionary.Add(SubComponentType.COLLISION_SPHERES, this);
        }

        public override void OnUpdate()
        {
        }

        public override void OnFixedUpdate()
        {
            foreach (OverlapChecker c in allOverlapCheckers)
            {
                c.UpdateChecker();
            }
        }

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

            //top
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(Resources.Load("ColliderEdge", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                upSpheres.Add(obj);
                obj.transform.parent = this.transform.Find("Bottom");
            }

            Reposition_UpSpheres();

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

            //add every overlapChecker
            var arr = this.gameObject.GetComponentsInChildren<OverlapChecker>();
            allOverlapCheckers.Clear();
            allOverlapCheckers.AddRange(arr);

        }
        public void Reposition_BackSpheres()
        {
            float bottom = control.boxCollider.bounds.center.y - (control.boxCollider.bounds.size.y / 2f); // в центре внизу. 
            float top = control.boxCollider.bounds.center.y + (control.boxCollider.bounds.size.y / 2f); // в центре вверху.
            float back = control.boxCollider.bounds.center.z - (control.boxCollider.bounds.size.z / 2f); // в центре спереди.

            backSpheres[0].transform.localPosition = new Vector3(0f, bottom + 0.05f, back) - control.transform.position;
            backSpheres[1].transform.localPosition = new Vector3(0f, top, back) - control.transform.position;

            float interval = (top - bottom + 0.05f) / 9;

            for (int i = 2; i < backSpheres.Count; i++)
            {
                backSpheres[i].transform.localPosition = new Vector3(0f, bottom + (interval * (i - 1)), back)
                     - control.transform.position;
            }
        }

        public void Reposition_FrontSpheres()
        {
            float bottom = control.boxCollider.bounds.center.y - (control.boxCollider.bounds.size.y / 2f); // в центре внизу. 
            float top = control.boxCollider.bounds.center.y + (control.boxCollider.bounds.size.y / 2f); // в центре вверху. ;
            float front = control.boxCollider.bounds.center.z + (control.boxCollider.bounds.size.z / 2f); // в центре спереди. ;
            //float back = boxCollider.bounds.center.z - boxCollider.bounds.size.z; // в центре сзади. ;;

            frontSpheres[0].transform.localPosition = new Vector3(0f, bottom + 0.05f, front) - control.transform.position;
            frontSpheres[1].transform.localPosition = new Vector3(0f, top, front) - control.transform.position;

            float interval = (top - bottom + 0.05f) / 9;

            for (int i = 2; i < frontSpheres.Count; i++)
            {
                frontSpheres[i].transform.localPosition = new Vector3(0f, bottom + (interval * (i - 1)), front)
                     - control.transform.position;
            }
        }

        public void Reposition_BottomSpheres()
        {
            float bottom = control.boxCollider.bounds.center.y - (control.boxCollider.bounds.size.y / 2f); // в центре внизу. 
            //float top = boxCollider.bounds.center.y + boxCollider.bounds.size.y; // в центре вверху. ;
            float front = control.boxCollider.bounds.center.z + (control.boxCollider.bounds.size.z / 2f); // в центре спереди. ;
            float back = control.boxCollider.bounds.center.z - (control.boxCollider.bounds.size.z / 2f); // в центре сзади. ;;

            bottomSpheres[0].transform.localPosition = new Vector3(0f, bottom, back) - control.transform.position;
            bottomSpheres[1].transform.localPosition = new Vector3(0f, bottom, front) - control.transform.position;

            float interval = (front - back) / 4;

            for (int i = 2; i < bottomSpheres.Count; i++)
            {
                bottomSpheres[i].transform.localPosition = new Vector3(0f, bottom, back + (interval * (i - 1)))
                     - control.transform.position;
            }
        }
        public void Reposition_UpSpheres()
        {
            float top = control.boxCollider.bounds.center.y + (control.boxCollider.bounds.size.y / 2f); // в центре вверху. 
            float front = control.boxCollider.bounds.center.z + (control.boxCollider.bounds.size.z / 2f); // в центре спереди. ;
            float back = control.boxCollider.bounds.center.z - (control.boxCollider.bounds.size.z / 2f); // в центре сзади. ;;

            upSpheres[0].transform.localPosition = new Vector3(0f, top, back) - control.transform.position;
            upSpheres[1].transform.localPosition = new Vector3(0f, top, front) - control.transform.position;

            float interval = (front - back) / 4;

            for (int i = 2; i < upSpheres.Count; i++)
            {
                upSpheres[i].transform.localPosition = new Vector3(0f, top, back + (interval * (i - 1)))
                     - control.transform.position;
            }
        }
    }
}

