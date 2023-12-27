using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class GameEvent : MonoBehaviour
    {
        public List<GameEventListener> ListListeners = new List<GameEventListener>();
        public GameObject eventObj;

        private void Awake()
        {
            ListListeners.Clear();
        }

        public void Raise()
        {
            foreach (GameEventListener listener in ListListeners)
            {
                listener.OnRaiseEvent();
            }
        }
        public void Raise(GameObject eventObj)
        {
            foreach (GameEventListener listener in ListListeners)
            {
                this.eventObj = eventObj;
                listener.OnRaiseEvent();
            }
        }
    }
}