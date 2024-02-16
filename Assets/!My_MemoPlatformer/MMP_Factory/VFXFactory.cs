using UnityEngine;
using UnityEngine.Rendering;

namespace My_MemoPlatformer
{
    public enum VFXType
    {
        VFX_HammerDown,
        VFX_Damage_White,
    }

    public class VFXFactory : MonoBehaviour, ICoreFactory<VFXType>
    {
        [SerializeField] private GameObject VFX_HammerDown;
        [SerializeField] private GameObject damageWhite_VFX;

        public VFXFactory()
        {
            VFX_HammerDown = Resources.Load(VFXType.VFX_HammerDown.ToString()) as GameObject;
            damageWhite_VFX = Resources.Load(VFXType.VFX_Damage_White.ToString()) as GameObject;
        }

        public GameObject SpawnGameobject(VFXType VFX_Type, Vector3 position, Quaternion rotation)
        {
            switch (VFX_Type)
            {
                case VFXType.VFX_HammerDown:
                    {
                        return Instantiate(VFX_HammerDown, position, rotation);
                    }
                case VFXType.VFX_Damage_White:
                    {
                        return Instantiate(damageWhite_VFX, position, rotation);
                    }
                default: 
                    {
                        Debug.Log("Cant instantiate " + VFX_Type);
                        return null;
                    }
            }
        }
    }
}