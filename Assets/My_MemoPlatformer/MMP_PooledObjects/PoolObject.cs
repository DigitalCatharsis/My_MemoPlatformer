using System.Collections;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class PoolObject : MonoBehaviour
    {
        public PoolObjectType poolObjectType;
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

            PoolManager.Instance.AddObject(this);
        }

        IEnumerator _ScheduledOff()
        {
            yield return new WaitForSeconds (scheduledOffTime);
            if (!PoolManager.Instance.poolDictionary[poolObjectType].Contains(this.gameObject))
            {
                TurnOff(); 
            }
        }
    }
}