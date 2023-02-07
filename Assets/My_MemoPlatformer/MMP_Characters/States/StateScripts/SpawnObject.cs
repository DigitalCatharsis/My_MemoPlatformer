using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/SpawnObject")]
    public class SpawnObject : StateData
    {
        [SerializeField] private PoolObjectType objectType;
        [Range(0f, 1f)]
        public float spawnTiming;
        public string parentObjectName = string.Empty;
        public bool stickToParent;




        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (spawnTiming== 0f)
            {
                CharacterControl control = characterState.GetCharacterControl(animator);
                SpawnObj(control);
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            if (!control.animationProgress.poolObjectList.Contains(objectType))
            {
                if (stateInfo.normalizedTime >= spawnTiming)
                {                    
                    SpawnObj(control);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);
            if (control.animationProgress.poolObjectList.Contains(objectType))
            {
                control.animationProgress.poolObjectList.Remove(objectType);
            }
        }

        private void SpawnObj(CharacterControl control)
        {
            if (control.animationProgress.poolObjectList.Contains(objectType))
            {
                return;
            }

            GameObject obj = PoolManager.Instance.GetObject(objectType);

            if (!string.IsNullOrEmpty(parentObjectName))
            {
                GameObject p = control.GetChildObj(parentObjectName);
                obj.transform.parent = p.transform;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;

            }

            if (!stickToParent)
            {
                obj.transform.parent = null;
            }

            obj.SetActive(true);

            control.animationProgress.poolObjectList.Add(objectType);

        }

    }

}
