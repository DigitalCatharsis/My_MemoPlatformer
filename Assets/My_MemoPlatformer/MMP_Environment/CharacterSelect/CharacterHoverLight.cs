using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    public class CharacterHoverLight : MonoBehaviour
    {
        public Vector3 Offset = new Vector3();

        private CharacterControl _hoverSelectedCharacter;
        private MouseControl _mouseHoverSelect;
        private Vector3 _targetPos = new Vector3();
        private Light _light;

        private void Start()
        {
            _mouseHoverSelect = GameObject.FindObjectOfType<MouseControl>();
            _light = GetComponent<Light>();
        }
        private void Update()
        {
            if (_mouseHoverSelect.selectedCharacterType == PlayableCharacterType.NONE)
            {
                _hoverSelectedCharacter = null;
                _light.enabled= false;
            }
            else
            {
                _light.enabled = true;
                LightUpSelectedCharacter();
            }
        }

        private void LightUpSelectedCharacter()
        {
            if (_hoverSelectedCharacter == null)
            {
                _hoverSelectedCharacter = CharacterManager.Instance.GetCharacter(_mouseHoverSelect.selectedCharacterType);
                this.transform.position = _hoverSelectedCharacter.transform.position + _hoverSelectedCharacter.transform.TransformDirection(Offset);
            }
        }
    }
}
