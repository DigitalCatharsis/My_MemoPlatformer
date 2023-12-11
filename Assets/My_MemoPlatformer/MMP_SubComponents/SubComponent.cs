using UnityEngine;

namespace My_MemoPlatformer
{
    public enum SubComponents
    {
        NONE,
        MANUALINPUT,
        LEDGECHECKER,
        RAGDOLL,
    }
    public enum BoolData
    {
        NONE,
        DOUBLETAP_DOWN,
        DOUBLETAP_UP,
        GRABBING_LEDGE,
    }
    public enum CharacterProc
    {
        NONE,
        LEDGE_COLLIDERS_OFF,
        RAGDOLL_ON,
    }

    public abstract class SubComponent : MonoBehaviour
    {
        public CharacterControl control;

        private void Awake()
        {
            control = this.gameObject.GetComponentInParent<CharacterControl>();
        }

        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
    }
}
