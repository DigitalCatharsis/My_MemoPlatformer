using System;
using System.Collections;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] private CharacterSelect _characterSelect;

        private Enum _objName;

        IEnumerator Start()
        {
            switch (_characterSelect.selectedCharacterType)
            {
                case PlayableCharacterType.YELLOW:
                    {
                        _objName = CharacterType.__Y_YBot_Yellow;
                    }
                    break;
                case PlayableCharacterType.RED:
                    {
                        _objName = CharacterType.__R_YBot_Red;
                    }
                    break;
                case PlayableCharacterType.GREEN:
                    {
                        _objName = CharacterType.__G_YBot_Green;
                    }
                    break;
                case PlayableCharacterType.NONE:
                    {
                        _characterSelect.selectedCharacterType = PlayableCharacterType.YELLOW;
                        _objName = CharacterType.__Y_YBot_Yellow;
                    }
                    break;
            }

            var obj = PoolManager.Instance.GetObject((CharacterType)_objName, PoolManager.Instance.characterPoolDictionary, this.transform.position, Quaternion.identity);

            //var obj = Instantiate(Resources.Load(_objName,typeof(GameObject))) as GameObject;
            //obj.transform.position = this.transform.position;
            GetComponent<MeshRenderer>().enabled = false;

            yield return new WaitForEndOfFrame(); //get some time for cinemachine to catch target, cause of control.RagdollData.GetBodypart is not initialized yet

            Cinemachine.CinemachineVirtualCamera[] arr = GameObject.FindObjectsOfType<Cinemachine.CinemachineVirtualCamera>();
            foreach (Cinemachine.CinemachineVirtualCamera v in arr)
            {
                yield return new WaitForEndOfFrame();
                var control =  CharacterManager.Instance.GetCharacter(_characterSelect.selectedCharacterType);
                Collider target = control.RAGDOLL_DATA.GetBodypart("mixamorig:Spine1");

                v.LookAt = target.transform;
                v.Follow = target.transform;
            }
        }
    }
}
