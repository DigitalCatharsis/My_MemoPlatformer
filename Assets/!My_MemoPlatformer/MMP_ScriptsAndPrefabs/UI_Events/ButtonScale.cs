using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace My_MemoPlatformer
{
    public enum UIParameters
    {
        ScaleUp, 
    }

    public class ButtonScale : MonoBehaviour
    {
        private GameEventListener listener;

        private Dictionary<GameObject, Animator> _dicButtons = new Dictionary<GameObject, Animator>();

        private void Awake()
        {
            listener = gameObject.GetComponent<GameEventListener>();
        }
        public void ScaleUpButton()
        {
            GetButtonAnimator(listener.gameEvent.EVENTOBJ).SetBool(UIParameters.ScaleUp.ToString(), true);
        }

        public void ResetButtonScale()
        {
            GetButtonAnimator(listener.gameEvent.EVENTOBJ).SetBool(UIParameters.ScaleUp.ToString(), false);
        }

        private Animator GetButtonAnimator(GameObject obj)
        {
            if (!_dicButtons.ContainsKey(obj))
            {
                Animator animator = obj.GetComponent<Animator>();
                _dicButtons.Add(obj, animator);
                return animator;
            }
            else
            {
                return _dicButtons[obj];
            }
        }
    }
}

