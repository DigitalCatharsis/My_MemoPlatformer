using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class CollisionSpheres : SubComponent
    {
        public CollisionSpheres_Data collisionSpheres_Data;

        private void Start()
        {
            collisionSpheres_Data = new CollisionSpheres_Data
            {
                bottomSpheres = new List<GameObject>(),
                frontSpheres = new List<GameObject>(),
                upSpheres = new List<GameObject>(),
                backSpheres = new List<GameObject>(),

                frontOverlapCheckers = new List<OverlapChecker>(),
                allOverlapCheckers = new List<OverlapChecker>(),

                Reposition_BackSpheres = Reposition_BackSpheres,
                Reposition_BottomSpheres = Reposition_BottomSpheres,
                Reposition_FrontSpheres = Reposition_FrontSpheres,
                Reposition_UpSpheres = Reposition_UpSpheres,
            };

            subComponentProcessor.collisionSpheres_Data = collisionSpheres_Data;
            subComponentProcessor.subcomponentsDictionary.Add(SubComponentType.COLLISION_SPHERES, this);

            SetColliderSpheres();
        }

        public override void OnUpdate()
        {
        }

        public override void OnFixedUpdate()
        {
            foreach (OverlapChecker c in collisionSpheres_Data.allOverlapCheckers)
            {
                c.UpdateChecker();
            }
        }

        private void SetColliderSpheres()
        {
            //bottom
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(Resources.Load("ColliderEdge", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                collisionSpheres_Data.bottomSpheres.Add(obj);
                obj.transform.parent = this.transform.Find("Bottom");
            }

            Reposition_BottomSpheres();

            //top
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(Resources.Load("ColliderEdge", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                collisionSpheres_Data.upSpheres.Add(obj);
                obj.transform.parent = this.transform.Find("Bottom");
            }

            Reposition_UpSpheres();

            //front
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(Resources.Load("ColliderEdge", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                collisionSpheres_Data.frontSpheres.Add(obj);
                collisionSpheres_Data.frontOverlapCheckers.Add(obj.GetComponent<OverlapChecker>());
                obj.transform.parent = this.transform.Find("Front");
            }

            Reposition_FrontSpheres();

            //back
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(Resources.Load("ColliderEdge", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                collisionSpheres_Data.backSpheres.Add(obj);
                obj.transform.parent = this.transform.Find("Back");
            }

            Reposition_BackSpheres();

            //add every overlapChecker
            var arr = this.gameObject.GetComponentsInChildren<OverlapChecker>();
            collisionSpheres_Data.allOverlapCheckers.Clear();
            collisionSpheres_Data.allOverlapCheckers.AddRange(arr);

        }
        private void Reposition_BackSpheres()
        {
            float bottom = control.boxCollider.bounds.center.y - (control.boxCollider.bounds.size.y / 2f); // в центре внизу. 
            float top = control.boxCollider.bounds.center.y + (control.boxCollider.bounds.size.y / 2f); // в центре вверху.
            float back = control.boxCollider.bounds.center.z - (control.boxCollider.bounds.size.z / 2f); // в центре спереди.

            collisionSpheres_Data.backSpheres[0].transform.localPosition = new Vector3(0f, bottom + 0.05f, back) - this.transform.position;
            collisionSpheres_Data.backSpheres[1].transform.localPosition = new Vector3(0f, top, back) - this.transform.position;

            float interval = (top - bottom + 0.05f) / 9;

            for (int i = 2; i < collisionSpheres_Data.backSpheres.Count; i++)
            {
                collisionSpheres_Data.backSpheres[i].transform.localPosition = new Vector3(0f, bottom + (interval * (i - 1)), back)
                     - this.transform.position;
            }
        }

        private void Reposition_FrontSpheres()
        {
            float bottom = control.boxCollider.bounds.center.y - (control.boxCollider.bounds.size.y / 2f); // в центре внизу. 
            float top = control.boxCollider.bounds.center.y + (control.boxCollider.bounds.size.y / 2f); // в центре вверху. ;
            float front = control.boxCollider.bounds.center.z + (control.boxCollider.bounds.size.z / 2f); // в центре спереди. ;
            //float back = boxCollider.bounds.center.z - boxCollider.bounds.size.z; // в центре сзади. ;;

            collisionSpheres_Data.frontSpheres[0].transform.localPosition = new Vector3(0f, bottom + 0.05f, front) - this.transform.position;
            collisionSpheres_Data.frontSpheres[1].transform.localPosition = new Vector3(0f, top, front) - this.transform.position;

            float interval = (top - bottom + 0.05f) / 9;

            for (int i = 2; i < collisionSpheres_Data.frontSpheres.Count; i++)
            {
                collisionSpheres_Data.frontSpheres[i].transform.localPosition = new Vector3(0f, bottom + (interval * (i - 1)), front)
                     - this.transform.position;
            }
        }

        private void Reposition_BottomSpheres()
        {
            float bottom = control.boxCollider.bounds.center.y - (control.boxCollider.bounds.size.y / 2f); // в центре внизу. 
            //float top = boxCollider.bounds.center.y + boxCollider.bounds.size.y; // в центре вверху. ;
            float front = control.boxCollider.bounds.center.z + (control.boxCollider.bounds.size.z / 2f); // в центре спереди. ;
            float back = control.boxCollider.bounds.center.z - (control.boxCollider.bounds.size.z / 2f); // в центре сзади. ;;

            collisionSpheres_Data.bottomSpheres[0].transform.localPosition = new Vector3(0f, bottom, back) - this.transform.position;
            collisionSpheres_Data.bottomSpheres[1].transform.localPosition = new Vector3(0f, bottom, front) - this.transform.position;

            float interval = (front - back) / 4;

            for (int i = 2; i < collisionSpheres_Data.bottomSpheres.Count; i++)
            {
                collisionSpheres_Data.bottomSpheres[i].transform.localPosition = new Vector3(0f, bottom, back + (interval * (i - 1)))
                     - this.transform.position;
            }
        }
        private void Reposition_UpSpheres()
        {
            float top = control.boxCollider.bounds.center.y + (control.boxCollider.bounds.size.y / 2f); // в центре вверху. 
            float front = control.boxCollider.bounds.center.z + (control.boxCollider.bounds.size.z / 2f); // в центре спереди. ;
            float back = control.boxCollider.bounds.center.z - (control.boxCollider.bounds.size.z / 2f); // в центре сзади. ;;

            collisionSpheres_Data.upSpheres[0].transform.localPosition = new Vector3(0f, top, back) - this.transform.position;
            collisionSpheres_Data.upSpheres[1].transform.localPosition = new Vector3(0f, top, front) - this.transform.position;

            float interval = (front - back) / 4;

            for (int i = 2; i < collisionSpheres_Data.upSpheres.Count; i++)
            {
                collisionSpheres_Data.upSpheres[i].transform.localPosition = new Vector3(0f, top, back + (interval * (i - 1)))
                     - this.transform.position;
            }
        }
    }
}

