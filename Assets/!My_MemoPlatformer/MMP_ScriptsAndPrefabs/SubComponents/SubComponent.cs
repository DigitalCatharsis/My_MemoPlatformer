using UnityEngine;

namespace My_MemoPlatformer
{
    public enum SubComponentType
    {
        MANUAL_INPUT,
        LEDGE_CHECKER,
        RAGDOLL,
        BLOCKING_OBJECTS,
        BOX_COLLIDER_UPDATER,
        VERTICAL_VELOCITY,
        DAMAGE_DETECTOR,
        COLLISION_SPHERES,
        INSTA_KILL,
        PLAYER_ATTACK,
        PLAYER_ROTATION,
        PLAYER_ANIMATION,
        PLAYER_GROUND,
        CHARACTER_MOVEMENT,
        INTERACTION,
        AI_CONTROLLER,
    }

    public abstract class SubComponent : MonoBehaviour
    {
        protected SubComponentProcessor subComponentProcessor;

        public CharacterControl Control
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
