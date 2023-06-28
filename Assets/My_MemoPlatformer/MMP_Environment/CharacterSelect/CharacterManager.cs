using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    public class CharacterManager : Singleton<CharacterManager>
    {
        public List<CharacterControl> characters = new List<CharacterControl>();

        public CharacterControl GetCharacter(PlayableCharacterType playableCharacterType)
        {
            foreach (CharacterControl control in characters) 
            {
                if (control.playableCharacterType == playableCharacterType)
                {
                    return control;
                }
            }
            return null;
        }

        public CharacterControl GetCharacter(Animator animator)
        {
            foreach (CharacterControl control in characters)
            {
                if (control.skinnedMeshAnimator == animator)
                {
                    return control;
                }
            }
            return null;
        }

        public CharacterControl GetPlayableCharacter()
        {
            foreach (CharacterControl control in characters)
            {
                ManualInput manualInput = control.GetComponent<ManualInput>();

                if (manualInput != null)
                {
                    if (manualInput.enabled == true)
                    {
                        return control;
                    }
                    
                }
            }

            return null;

        }

    }
}
