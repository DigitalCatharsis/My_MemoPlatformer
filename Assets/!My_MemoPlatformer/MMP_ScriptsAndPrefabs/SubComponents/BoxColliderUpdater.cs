using UnityEngine;

namespace My_MemoPlatformer
{
    public class BoxColliderUpdater : SubComponent
    {
        public BoxCollider_Data _boxCollider_Data;

        private void OnEnable()
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

                boxColliderBounds = GetBoxColliderBounds(Control.boxCollider),
            };

            subComponentProcessor.boxCollider_Data = _boxCollider_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.BOX_COLLIDER_UPDATER] = this;
        }
        public override void OnFixedUpdate()
        {
            _boxCollider_Data.boxColliderBounds = GetBoxColliderBounds(Control.boxCollider);

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

                Control.COLLISION_SPHERE_DATA.Reposition_FrontSpheres();
                Control.COLLISION_SPHERE_DATA.Reposition_BottomSpheres();
                Control.COLLISION_SPHERE_DATA.Reposition_BackSpheres();
                Control.COLLISION_SPHERE_DATA.Reposition_UpSpheres();

                if (_boxCollider_Data.isLanding)  //prevent bug when idle after catching corner of platform
                {
                    //Debug.Log("repositioning y");
                    Control.rigidBody.MovePosition(new Vector3(
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
            if (!Control.PLAYER_ANIMATION_DATA.IsRunning(typeof(UpdateBoxCollider)))
            {
                return;
            }

            if (Vector3.SqrMagnitude(Control.boxCollider.size - _boxCollider_Data.targetSize) > 0.00001f || Vector3.SqrMagnitude(Control.boxCollider.size - _boxCollider_Data.targetSize) < 0.00001f)
            {
                if (DebugContainer_Data.Instance.debug_Colliders)
                {
                    Debug.Log("Updating box collider size (Lerp)");
                }

                Control.boxCollider.size = Vector3.Lerp(
                    Control.boxCollider.size,
                    _boxCollider_Data.targetSize,
                    Time.deltaTime * _boxCollider_Data.size_Update_Speed);

                _boxCollider_Data.isUpdatingSpheres = true;
            }
        }

        public void UpdateBoxCollider_Center()
        {
            if (!Control.PLAYER_ANIMATION_DATA.IsRunning(typeof(UpdateBoxCollider)))
            {
                return;
            }

            if (Vector3.SqrMagnitude(Control.boxCollider.center - _boxCollider_Data.targetCenter) > 0.00001f || Vector3.SqrMagnitude(Control.boxCollider.center - _boxCollider_Data.targetCenter) < 0.00001f)
            {
                if (DebugContainer_Data.Instance.debug_Colliders)
                {
                    Debug.Log("Updating box collider center (Lerp)");
                }

                Control.boxCollider.center = Vector3.Lerp(Control.boxCollider.center,
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
