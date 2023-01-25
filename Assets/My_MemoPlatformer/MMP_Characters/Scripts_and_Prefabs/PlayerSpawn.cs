using Cinemachine;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] private CharacterSelect _characterSelect;

        private string _objName;

        private void Start()
        {
            switch (_characterSelect.selectedCharacterType)
            {
                case PlayableCharacterType.Yellow:
                    {
                        _objName = "YBot - Yellow";
                    }
                    break;
                case PlayableCharacterType.Red:
                    {
                        _objName = "YBot - Red Variant";
                    }
                    break;
                case PlayableCharacterType.Green:
                    {
                        _objName = "YBot - Green Variant";
                    }
                    break;
            }

            GameObject obj = Instantiate(Resources.Load(_objName,typeof(GameObject))) as GameObject;
            obj.transform.position = this.transform.position;
            GetComponent<MeshRenderer>().enabled = false;

            Cinemachine.CinemachineVirtualCamera[] arr = GameObject.FindObjectsOfType<Cinemachine.CinemachineVirtualCamera>();
            foreach (Cinemachine.CinemachineVirtualCamera v in arr)
            {
                CharacterControl control =  CharacterManager.Instance.GetCharacter(_characterSelect.selectedCharacterType);
                Collider target = control.GetBodyPart("Spine1");

                v.LookAt = target.transform;
                v.Follow = target.transform;

            }
        }
    }



}
