using UnityEngine;
using UnityEngine.Rendering;

namespace My_MemoPlatformer
{
    public enum DataType
    {
        AttackCondition,
    }

    public class DataFactory : MonoBehaviour, ICoreFactory<DataType>
    {
        [SerializeField] private GameObject attackCondition;

        public DataFactory()
        {
            attackCondition = Resources.Load(DataType.AttackCondition.ToString()) as GameObject;
        }

        public GameObject SpawnGameobject(DataType VFX_Type, Vector3 position, Quaternion rotation)
        {
            switch (VFX_Type)
            {
                case DataType.AttackCondition:
                    {
                        return Instantiate(attackCondition, position, rotation);
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