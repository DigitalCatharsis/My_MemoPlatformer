using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
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

        
    }
}
