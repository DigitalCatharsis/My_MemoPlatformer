using UnityEngine;

namespace My_MemoPlatformer
{
    public class EnemyPoolObject : MonoBehaviour, IPoolObject
    {
        public CharacterType poolObjectType;

        private void OnEnable()                //Есть бага, когда объект не апдейтится, объект пула перестает деактивироваться. Это FailSafe
        {
        }

        //TODO: implement turn off properly
        public void TurnOff()
        {
            this.transform.parent = null;
            this.transform.position = Vector3.zero;
            this.transform.rotation = Quaternion.identity;

            ReturnToPool();
        }

        public void ReturnToPool()
        {
            if (!PoolManager.Instance.CharacterPoolDictionary[poolObjectType].Contains(this.gameObject))
            {
                PoolManager.Instance.AddObject(poolObjectType, PoolManager.Instance.CharacterPoolDictionary, this.gameObject);
            }
        }
    }
}