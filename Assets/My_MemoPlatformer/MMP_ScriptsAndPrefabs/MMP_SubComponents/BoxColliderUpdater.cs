using UnityEngine;

namespace My_MemoPlatformer
{
    public class BoxColliderUpdater : SubComponent
    {
        BoxCollider_Data boxColliderData;

        private void Start()
        {
            boxColliderData = new BoxCollider_Data
            {
                updatingSpheres = false,
                isLanding = false,

                size_Update_Speed = 0f,
                center_Update_Speed = 0f,

                targetCenter = Vector3.zero,
                targetSize = Vector3.zero,
                landingPosition = Vector3.zero,
            };

            subComponentProcessor.boxCollider_Data = boxColliderData;
        }
        public override void OnFixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
