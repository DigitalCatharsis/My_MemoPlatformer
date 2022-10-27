using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class PoolObject : MonoBehaviour
    {
        public PoolObjectType poolObjectType;
        [SerializeField] float scheduledOffTime;
        private Coroutine offRoutine;

        private void OnOnEnable()  //failsafe
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