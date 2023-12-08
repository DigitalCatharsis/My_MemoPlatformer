using UnityEngine;

namespace My_MemoPlatformer
{
    public enum SubComponents
    {
        NONE,
        MANUALINPUT,
    }

    public enum BoolData
    {
        NONE,
        DOUBLETAP_DOWN,
        DOUBLETAP_UP,
    }

    public abstract class SubComponent : MonoBehaviour
    {
        public CharacterControl control;

        private void Awake()
        {
            control = this.gameObject.GetComponentInParent<CharacterControl>();
        }

        public abstract void OnUpdate();
    }
}
