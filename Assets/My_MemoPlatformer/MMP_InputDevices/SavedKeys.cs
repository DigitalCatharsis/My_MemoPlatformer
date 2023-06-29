using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "Settings", menuName = "My_MemoPlatformer/Settings/SavedKeys")]
    public class SavedKeys : ScriptableObject
    {
        public List<KeyCode> keyCodesList = new List<KeyCode>();
    }
}