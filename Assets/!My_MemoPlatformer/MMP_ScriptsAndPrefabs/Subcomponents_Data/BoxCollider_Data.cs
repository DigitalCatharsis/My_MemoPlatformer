using System;
using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class BoxCollider_Data
    {
        public UpdateBoxCollider latestUpdateBoxCollider;
        public bool isUpdatingSpheres;
        public bool isLanding;

        public float size_Update_Speed;
        public float center_Update_Speed;

        public Vector3 targetCenter;
        public Vector3 targetSize;
        public Vector3 landingPosition;

        public Bounds boxColliderBounds;
    }
}

