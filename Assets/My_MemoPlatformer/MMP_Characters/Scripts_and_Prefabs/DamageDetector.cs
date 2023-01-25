using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

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
                CheckAttack();
            }
        }

        private void CheckAttack()
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
                else
                {
                    float dist = Vector3.SqrMagnitude( this.gameObject.transform.position - info.attacker.transform.position); //distance between target and attacker
                    Debug.Log(this.gameObject.name + "dist: "+ dist.ToString() );
                    if (dist  <= info.lethalRange) 
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
                    foreach (string name in info.colliderNames)  //Имена атакующих коллайдеров
                    {
                        if (name == collider.gameObject.name)  //Смотрим, что коллайдер атакующий
                        {
                            _damagePart = trigger.generalBodyPart; //Куда нанесли урон (upper и тд, смотри enum)
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void TakeDamage(AttackInfo info)
        {
            if (info.mustCollide)
            {
                CameraManager.Instance.ShakeCamera(0.2f);
            }
            CameraManager.Instance.ShakeCamera(0.2f);

            Debug.Log(info.attacker.gameObject.name + " hits " + this.gameObject.name);
            Debug.Log(this.gameObject.name + " hit " + _damagePart.ToString());


            //control.skinnedMeshAnimator.runtimeAnimatorController = info.attackAbility.GetDeathAnimator();
            control.skinnedMeshAnimator.runtimeAnimatorController = DeathAnimationManager.Instance.GetAnimator(_damagePart, info);
            info.currentHits++;

            control.GetComponent<BoxCollider>().enabled = false;
            control.Rigid_Body.useGravity = false;
        }

    }
}