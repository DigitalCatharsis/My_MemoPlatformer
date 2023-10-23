using UnityEngine;

namespace My_MemoPlatformer
{

    public class DamageDetector : MonoBehaviour  //Compare collision info versus attack input that is being registered
    {
        private CharacterControl _control;

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
                    Vector3 vec = this.transform.position - info.attacker.transform.position;  //������ �� ������ �� �����������
                    if (vec.z * info.attacker.transform.forward.z < 0f) //�� ���������� 2 �������, ���� ��� ������� � ������ �������, �� ���� �� ��� �������������, �������������, ������������ ������ ������� �� ��� ���� �� �����
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
                foreach (Collider collider in trigger.collidingParts) //control.collidingParts - ������ �����������, ���������� ��������� control
                {
                    foreach (AttackPartType part in info.attackParts)  //����� ��������� �����������
                    {
                        if (part == AttackPartType.LEFT_HAND)
                        {
                            if (collider.gameObject == info.attacker.leftHand_Attack)
                            {
                                _control.animationProgress.attack = info.attackAbility;
                                _control.animationProgress.attacker = info.attacker;
                                _control.animationProgress.damagedTrigger = trigger; 
                                return true;    
                            }
                        }
                        else if (part == AttackPartType.RIGHT_HAND)
                        {
                            if (collider.gameObject == info.attacker.rightHand_Attack)
                            {
                                _control.animationProgress.attack = info.attackAbility;
                                _control.animationProgress.attacker = info.attacker;
                                _control.animationProgress.damagedTrigger = trigger;
                                return true;
                            }
                        }
                        else if (part == AttackPartType.LEFT_FOOT)
                        {
                            if (collider.gameObject == info.attacker.leftFoot_Attack)
                            {
                                _control.animationProgress.attack = info.attackAbility;
                                _control.animationProgress.attacker = info.attacker;
                                _control.animationProgress.damagedTrigger = trigger;
                                return true;
                            }
                        }
                        else if (part == AttackPartType.RIGHT_FOOT)
                        {
                            if (collider.gameObject == info.attacker.rightFoot_Attack)
                            {
                                _control.animationProgress.attack = info.attackAbility;
                                _control.animationProgress.attacker = info.attacker;
                                _control.animationProgress.damagedTrigger = trigger;
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

                if (info.attackAbility.useDeathParticles)
                {
                    if (info.attackAbility.ParticleType.ToString().Contains("VFX"))
                    {
                        GameObject vfx =  PoolManager.Instance.GetObject(info.attackAbility.ParticleType);

                        vfx.transform.position = _control.animationProgress.damagedTrigger.transform.position;
                    }
                }
            }

            if (_debug)
            {
                Debug.Log(info.attacker.gameObject.name + " hits " + this.gameObject.name);
            }

            info.currentHits++;
            damageTaken++;

            AttackManager.Instance.ForceDeregester(_control);

            _control.animationProgress.ragdollTriggered = true;
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