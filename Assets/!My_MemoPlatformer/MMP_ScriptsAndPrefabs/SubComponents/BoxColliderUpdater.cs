using UnityEngine;

namespace My_MemoPlatformer
{
    public class BoxColliderUpdater : SubComponent
    {
        public BoxCollider_Data _boxCollider_Data;

        private void Start()
        {
            _boxCollider_Data = new BoxCollider_Data
            {
                isUpdatingSpheres = false,
                isLanding = false,

                size_Update_Speed = 0f,
                center_Update_Speed = 0f,

                targetCenter = Vector3.zero,
                targetSize = Vector3.zero,
                landingPosition = Vector3.zero,
            };

            subComponentProcessor.boxCollider_Data = _boxCollider_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.BOX_COLLIDER_UPDATER] = this;
        }
        public override void OnFixedUpdate()
        {
            //Spheres
            _boxCollider_Data.isUpdatingSpheres = false;

            UpdateBoxCollider_Size();
            UpdateBoxCollider_Center();

            if (_boxCollider_Data.isUpdatingSpheres)
            {
                control.COLLISION_SPHERE_DATA.Reposition_FrontSpheres();
                control.COLLISION_SPHERE_DATA.Reposition_BottomSpheres();
                control.COLLISION_SPHERE_DATA.Reposition_BackSpheres();
                control.COLLISION_SPHERE_DATA.Reposition_UpSpheres();

                if (_boxCollider_Data.isLanding)  //prevent bug when idle after catching corner of platform
                {
                    //Debug.Log("repositioning y");
                    control.RIGID_BODY.MovePosition(new Vector3(
                        0f,
                        _boxCollider_Data.landingPosition.y,
                        this.transform.position.z));
                }
            }
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateBoxCollider_Size()
        {
            if (!control.ANIMATION_DATA.IsRunning(typeof(UpdateBoxCollider)))
            {
                return;
            }

            if (Vector3.SqrMagnitude(control.boxCollider.size - _boxCollider_Data.targetSize) > 0.00001f)
            {
                control.boxCollider.size = Vector3.Lerp(control.boxCollider.size,
                    _boxCollider_Data.targetSize, 
                    Time.deltaTime * _boxCollider_Data.size_Update_Speed);

                _boxCollider_Data.isUpdatingSpheres = true;
            }
        }

        public void UpdateBoxCollider_Center()
        {
            if (!control.ANIMATION_DATA.IsRunning(typeof(UpdateBoxCollider)))
            {
                return;
            }

            if (Vector3.SqrMagnitude(control.boxCollider.center - _boxCollider_Data.targetCenter) > 0.0001f)
            {
                control.boxCollider.center = Vector3.Lerp(control.boxCollider.center,
                    _boxCollider_Data.targetCenter,
                    Time.deltaTime * _boxCollider_Data.center_Update_Speed);

                _boxCollider_Data.isUpdatingSpheres = true;
            }
        }
    }
}
