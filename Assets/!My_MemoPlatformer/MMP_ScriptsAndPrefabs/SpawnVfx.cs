using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/SpawnObject")]
    public class SpawnVfx : CharacterAbility
    {
        public VFXType objectType;
        [Range(0f, 1f)]
        public float spawnTiming;
        public string parentObjectName = string.Empty;
        public bool stickToParent;

        [Range(0f, 1f)]
        [SerializeField] private float delayBeforeSpawn;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.INTERACTION_DATA.SpawnParticle(characterState.characterControl, objectType, parentObjectName, stickToParent, delayBeforeSpawn);
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }
}
