using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class DamageDetector : MonoBehaviour  //Compare collision info versus attack input that is being registered
    {
        CharacterControl control;

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
            foreach (Collider collider in control.collidingParts) //control.collidingParts - список коллайдеров, задевающих владельца control
            {
                foreach (string name in info.colliderNames)  //
                {
                    if(name == collider.gameObject.name)  //Сопоставляем список действующих на владельца control колладеров с теми коллайдерами, которые активны за нужный attack
                    {
                        TakeDamage(info);
                        return true;
                    }
                }
            }
            return false;
        }

        private void TakeDamage(AttackInfo info)
        {
            Debug.Log(info.attacker.gameObject.name + " hits " + this.gameObject.name);
            control.skinnedMeshAnimator.runtimeAnimatorController = info.attackAbility.GetDeathAnimator();
            info.currentHits++;

            control.GetComponent<BoxCollider>().enabled = false;
            control.Rigid_Body.useGravity = false;
        }

    }
}