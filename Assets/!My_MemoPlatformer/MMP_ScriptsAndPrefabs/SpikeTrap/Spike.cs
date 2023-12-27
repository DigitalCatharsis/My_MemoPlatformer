using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class Spike : MonoBehaviour
    {
        public void Shoot()
        {
            if (this.transform.localPosition.y < 0f)
            {
                StartCoroutine(_Shoot());
            }
        }

        IEnumerator _Shoot()
        {
            var delay = Random.Range(0f, 0.25f);
            yield return new WaitForSeconds(delay);
            this.transform.localPosition += (Vector3.up * 1f);
        }

        public void Retract()
        {
            if (this.transform.localPosition.y > 0f)
            {
                StartCoroutine(_Retract());
            }
        }

        IEnumerator _Retract()
        {
            var delay = Random.Range(0f, 0.25f);
            yield return new WaitForSeconds(delay);
            this.transform.localPosition -= (Vector3.up * 1f);
        }
    }

}
