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
        private GameObject _whiteSelection;
        Animator _characterSelectCamAnimator;

        private void Awake()
        {
            characterSelect.selectedCharacterType = PlayableCharacterType.NONE;
            _characterSelectLight = GameObject.FindObjectOfType<CharacterSelectLight>();
            _characterHoverLight = GameObject.FindObjectOfType<CharacterHoverLight>();

            _whiteSelection = GameObject.Find("WhteSelection");
            _whiteSelection.SetActive(false);

            _characterSelectCamAnimator = GameObject.Find("CharacterSelectCameraControler").GetComponent<Animator>();
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



                    //Временно
                    _whiteSelection.SetActive(true);                    
                    _whiteSelection.transform.parent = control.skinnedMeshAnimator.transform;
                    _whiteSelection.transform.localPosition = new Vector3(-0.07f, 0.04f, 0.21f);
                }
                else
                {
                    characterSelect.selectedCharacterType = PlayableCharacterType.NONE;
                    _characterSelectLight.light.enabled = false;

                    _whiteSelection.SetActive(false);
                }

                foreach (CharacterControl c in CharacterManager.Instance.characters)
                {
                    if (c.playableCharacterType == selectedCharacterType)
                    {

                        c.skinnedMeshAnimator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.ClickAnimation], true);
                    }
                    else
                    {
                        c.skinnedMeshAnimator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.ClickAnimation], false);
                    }
                }

                _characterSelectCamAnimator.SetBool(selectedCharacterType.ToString(), true);
            }
        }
    }
}