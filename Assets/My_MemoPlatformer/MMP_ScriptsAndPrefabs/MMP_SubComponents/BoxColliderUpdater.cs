using UnityEngine;

namespace My_MemoPlatformer
{
    public class BoxColliderUpdater : SubComponent
    {
        [SerializeField]private bool _debug;

        BoxCollider_Data boxCollider_Data;

        private void Start()
        {
            boxCollider_Data = new BoxCollider_Data
            {
                updatingSpheres = false,
                isLanding = false,

                size_Update_Speed = 0f,
                center_Update_Speed = 0f,

                targetCenter = Vector3.zero,
                targetSize = Vector3.zero,
                landingPosition = Vector3.zero,
            };

            subComponentProcessor.boxCollider_Data = boxCollider_Data;
            subComponentProcessor.subcomponentsDictionary.Add(SubComponentType.BOX_COLLIDER_UPDATER, this);
        }
        public override void OnFixedUpdate()
        {
            //Spheres
            boxCollider_Data.updatingSpheres = false;

            UpdateBoxColliderSize();
            UpdateBoxColliderCenter();

            if (boxCollider_Data.updatingSpheres)
            {
                control.collisionSpheres.Reposition_FrontSpheres();
                control.collisionSpheres.Reposition_BottomSpheres();
                control.collisionSpheres.Reposition_BackSpheres();
                control.collisionSpheres.Reposition_UpSpheres();

                if (boxCollider_Data.isLanding)  //prevent bug when idle after catching corner of platform
                {
                    if (_debug)
                    {
                        Debug.Log("repositioning y");
                    }
                    control.Rigid_Body.MovePosition(new Vector3(0f, boxCollider_Data.landingPosition.y, this.transform.position.z));
                }
            }
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateBoxColliderSize()
        {
            if (!control.animationProgress.IsRunning(typeof(UpdateBoxCollider)))
            {
                return;
            }

            if (Vector3.SqrMagnitude(control.boxCollider.size - boxCollider_Data.targetSize) > 0.00001f)
            {
                control.boxCollider.size = Vector3.Lerp(control.boxCollider.size, boxCollider_Data.targetSize, Time.deltaTime * boxCollider_Data.size_Update_Speed);

                boxCollider_Data.updatingSpheres = true;
            }
        }

        public void UpdateBoxColliderCenter()
        {
            if (!control.animationProgress.IsRunning(typeof(UpdateBoxCollider)))
            {
                return;
            }

            if (Vector3.SqrMagnitude(control.boxCollider.center - boxCollider_Data.targetCenter) > 0.0001f)
            {
                control.boxCollider.center = Vector3.Lerp(control.boxCollider.center, boxCollider_Data.targetCenter, Time.deltaTime * boxCollider_Data.center_Update_Speed);

                boxCollider_Data.updatingSpheres = true;
            }
        }
    }
}
