using UnityEngine;

namespace My_MemoPlatformer
{
    public enum SubComponents
    {
        NONE,
        MANUALINPUT,
        LEDGECHECKER,
        RAGDOLL,
        BLOCKINGOBJECTS,
    }
    public enum BoolData
    {
        NONE,
        DOUBLETAP_DOWN,
        DOUBLETAP_UP,
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
