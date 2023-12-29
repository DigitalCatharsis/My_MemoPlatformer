using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class CharacterManager : Singleton<CharacterManager>
    {
        public List<CharacterControl> characters = new List<CharacterControl>();

        public CharacterControl GetCharacter(PlayableCharacterType playableCharacterType)
        {
            foreach (var control in characters) 
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
            foreach (var control in characters)
            {
                if (control.skinnedMeshAnimator == animator)
                {
                    return control;
                }
            }
            return null;
        }
           
        public CharacterControl GetCharacter(GameObject obj)
        {
            foreach (var control in characters)
            {
                if (control.gameObject == obj)
                {
                    return control;
                }
            }
            return null;
        }

        public CharacterControl GetPlayableCharacter()
        {
            foreach (var control in characters)
            {
                if (control.subComponentProcessor.arrSubComponents[(int)SubComponentType.MANUAL_INPUT] != null)
                {
                    return control;
                }
            }

            return null;
        }
    }
}
