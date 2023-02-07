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

        public int damageTaken; //templory

        private void Awake()
        {
            damageTaken = 0;
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

                if (info.mustFaceAttacker)
                {
                    Vector3 vec = this.transform.position - info.attacker.transform.position;  //¬ектор от жертвы до нападающего
                    if (vec.z * info.attacker.transform.forward.z < 0f) //ћы сравниваем 2 вектора, если они смотр€т в разные стороны, со один из них отрицательный, следовательно, перменожение решает смотр€т ли они друг на друга
                    {
                        continue;
                    }
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
                    //Debug.Log(this.gameObject.name + "dist: "+ dist.ToString() );
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
                    foreach (string name in info.colliderNames)  //»мена атакующих коллайдеров
                    {
                        if (name.Equals(collider.gameObject.name))  //—мотрим, что коллайдер атакующий
                        {
                            if (collider.transform.root.gameObject == info.attacker.gameObject) //фикс, если несколько игроков бьют врагов одновременно
                            {
                                _damagePart = trigger.generalBodyPart; // уда нанесли урон (upper и тд, смотри enum)
                                return true;
                            }
                            

                        }
                    }
                }
            }
            return false;
        }

        private void TakeDamage(AttackInfo info)
        {
            if (damageTaken > 0)  //templory fix for hitting dead enemy
            {
                //return;
            }

            if (info.mustCollide)
            {
                CameraManager.Instance.ShakeCamera(0.2f);
            }
            CameraManager.Instance.ShakeCamera(0.2f);

            Debug.Log(info.attacker.gameObject.name + " hits " + this.gameObject.name);
            Debug.Log(this.gameObject.name + " hit " + _damagePart.ToString());

            control.skinnedMeshAnimator.runtimeAnimatorController = DeathAnimationManager.Instance.GetAnimator(_damagePart, info);
            info.currentHits++;

            control.GetComponent<BoxCollider>().enabled = false;
            control.Rigid_Body.useGravity = false;

            damageTaken++; 
        }

    }
}