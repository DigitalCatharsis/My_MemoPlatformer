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
                bottomSpheres = new GameObject[5],
                frontSpheres = new GameObject[10],
                backSpheres = new GameObject[10],
                upSpheres = new GameObject[5],

                frontOverlapCheckers = new OverlapChecker[10],
                FrontOverlapCheckerContains = IsSphereContainsOverlapChecker,

                Reposition_FrontSpheres = Reposition_FrontSpheres,
                Reposition_BottomSpheres = Reposition_BottomSpheres,
                Reposition_BackSpheres = Reposition_BackSpheres,
                Reposition_UpSpheres = Reposition_UpSpheres,
            };

            subComponentProcessor.collisionSpheres_Data = collisionSpheres_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.COLLISION_SPHERES] = this;

            SetColliderSpheres();
        }

        public override void OnUpdate()
        {
        }

        public override void OnFixedUpdate()
        {
            for (int i = 0; i < collisionSpheres_Data.allOverlapCheckers.Length; i++)
            {
                collisionSpheres_Data.allOverlapCheckers[i].UpdateChecker();
            }
        }

        private GameObject LoadCollisionSpheres()
        {
            return Instantiate(Resources.Load("CollisionSphere", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
        }

        private void SetColliderSpheres()
        {
            //bottom
            for (int i = 0; i < 5; i++)
            {
                var obj = LoadCollisionSpheres();
                collisionSpheres_Data.bottomSpheres[i] = obj;
                obj.transform.parent = this.transform.Find("Bottom");
            }

            Reposition_BottomSpheres();

            //top
            for (int i = 0; i < 5; i++)
            {
                var obj = LoadCollisionSpheres();
                collisionSpheres_Data.upSpheres[i] = obj;
                obj.transform.parent = this.transform.Find("Bottom");
            }

            Reposition_UpSpheres();

            //front
            for (int i = 0; i < 10; i++)
            {
                var obj = LoadCollisionSpheres();
                collisionSpheres_Data.frontSpheres[i] = obj;
                collisionSpheres_Data.frontOverlapCheckers[i] = obj.GetComponent<OverlapChecker>();
                obj.transform.parent = this.transform.Find("Front");
            }

            Reposition_FrontSpheres();

            //back
            for (int i = 0; i < 10; i++)
            {
                var obj = LoadCollisionSpheres();
                collisionSpheres_Data.backSpheres[i] = obj;
                obj.transform.parent = this.transform.Find("Back");
            }

            Reposition_BackSpheres();

            //add every overlapChecker
            var arr = this.gameObject.GetComponentsInChildren<OverlapChecker>();
            collisionSpheres_Data.allOverlapCheckers = arr;
        }

        private void Reposition_FrontSpheres()
        {
            var bottom = Control.boxCollider.bounds.center.y - (Control.boxCollider.bounds.size.y / 2f); // в центре внизу. 
            var top = Control.boxCollider.bounds.center.y + (Control.boxCollider.bounds.size.y / 2f); // в центре вверху. ;
            var front = Control.boxCollider.bounds.center.z + (Control.boxCollider.bounds.size.z / 2f); // в центре спереди. ;

            collisionSpheres_Data.frontSpheres[0].transform.localPosition =
                new Vector3(0f, bottom + 0.05f, front) - Control.transform.position;

            collisionSpheres_Data.frontSpheres[1].transform.localPosition =
                new Vector3(0f, top, front) - Control.transform.position;

            float interval = (top - bottom + 0.05f) / 9;

            for (int i = 2; i < collisionSpheres_Data.frontSpheres.Length; i++)
            {
                collisionSpheres_Data.frontSpheres[i].transform.localPosition =
                    new Vector3(0f, bottom + (interval * (i - 1)), front) - Control.transform.position;
            }
        }
        private void Reposition_BackSpheres()
        {
            float bottom = Control.boxCollider.bounds.center.y - (Control.boxCollider.bounds.size.y / 2f); // в центре внизу. 
            float top = Control.boxCollider.bounds.center.y + (Control.boxCollider.bounds.size.y / 2f); // в центре вверху.
            float back = Control.boxCollider.bounds.center.z - (Control.boxCollider.bounds.size.z / 2f); // в центре спереди.

            collisionSpheres_Data.backSpheres[0].transform.localPosition =
                new Vector3(0f, bottom + 0.05f, back) - Control.transform.position;

            collisionSpheres_Data.backSpheres[1].transform.localPosition =
                new Vector3(0f, top, back) - Control.transform.position;

            float interval = (top - bottom + 0.05f) / 9;

            for (int i = 2; i < collisionSpheres_Data.backSpheres.Length; i++)
            {
                collisionSpheres_Data.backSpheres[i].transform.localPosition =
                    new Vector3(0f, bottom + (interval * (i - 1)), back) - Control.transform.position;
            }
        }

        private void Reposition_BottomSpheres()
        {
            var bottom = Control.boxCollider.bounds.center.y - (Control.boxCollider.bounds.size.y / 2f);
            var front = Control.boxCollider.bounds.center.z + (Control.boxCollider.bounds.size.z / 2f);
            var back = Control.boxCollider.bounds.center.z - (Control.boxCollider.bounds.size.z / 2f);

            collisionSpheres_Data.bottomSpheres[0].transform.localPosition =
                new Vector3(0f, bottom, back) - Control.transform.position;

            collisionSpheres_Data.bottomSpheres[1].transform.localPosition =
                new Vector3(0f, bottom, front) - Control.transform.position;

            float interval = (front - back) / 4;

            for (int i = 2; i < collisionSpheres_Data.bottomSpheres.Length; i++)
            {
                collisionSpheres_Data.bottomSpheres[i].transform.localPosition =
                    new Vector3(0f, bottom, back + (interval * (i - 1))) - Control.transform.position;
            }
        }
        private void Reposition_UpSpheres()
        {
            var top = Control.boxCollider.bounds.center.y + (Control.boxCollider.bounds.size.y / 2f);
            var front = Control.boxCollider.bounds.center.z + (Control.boxCollider.bounds.size.z / 2f);
            var back = Control.boxCollider.bounds.center.z - (Control.boxCollider.bounds.size.z / 2f);

            collisionSpheres_Data.upSpheres[0].transform.localPosition =
                new Vector3(0f, top, back) - Control.transform.position;

            collisionSpheres_Data.upSpheres[1].transform.localPosition =
                new Vector3(0f, top, front) - Control.transform.position;

            float interval = (front - back) / 4;

            for (int i = 2; i < collisionSpheres_Data.upSpheres.Length; i++)
            {
                collisionSpheres_Data.upSpheres[i].transform.localPosition =
                    new Vector3(0f, top, back + (interval * (i - 1))) - Control.transform.position;
            }
        }

        private bool IsSphereContainsOverlapChecker(OverlapChecker checker)
        {
            for (int i = 0; i < collisionSpheres_Data.frontOverlapCheckers.Length; i++)
            {
                if (collisionSpheres_Data.frontOverlapCheckers[i] == checker)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

