using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class DamageDetector : MonoBehaviour  //Compare collision info versus attack input that is being registered
    {
        CharacterControl control;
        GeneralBodyPart _damagePart; 

        private void Awake()
        {
            control = GetComponent<CharacterControl>();

        }

        private void Update()
        {
            if (AttackManager.Instance.currentAttacks.Count > 0)
            {
                checkAttack();
            }
        }

        private void checkAttack()
        {
            foreach(AttackInfo info in AttackManager.Instance.currentAttacks)
            {
                if (info == null)
                {
                    continue;   
                }

                if (!info.isRegistered)
                {
                    continue;
                }

                if (info.isFinished)
                {
                    continue;
                }

                if (info.currentHits >= info.maxHits)
                {
                    continue;
                }

                if(info.attacker == control)
                {
                    continue;
                }

                if (info.mustCollide)
                {
                    if (IsCollided(info))
                    {
                        TakeDamage(info);
                    }
                }
            }
        }

        private bool IsCollided(AttackInfo info)
        {
            foreach (TriggerDetector trigger in control.GetAllTriggers())
            {
                foreach (Collider collider in trigger.collidingParts) //control.collidingParts - список коллайдеров, задевающих владельца control
                {
                    foreach (string name in info.colliderNames)  //
                    {
                        if (name == collider.gameObject.name)  //Сопоставляем список действующих на владельца control колладеров с теми коллайдерами, которые активны за нужный attack
                        {
                            _damagePart = trigger.generalBodyPart;
                            TakeDamage(info);
                            return true;
                        }
                    }
                }
            }


            return false;
        }

        private void TakeDamage(AttackInfo info)
        {
            CameraManager.Instance.ShakeCamera(0.35f);

            Debug.Log(info.attacker.gameObject.name + " hits " + this.gameObject.name);
            Debug.Log(this.gameObject.name + " hit " + _damagePart.ToString());

            //control.skinnedMeshAnimator.runtimeAnimatorController = info.attackAbility.GetDeathAnimator();
            control.skinnedMeshAnimator.runtimeAnimatorController = DeathAnimationManager.Instance.GetAnimator(_damagePart);
            info.currentHits++;

            control.GetComponent<BoxCollider>().enabled = false;
            control.Rigid_Body.useGravity = false;
        }

    }
}