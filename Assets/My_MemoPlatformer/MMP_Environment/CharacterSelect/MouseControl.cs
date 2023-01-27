using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace My_MemoPlatformer
{
    public class MouseControl : MonoBehaviour
    {
        Ray ray;
        RaycastHit hit;
        public PlayableCharacterType selectedCharacterType;
        public CharacterSelect characterSelect;
        private CharacterSelectLight _characterSelectLight;
        private CharacterHoverLight _characterHoverLight;

        private void Awake()
        {
            characterSelect.selectedCharacterType = PlayableCharacterType.NONE;
            _characterSelectLight = GameObject.FindObjectOfType<CharacterSelectLight>();
            _characterHoverLight = GameObject.FindObjectOfType<CharacterHoverLight>();
        }

        private void Update()
        {
            ray = CameraManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                CharacterControl control = hit.collider.gameObject.GetComponent<CharacterControl>();
                if (control != null)
                {
                    selectedCharacterType = control.playableCharacterType;
                }
                else
                {
                    selectedCharacterType = PlayableCharacterType.NONE;
                }
            }

            if (Input.GetMouseButtonDown(0)) //button pressed
            {
                if (selectedCharacterType != PlayableCharacterType.NONE)
                {
                    characterSelect.selectedCharacterType = this.selectedCharacterType;
                    _characterSelectLight.transform.position = _characterHoverLight.transform.position;
                    CharacterControl control = CharacterManager.Instance.GetCharacter(selectedCharacterType);
                    _characterSelectLight.transform.parent = control.skinnedMeshAnimator.transform;
                    _characterSelectLight.light.enabled = true;
                }
                else
                {
                    characterSelect.selectedCharacterType = PlayableCharacterType.NONE;
                    _characterSelectLight.light.enabled = false;
                }

                foreach (CharacterControl c in CharacterManager.Instance.characters)
                {
                    if (c.playableCharacterType == selectedCharacterType)
                    {

                        c.skinnedMeshAnimator.SetBool(TransitionParameter.ClickAnimation.ToString(), true);
                    }
                    else
                    {
                        c.skinnedMeshAnimator.SetBool(TransitionParameter.ClickAnimation.ToString(), false);
                    }
                }
            }
        }
    }
}