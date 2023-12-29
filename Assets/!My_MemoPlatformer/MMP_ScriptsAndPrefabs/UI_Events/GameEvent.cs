using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class GameEvent : MonoBehaviour
    {
        public List<GameEventListener> ListListeners = new List<GameEventListener>();
        private GameObject _eventObj;
        public GameObject EVENTOBJ
        {
            get { return _eventObj; }
        }

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
            _eventObj = eventObj;

            foreach (GameEventListener listener in ListListeners)
            {
                listener.OnRaiseEvent();
            }
        }
        public void AddListener(GameEventListener listener)
        {
            if (!ListListeners.Contains(listener))
            {
                ListListeners.Add(listener);
            }
        }
    }
}