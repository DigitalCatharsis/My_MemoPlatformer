using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class InstaKill : MonoBehaviour
    {
        private CharacterControl _control;
        private void Start()
        {
            _control = this.gameObject.GetComponentInParent<CharacterControl>();
        }

        private void FixedUpdate()
        {
            if (_control.subComponentProcessor.componentsDictionary.ContainsKey(SubComponents.MANUALINPUT))
            {
                return;
            }

            if (!_control.skinnedMeshAnimator.GetBool(HashManager.Instance.dicMainParams[TransitionParameter.Grounded]))
            {
                return;
            }

            //if one of bodypart is player, there gonna be instakill
            foreach (KeyValuePair<TriggerDetector, List<Collider>> data in _control.animationProgress.collidingBodyParts) 
            {
                foreach (var col in data.Value)
                {
                    var c = CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);  

                    if (c == _control)
                    {
                        continue;
                    }

                    if (!c.subComponentProcessor.componentsDictionary.ContainsKey(SubComponents.MANUALINPUT)) //has to be a player, if not - next
                    {
                        continue;
                    }

                    if (c.animationProgress.IsRunning(typeof(Attack)))
                    {
                        continue;
                    }

                    if (_control.animationProgress.IsRunning(typeof(Attack)))
                    {
                        continue;
                    }

                    if (c.animationProgress.StateNameContains("RunningSlide"))
                    {
                        continue;
                    }

                    if (c.damageDetector.IsDead())
                    {
                        continue;
                    }

                    if (_control.damageDetector.IsDead())
                    {
                        continue;
                    }

                    Debug.Log("InstaKill");
                    c.damageDetector.DeathByInstakill(_control);

                    return;
                }
            }
        }
    }
}