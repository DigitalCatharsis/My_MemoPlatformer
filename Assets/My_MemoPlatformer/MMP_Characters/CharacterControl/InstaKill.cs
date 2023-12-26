using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class InstaKill : SubComponent
    {
        public InstaKill_Data instaKill_Data;
        [SerializeField] private RuntimeAnimatorController Assassination_Assassin;
        [SerializeField] private RuntimeAnimatorController Assassination_Victim;
        private void Start()
        {
            instaKill_Data = new InstaKill_Data
            {
                Animation_Assassin = Assassination_Assassin,
                Animation_Victim = Assassination_Victim,
            };

            subComponentProcessor.instaKill_Data = instaKill_Data;
            subComponentProcessor.subcomponentsDictionary.Add(SubComponentType.INSTAKILL, this);
        }
        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnFixedUpdate()
        {
            if (control.subComponentProcessor.subcomponentsDictionary.ContainsKey(SubComponentType.MANUALINPUT))
            {
                return;
            }

            if (!control.skinnedMeshAnimator.GetBool(HashManager.Instance.dicMainParams[TransitionParameter.Grounded]))
            {
                return;
            }

            //if one of bodypart is player, there gonna be instakill
            foreach (KeyValuePair<TriggerDetector, List<Collider>> data in control.animationProgress.collidingBodyParts)
            {
                foreach (var col in data.Value)
                {
                    var c = CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);

                    if (c == control)
                    {
                        continue;
                    }

                    if (!c.subComponentProcessor.subcomponentsDictionary.ContainsKey(SubComponentType.MANUALINPUT)) //has to be a player, if not - next
                    {
                        continue;
                    }

                    if (c.animationProgress.IsRunning(typeof(Attack)))
                    {
                        continue;
                    }

                    if (control.animationProgress.IsRunning(typeof(Attack)))
                    {
                        continue;
                    }

                    if (c.animationProgress.StateNameContains("RunningSlide"))
                    {
                        continue;
                    }

                    if (c.DamageDetector_Data.IsDead())
                    {
                        continue;
                    }

                    if (control.DamageDetector_Data.IsDead())
                    {
                        continue;
                    }

                    Debug.Log("InstaKill");
                    c.damageDetector.DeathByInstakill(control);

                    return;
                }
            }
        }

    }
}