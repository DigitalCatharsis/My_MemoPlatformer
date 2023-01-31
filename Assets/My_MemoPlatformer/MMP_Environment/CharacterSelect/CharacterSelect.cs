using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public enum PlayableCharacterType
    {
        NONE,
        YELLOW,
        RED,
        GREEN,
    }

    [CreateAssetMenu(fileName = "chracterSelect", menuName = "My_MemoPlatformer/CharacterSelect/CharacterSelect")]
    public class CharacterSelect : ScriptableObject
    {
        public PlayableCharacterType selectedCharacterType;
    }
}