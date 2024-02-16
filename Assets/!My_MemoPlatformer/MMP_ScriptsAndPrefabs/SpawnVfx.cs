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

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (spawnTiming == 0f)
            {
                SpawnObj(characterState.characterControl);
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!characterState.characterControl.OBJ_POOLING_DATA.Vfxs.Contains(objectType))
            {
                if (stateInfo.normalizedTime >= spawnTiming)
                {
                    SpawnObj(characterState.characterControl);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (characterState.characterControl.OBJ_POOLING_DATA.Vfxs.Contains(objectType))
            {
                characterState.characterControl.OBJ_POOLING_DATA.Vfxs.Remove(objectType);
            }
        }

        private void SpawnObj(CharacterControl control)
        {
            if (control.OBJ_POOLING_DATA.Vfxs.Contains(objectType))
            {
                return;
            }

            var obj = PoolManager.Instance.GetObject(objectType, PoolManager.Instance.vfxPoolDictionary, Vector3.zero, Quaternion.identity);

            if (DebugContainer_Data.Instance.debug_SpawnObjects)
            {
                Debug.Log("spawning " + objectType.ToString() + " | looking for: " + parentObjectName);
            }

            if (!string.IsNullOrEmpty(parentObjectName))
            {
                var p = control.GetChildObj(parentObjectName);
                obj.transform.parent = p.transform;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
            }

            if (!stickToParent)
            {
                obj.transform.parent = null;
            }

            obj.SetActive(true);

            control.OBJ_POOLING_DATA.Vfxs.Add(objectType);
        }
    }
}
