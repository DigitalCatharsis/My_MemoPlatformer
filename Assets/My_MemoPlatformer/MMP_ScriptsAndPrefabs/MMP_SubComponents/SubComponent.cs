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
        GRABBING_LEDGE,
        UPBLOCKINGOBJ_DICTIONARY_IS_EMPTY,
        FRONTBLOCKINGOBJ_DICTIONARY_IS_EMPTY,
        RIGHTSIDE_IS_BLOCKED,
        LEFTSIDE_IS_BLOCKED,
    }
    public enum ListData
    {
        FRONTBLOCKING_CHARACTERS,
        FRONTBLOCKING_OBJECTS,
    }

    public enum CharacterProc
    {
        NONE,
        LEDGE_COLLIDERS_OFF,
        RAGDOLL_ON,
        CLEAR_FRONTBLOCKING_OBJ_DICTIONARY,
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
