using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace My_MemoPlatformer
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent gameEvent;
        [Space(10)]
        [SerializeField] UnityEvent response;

        private void Start()
        {
            if (gameEvent != null)
            {
                if (!gameEvent.ListListeners.Contains(this))
                {
                    gameEvent.ListListeners.Add(this);
                }
            }
        }

        public void OnRaiseEvent()
        {
            response.Invoke();
        }
    }

}
