using System.Collections;
using System.Linq;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class BoxColliderUpdater : SubComponent
    {
        public BoxCollider_Data _boxCollider_Data;

        public override void OnComponentEnabled()
        {
            _boxCollider_Data = new BoxCollider_Data
            {

                latestUpdateBoxCollider = null,

                isUpdatingSpheres = false,
                isLanding = false,

                size_Update_Speed = 0f,
                center_Update_Speed = 0f,

                targetCenter = Vector3.zero,
                targetSize = Vector3.zero,
                landingPosition = Vector3.zero,

                boxColliderBounds = GetBoxColliderBounds(control.boxCollider),
            };

            subComponentProcessor.boxCollider_Data = _boxCollider_Data;
        }

        public override void OnFixedUpdate()
        {
            _boxCollider_Data.boxColliderBounds = GetBoxColliderBounds(control.boxCollider);

            //Spheres
            _boxCollider_Data.isUpdatingSpheres = false;

            if (DebugContainer_Data.Instance.debug_Colliders)
            {
                Debug.Log("Updating Size");
            }

            UpdateBoxCollider_Size();

            if (DebugContainer_Data.Instance.debug_Colliders)
            {
                Debug.Log("Updating Center");
            }

            UpdateBoxCollider_Center();

            if (_boxCollider_Data.isUpdatingSpheres)
            {
                if (DebugContainer_Data.Instance.debug_Colliders)
                {
                    Debug.Log("Repositioning Spheres (BoxColliderUpdater.cs)");
                }

                control.COLLISION_SPHERE_DATA.Reposition_FrontSpheres();
                control.COLLISION_SPHERE_DATA.Reposition_BottomSpheres();
                control.COLLISION_SPHERE_DATA.Reposition_BackSpheres();
                control.COLLISION_SPHERE_DATA.Reposition_UpSpheres();

                if (_boxCollider_Data.isLanding)  //prevent bug when idle after catching corner of platform
                {
                    //Debug.Log("repositioning y");
                    control.rigidBody.MovePosition(new Vector3(
                        0f,
                        _boxCollider_Data.landingPosition.y,
                        this.transform.position.z));
                }
            }
        }

        public override void OnUpdate()
        {
        }

        public void UpdateBoxCollider_Size()
        {
            if (!control.PLAYER_ANIMATION_DATA.IsRunning(typeof(UpdateBoxCollider)))
            {
                return;
            }

            if (Vector3.SqrMagnitude(control.boxCollider.size - _boxCollider_Data.targetSize) > 0.00001f || Vector3.SqrMagnitude(control.boxCollider.size - _boxCollider_Data.targetSize) < 0.00001f)
            {
                if (DebugContainer_Data.Instance.debug_Colliders)
                {
                    Debug.Log("Updating box collider size (Lerp)");
                }

                control.boxCollider.size = Vector3.Lerp(
                    control.boxCollider.size,
                    _boxCollider_Data.targetSize,
                    Time.deltaTime * _boxCollider_Data.size_Update_Speed);

                _boxCollider_Data.isUpdatingSpheres = true;
            }
        }

        public void UpdateBoxCollider_Center()
        {
            if (!control.PLAYER_ANIMATION_DATA.IsRunning(typeof(UpdateBoxCollider)))
            {
                return;
            }

            if (Vector3.SqrMagnitude(control.boxCollider.center - _boxCollider_Data.targetCenter) > 0.00001f || Vector3.SqrMagnitude(control.boxCollider.center - _boxCollider_Data.targetCenter) < 0.00001f)
            {
                if (DebugContainer_Data.Instance.debug_Colliders)
                {
                    Debug.Log("Updating box collider center (Lerp)");
                }

                control.boxCollider.center = Vector3.Lerp(control.boxCollider.center,
                    _boxCollider_Data.targetCenter,
                    Time.deltaTime * _boxCollider_Data.center_Update_Speed);

                _boxCollider_Data.isUpdatingSpheres = true;
            }
        }

        public Bounds GetBoxColliderBounds(BoxCollider boxCollider)
        {
            return new Bounds(boxCollider.center, boxCollider.size);
        }
    }
}
