using UnityEngine;

namespace My_MemoPlatformer
{
    public enum SubComponentType
    {
        NONE,
        MANUALINPUT,
        LEDGECHECKER,
        RAGDOLL,
        BLOCKINGOBJECTS,
        BOX_COLLIDER_UPDATER,
        VERTICALVELOCITY_DATA,
        DAMAGE_DETECTOR_DATA,
        COLLISION_SPHERES,
        INSTAKILL,
    }

    public abstract class SubComponent : MonoBehaviour
    {
        protected SubComponentProcessor subComponentProcessor;

        public CharacterControl control
        {
            get
            {
                return subComponentProcessor.control;
            }
        }

        private void Awake()
        {
            subComponentProcessor = this.gameObject.GetComponentInParent<SubComponentProcessor>();
        }

        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
    }
}
