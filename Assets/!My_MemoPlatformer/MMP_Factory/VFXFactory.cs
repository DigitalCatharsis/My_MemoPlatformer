using UnityEngine;
using UnityEngine.Rendering;

namespace My_MemoPlatformer
{
    public enum VFXType
    {
        VFX_Shouryken,
        VFX_Damage_White,
    }

    public class VFXFactory : MonoBehaviour, ICoreFactory<VFXType>
    {
        [SerializeField] private GameObject GO_VFX_Shouryken;
        [SerializeField] private GameObject damageWhite_VFX;

        public VFXFactory()
        {
            GO_VFX_Shouryken = Resources.Load(VFXType.VFX_Shouryken.ToString()) as GameObject;
            damageWhite_VFX = Resources.Load(VFXType.VFX_Damage_White.ToString()) as GameObject;
        }

        public GameObject SpawnGameobject(VFXType VFX_Type, Vector3 position, Quaternion rotation)
        {
            switch (VFX_Type)
            {
                case VFXType.VFX_Shouryken:
                    {
                        return Instantiate(GO_VFX_Shouryken, position, rotation);
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