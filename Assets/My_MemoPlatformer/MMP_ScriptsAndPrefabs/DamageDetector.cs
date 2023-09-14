using UnityEngine;

namespace My_MemoPlatformer
{

    public class DamageDetector : MonoBehaviour  //Compare collision info versus attack input that is being registered
    {
        private CharacterControl _control;
        private GeneralBodyPart _damagePart;

        [SerializeField] private bool _debug;

        public int damageTaken; //templory

        private void Awake()
        {
            damageTaken = 0;
            _control = GetComponent<CharacterControl>();
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
            foreach (AttackInfo info in AttackManager.Instance.currentAttacks)
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

                if (info.attacker == _control)
                {
                    continue;
                }

                if (info.mustFaceAttacker)
                {
                    Vector3 vec = this.transform.position - info.attacker.transform.position;  //Вектор от жертвы до нападающего
                    if (vec.z * info.attacker.transform.forward.z < 0f) //Мы сравниваем 2 вектора, если они смотрят в разные стороны, со один из них отрицательный, следовательно, перменожение решает смотрят ли они друг на друга
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
                    float dist = Vector3.SqrMagnitude(this.gameObject.transform.position - info.attacker.transform.position); //distance between target and attacker
                    //Debug.Log(this.gameObject.name + "dist: "+ dist.ToString() );
                    if (dist <= info.lethalRange)
                    {
                        TakeDamage(info);
                    }
                }
            }
        }

        private bool IsCollided(AttackInfo info)
        {
            foreach (TriggerDetector trigger in _control.GetAllTriggers())
            {
                foreach (Collider collider in trigger.collidingParts) //control.collidingParts - список коллайдеров, задевающих владельца control
                {
                    foreach (AttackPartType part in info.attackParts)  //Имена атакующих коллайдеров
                    {
                        if (part == AttackPartType.LEFT_HAND)
                        {
                            if (collider.gameObject == info.attacker.leftHand_Attack)
                            {
                                _damagePart = trigger.generalBodyPart; //Куда нанесли урон (upper и тд, смотри enum)
                                return true;
                            }
                        }
                        else if (part == AttackPartType.RIGHT_HAND)
                        {
                            if (collider.gameObject == info.attacker.rightHand_Attack)
                            {
                                _damagePart = trigger.generalBodyPart; //Куда нанесли урон (upper и тд, смотри enum)
                                return true;
                            }
                        }
                        else if (part == AttackPartType.LEFT_FOOT)
                        {
                            if (collider.gameObject == info.attacker.leftFoot_Attack)
                            {
                                _damagePart = trigger.generalBodyPart; //Куда нанесли урон (upper и тд, смотри enum)
                                return true;
                            }
                        }
                        else if (part == AttackPartType.RIGHT_FOOT)
                        {
                            if (collider.gameObject == info.attacker.rightFoot_Attack)
                            {
                                _damagePart = trigger.generalBodyPart; //Куда нанесли урон (upper и тд, смотри enum)
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
                return;
            }

            if (info.mustCollide)
            {
                CameraManager.Instance.ShakeCamera(0.2f);
            }
            CameraManager.Instance.ShakeCamera(0.2f);

            if (_debug)
            {
                Debug.Log(info.attacker.gameObject.name + " hits " + this.gameObject.name);
                Debug.Log(this.gameObject.name + " hit " + _damagePart.ToString());
            }

            if (!info.useRagdollDeath)
            {
                _control.skinnedMeshAnimator.runtimeAnimatorController = DeathAnimationManager.Instance.GetAnimator(_damagePart, info);
            }
            else
            {
                _control.animationProgress.ragdollTriggered = true;
            }


            info.currentHits++;

            _control.GetComponent<BoxCollider>().enabled = false;
            _control.ledgeChecker.GetComponent<BoxCollider>().enabled = false;
            _control.Rigid_Body.useGravity = false;

            if (_control.aiController != null)
            {
                _control.aiController.gameObject.SetActive(false);
                _control.navMeshObstacle.carving = false; //we dont need carving when enemy is dead
            }

            damageTaken++;
        }

    }
}