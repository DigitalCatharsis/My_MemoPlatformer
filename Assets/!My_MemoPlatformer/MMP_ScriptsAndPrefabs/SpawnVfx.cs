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

        private GameObject _spawnedVFX;

        //TODO: передлать условия спауна, т.к тут оно делает отдельно
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (spawnTiming == 0f)
            {
                SpawnVFX(characterState.characterControl);
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!characterState.characterControl.OBJ_POOLING_DATA.Vfxs.Contains(_spawnedVFX))
            {
                if (stateInfo.normalizedTime >= spawnTiming)
                {
                    SpawnVFX(characterState.characterControl);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (characterState.characterControl.OBJ_POOLING_DATA.Vfxs.Contains(_spawnedVFX))
            {
                characterState.characterControl.OBJ_POOLING_DATA.Vfxs.Remove(_spawnedVFX);
            }
        }

        private void SpawnVFX(CharacterControl control)
        {
            if (control.OBJ_POOLING_DATA.Vfxs.Contains(this._spawnedVFX))
            {
                return;
            }

            _spawnedVFX = PoolManager.Instance.GetObject(objectType, PoolManager.Instance.vfxPoolDictionary, Vector3.zero, Quaternion.identity);

            if (DebugContainer_Data.Instance.debug_SpawnObjects)
            {
                Debug.Log("spawning " + objectType.ToString() + " | looking for: " + parentObjectName);
            }

            if (!string.IsNullOrEmpty(parentObjectName))
            {
                var p = control.GetChildObj(parentObjectName);
                _spawnedVFX.transform.parent = p.transform;
                _spawnedVFX.transform.localPosition = Vector3.zero;
                _spawnedVFX.transform.localRotation = Quaternion.identity;
            }

            if (!stickToParent)
            {
                _spawnedVFX.transform.parent = null;
            }

            _spawnedVFX.SetActive(true);

            control.OBJ_POOLING_DATA.Vfxs.Add(_spawnedVFX);
        }
    }
}
