using System.Collections;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class VFXPoolObject : MonoBehaviour, IPoolObject
    {
        public VFXType poolObjectType;
        [SerializeField] float scheduledOffTime;
        private Coroutine offRoutine;

        private void OnEnable()                //���� ����, ����� ������ �� ����������, ������ ���� ��������� ����������������. ��� FailSafe
        {
            if (offRoutine != null)
            {
                StopCoroutine(offRoutine);
            }

            if (scheduledOffTime > 0f)
            {
                offRoutine = StartCoroutine(_ScheduledOff());
            }
        }

        public void TurnOff()
        {
            this.transform.parent = null;
            this.transform.position = Vector3.zero;
            this.transform.rotation = Quaternion.identity;
        }

        IEnumerator _ScheduledOff()
        {
            yield return new WaitForSeconds(scheduledOffTime);
            if (!PoolManager.Instance.vfxPoolDictionary[poolObjectType].Contains(this.gameObject))
            {
                TurnOff();
                ReturnToPool();
            }
        }

        public void ReturnToPool()
        {
            if (!PoolManager.Instance.vfxPoolDictionary[poolObjectType].Contains(this.gameObject))
            {
                PoolManager.Instance.AddObject(poolObjectType, PoolManager.Instance.vfxPoolDictionary, this.gameObject);
            }
        }
    }
}