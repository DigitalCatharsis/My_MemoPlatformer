using UnityEngine;

namespace My_MemoPlatformer
{
    public class CharacterSelectLight : MonoBehaviour
    {
        public new Light light;
        private void Start()
        {
            light = GetComponent<Light>();
            light.enabled = false;
        }


    }
}