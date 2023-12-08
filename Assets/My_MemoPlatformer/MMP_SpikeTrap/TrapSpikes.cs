using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    public class TrapSpikes : MonoBehaviour
    {
        public List<CharacterControl> listOfCharacter = new List<CharacterControl>();
        public List<CharacterControl> listOfSpikeVictims = new List<CharacterControl>();
        public List<Spike> listOfSpikes = new List<Spike>();
        public RuntimeAnimatorController spikeDeathAnimator;

        private Coroutine _spikeTriggerRoutine;
        private bool _spikesReloaded;

        private void Start()
        {
            _spikeTriggerRoutine = null;
            _spikesReloaded = true;
            listOfCharacter.Clear();
            listOfSpikes.Clear();
            listOfSpikeVictims.Clear();

            var arr = this.gameObject.GetComponentsInChildren<Spike>();

            foreach (var s in arr)
            {
                listOfSpikes.Add(s);
            }
        }

        private void Update()
        {
            if (listOfCharacter.Count != 0)
            {
                foreach (var control in listOfCharacter)
                {
                    if (!control.damageDetector.IsDead())
                    {
                        if (_spikeTriggerRoutine == null && _spikesReloaded)
                        {
                            if (!listOfSpikeVictims.Contains(control))
                            {
                                listOfSpikeVictims.Add(control);
                                control.damageDetector.DeathBySpikes();
                            }
                        }
                    }
                }
            }

            foreach (var control in listOfSpikeVictims)
            {
                if (control.skinnedMeshAnimator.avatar != null)
                {
                    if (_spikeTriggerRoutine == null && _spikesReloaded)
                    {
                        _spikeTriggerRoutine = StartCoroutine(_TriggerSpikes());
                    }
                }
            }
        }

        IEnumerator _TriggerSpikes()
        {
            _spikesReloaded = false;

            foreach (var s in listOfSpikes)
            {
                s.Shoot();
            }

            yield return new WaitForSeconds(0.08f);

            foreach (var control in listOfSpikeVictims)
            {
                control.damageDetector.TriggerSpikeDeath(spikeDeathAnimator);
            }

            yield return new WaitForSeconds(1.5f);

            foreach (var s in listOfSpikes)
            {
                s.Retract();
            }

            foreach (var control in listOfSpikeVictims)
            {
                control.procDict[CharacterProc.RAGDOLL_ON]();
            }

            yield return new WaitForSeconds(1);

            _spikeTriggerRoutine = null;
            _spikesReloaded = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var control = other.gameObject.transform.root.gameObject.GetComponent<CharacterControl>();

            if (control != null)
            {
                if (control.gameObject != other.gameObject)    //not a root box collider of character
                {
                    if (!listOfCharacter.Contains(control))
                    {
                        listOfCharacter.Add(control);
                    }
                }
            }

        }

        private void OnTriggerExit(Collider other)
        {
            var control = other.gameObject.transform.root.gameObject.GetComponent<CharacterControl>();

            if (control != null)
            {
                if (control.gameObject != other.gameObject)
                {
                    if (listOfCharacter.Contains(control))
                    {
                        listOfCharacter.Remove(control);
                    }
                }
            }
        }

        public static bool IsTrap(GameObject obj)
        {
            if (obj.transform.root.gameObject.GetComponent<TrapSpikes>() == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

