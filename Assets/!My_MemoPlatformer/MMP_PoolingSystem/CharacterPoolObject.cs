using UnityEngine;

namespace My_MemoPlatformer
{
    public class CharacterPoolObject : MonoBehaviour, IPoolObject
    {
        public CharacterType poolObjectType;

        public void TurnOff()
        {
            this.transform.parent = null;
            this.transform.position = Vector3.zero;
            this.transform.rotation = Quaternion.identity;

            ReturnToPool();
        }

        public void ReturnToPool()
        {
            if (!PoolManager.Instance.characterPoolDictionary[poolObjectType].Contains(this.gameObject))
            {
                PoolManager.Instance.AddObject(poolObjectType, PoolManager.Instance.characterPoolDictionary, this.gameObject);
            }
        }
    }
}