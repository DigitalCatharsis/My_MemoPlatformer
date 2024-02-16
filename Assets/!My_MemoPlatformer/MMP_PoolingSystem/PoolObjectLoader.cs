using System;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class PoolObjectLoader : Singleton<PoolObjectLoader>
    {
        public GameObject InstantiatePrefab<T>(T objType, Vector3 position, Quaternion rotation)
        {
            var typelist = objType switch
            {
                CharacterType characterType => InstantiateCharacter(characterType, position, rotation),
                VFXType powerUpType => InstantiateVFX(powerUpType, position, rotation),
                DataType dataType => InstantiateProjectile(dataType, position, rotation),
                var unknownType => throw new Exception($"{unknownType?.GetType()}")
            };
            return typelist;
        }

        public GameObject InstantiateCharacter(CharacterType poolObjectType, Vector3 position, Quaternion rotation)
        {
            return Spawner.Instance.CharacterFactory.SpawnGameobject(poolObjectType, position, rotation);
        }

        public GameObject InstantiateVFX(VFXType poolObjectType, Vector3 position, Quaternion rotation)
        {
            return Spawner.Instance.vFXFactory.SpawnGameobject(poolObjectType, position, rotation);
        }
        public GameObject InstantiateProjectile(DataType poolObjectType, Vector3 position, Quaternion rotation)
        {
            return Spawner.Instance.dataFactory.SpawnGameobject(poolObjectType, position, rotation);
        }
    }
}