using UnityEngine;
using UnityEngine.Events;

namespace My_MemoPlatformer
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent gameEvent;
        [Space(10)]
        [SerializeField] UnityEvent response;

        private void Start()
        {
            if (gameEvent != null)
            {
                gameEvent.AddListener(this);
            }
        }

        public void OnRaiseEvent()
        {
            response.Invoke();
        }
    }
}
