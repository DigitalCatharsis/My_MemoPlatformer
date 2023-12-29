using UnityEngine;


namespace My_MemoPlatformer
{
    public class CharacterHoverLight : MonoBehaviour
    {
        public Vector3 Offset = new Vector3();

        private CharacterControl _hoverSelectedCharacter;
        private MouseControl _mouseHoverSelect;
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
                this.transform.position = _hoverSelectedCharacter.skinnedMeshAnimator.transform.position + _hoverSelectedCharacter.transform.TransformDirection(Offset);
                this.transform.parent = _hoverSelectedCharacter.skinnedMeshAnimator.transform; //adding to the hierarchy of the character to make this move with char.
            }
        }
    }
}
