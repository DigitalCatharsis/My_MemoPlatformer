using UnityEngine;
using UnityEngine.Rendering;

namespace My_MemoPlatformer
{
    public class CollisionSpheres : SubComponent
    {
        [Header("Spheres")]
        [Space(10)]
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
                IsFrontSphereContainsOverlapChecker = IsFrontSphereContainsOverlapChecker,

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

        private void OnDrawGizmos()
        {
            BoxCollider boxCollider = gameObject.transform.root.GetComponent<BoxCollider>();

            Bounds localBounds = new Bounds(boxCollider.center, boxCollider.size);

            const float radius = 0.1f;

            Gizmos.color = Color.cyan;

            Gizmos.DrawSphere(transform.TransformPoint(new Vector3(localBounds.min.x, localBounds.min.y, localBounds.min.z)), radius);
            Gizmos.DrawSphere(transform.TransformPoint(new Vector3(localBounds.min.x, localBounds.min.y, localBounds.max.z)), radius);
            Gizmos.DrawSphere(transform.TransformPoint(new Vector3(localBounds.max.x, localBounds.min.y, localBounds.max.z)), radius);
            Gizmos.DrawSphere(transform.TransformPoint(new Vector3(localBounds.max.x, localBounds.min.y, localBounds.min.z)), radius);

            Gizmos.DrawSphere(transform.TransformPoint(new Vector3(localBounds.min.x, localBounds.max.y, localBounds.min.z)), radius);
            Gizmos.DrawSphere(transform.TransformPoint(new Vector3(localBounds.min.x, localBounds.max.y, localBounds.max.z)), radius);
            Gizmos.DrawSphere(transform.TransformPoint(new Vector3(localBounds.max.x, localBounds.max.y, localBounds.max.z)), radius);
            Gizmos.DrawSphere(transform.TransformPoint(new Vector3(localBounds.max.x, localBounds.max.y, localBounds.min.z)), radius);
        }

        private void SetColliderSpheres()
        {
            if (DebugContainer_Data.Instance.debug_Colliders)
            {
                Debug.Log("Setting Collider Spheres");
            }

            //bottom
            for (int i = 0; i < 5; i++)
            {
                var sphere = LoadCollisionSpheres();
                collisionSpheres_Data.bottomSpheres[i] = sphere;
                sphere.transform.parent = this.transform.Find("Bottom");
            }

            Reposition_BottomSpheres();

            //top
            for (int i = 0; i < 5; i++)
            {
                var sphere = LoadCollisionSpheres();
                collisionSpheres_Data.upSpheres[i] = sphere;
                sphere.transform.parent = this.transform.Find("Up");
            }

            Reposition_UpSpheres();

            //front
            for (int i = 0; i < 10; i++)
            {
                var sphere = LoadCollisionSpheres();
                collisionSpheres_Data.frontSpheres[i] = sphere;
                collisionSpheres_Data.frontOverlapCheckers[i] = sphere.GetComponent<OverlapChecker>();
                sphere.transform.parent = this.transform.Find("Front");
            }

            Reposition_FrontSpheres();

            //back
            for (int i = 0; i < 10; i++)
            {
                var sphere = LoadCollisionSpheres();
                collisionSpheres_Data.backSpheres[i] = sphere;
                sphere.transform.parent = this.transform.Find("Back");
            }

            Reposition_BackSpheres();

            //add every overlapChecker
            var overlapCheckers_Array = this.gameObject.GetComponentsInChildren<OverlapChecker>();
            collisionSpheres_Data.allOverlapCheckers = overlapCheckers_Array;
        }

        //private void RepositionSpheresWithInterval(int spheresNumber, float startAxisPosition, float endAxisPosition, GameObject[] spheresCollection)
        //{
        //    float interval = (startAxisPosition - endAxisPosition + 0.05f) / spheresNumber;

        //    for (int i = 2; i < collisionSpheres_Data.frontSpheres.Length; i++)
        //    {
        //        collisionSpheres_Data.frontSpheres[i].transform.localPosition =
        //            new Vector3(0f, startAxisPosition + (interval * (i - 1)), endAxisPosition);
        //    }
        //}

        private void Reposition_FrontSpheres()
        {
            var bounds = Control.boxColliderBounds;

            var upPosition = new Vector3(0, bounds.max.y, bounds.max.z);
            var bottomPosition = new Vector3(0, bounds.min.y, bounds.max.z);

            collisionSpheres_Data.frontSpheres[0].transform.localPosition = upPosition;
            collisionSpheres_Data.frontSpheres[1].transform.localPosition = bottomPosition;

            float interval = (upPosition.y - bottomPosition.y + 0.05f) / 9;

            for (int i = 2; i < collisionSpheres_Data.frontSpheres.Length; i++)
            {
                collisionSpheres_Data.frontSpheres[i].transform.localPosition =
                    new Vector3(0f, upPosition.y - (interval * (i - 1)), upPosition.z);
            }
        }
        private void Reposition_BackSpheres()
        {
            var bounds = Control.boxColliderBounds;

            var upPosition = new Vector3(0, bounds.max.y, bounds.min.z);
            var bottomPosition = new Vector3(0, bounds.min.y, bounds.min.z);

            collisionSpheres_Data.backSpheres[0].transform.localPosition = upPosition;
            collisionSpheres_Data.backSpheres[1].transform.localPosition = bottomPosition;

            float interval = (upPosition.y - bottomPosition.y + 0.05f) / 9;

            for (int i = 2; i < collisionSpheres_Data.backSpheres.Length; i++)
            {
                collisionSpheres_Data.backSpheres[i].transform.localPosition =
                    new Vector3(0f, upPosition.y - (interval * (i - 1)), upPosition.z);
            }
        }

        private void Reposition_BottomSpheres()
        {
            {
                var bounds = Control.boxColliderBounds;

                var frontPosition = new Vector3(0, bounds.min.y, bounds.max.z);
                var backPosition = new Vector3(0, bounds.min.y, bounds.min.z);

                collisionSpheres_Data.bottomSpheres[0].transform.localPosition = frontPosition;
                collisionSpheres_Data.bottomSpheres[1].transform.localPosition = backPosition;

                float interval = (frontPosition.z - backPosition.z) / 4;

                for (int i = 2; i < collisionSpheres_Data.bottomSpheres.Length; i++)
                {
                    collisionSpheres_Data.bottomSpheres[i].transform.localPosition =
                        new Vector3(0f, frontPosition.y, frontPosition.z - (interval * (i - 1)));
                }
            }
        }

        private void Reposition_UpSpheres()
        {
            var bounds = Control.boxColliderBounds;

            var frontPosition = new Vector3(0, bounds.max.y, bounds.max.z);
            var backPosition = new Vector3(0, bounds.max.y, bounds.min.z);

            collisionSpheres_Data.upSpheres[0].transform.localPosition = frontPosition;
            collisionSpheres_Data.upSpheres[1].transform.localPosition = backPosition;

            float interval = (frontPosition.z - backPosition.z + 0.05f) / 4;

            for (int i = 2; i < collisionSpheres_Data.upSpheres.Length; i++)
            {
                collisionSpheres_Data.upSpheres[i].transform.localPosition =
                    new Vector3(0f, frontPosition.y, frontPosition.z - (interval * (i - 1)));
            }
            //if (DebugContainer_Data.Instance.debug_Colliders)
            //{
            //    Debug.Log("Repositioning Up Spheres");
            //}s

            //var top = Control.boxCollider.bounds.center.y + (Control.boxCollider.bounds.size.y / 2f);
            //var front = Control.boxCollider.bounds.center.z + (Control.boxCollider.bounds.size.z / 2f);

            //var front_UpSphere = new Vector3(0f, top, front) - Control.transform.position;
            //collisionSpheres_Data.upSpheres[1].transform.localPosition = front_UpSphere;

            //var back_UpSphere = new Vector3(0, front_UpSphere.y, -front_UpSphere.z);
            //collisionSpheres_Data.upSpheres[0].transform.localPosition = back_UpSphere;

            //float interval = (front_UpSphere.z - back_UpSphere.z) / 4;

            //for (int i = 2; i < collisionSpheres_Data.upSpheres.Length; i++)
            //{
            //    collisionSpheres_Data.upSpheres[i].transform.localPosition =
            //        new Vector3(0f, front_UpSphere.y, back_UpSphere.z + (interval * (i - 1)));
            //}
        }

        private bool IsFrontSphereContainsOverlapChecker(OverlapChecker checker)
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

