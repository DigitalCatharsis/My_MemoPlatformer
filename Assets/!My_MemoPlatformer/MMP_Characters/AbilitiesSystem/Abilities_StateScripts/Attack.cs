using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace My_MemoPlatformer
{

    public enum AttackPartType
    {
        LEFT_HAND,
        RIGHT_HAND,
        
        LEFT_FOOT,
        RIGHT_FOOT,

        MELEE_WEAPON,
    }

    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/Attack")]
    public class Attack : CharacterAbility
    {

        public bool debug;

        public float startAttackTime; //Is % of the animation duration
        public float endAttackTime; //Is % of the animation duration
        public List<AttackPartType> attackParts = new List<AttackPartType>();
        public bool mustCollide;
        public bool mustFaceAttacker;
        public float lethalRange;
        public int maxHits;
        public float damage;

        [Header("Combo")]
        public float comboStartTime;
        public float comboEndTime;

        [Header("Ragdoll Death")]
        public float forwardForce;
        public float rightForce;
        public float upForce;

        [Header("Death Particles")]
        public bool useDeathParticles;
        public PoolObjectType ParticleType;

        private List<AttackCondition> _finishedAttacks = new List<AttackCondition> ();

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.PLAYER_ATTACK_DATA.attackTriggered = false;

            animator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Attack], false);

            GameObject obj = PoolManager.Instance.GetObject(PoolObjectType.AttackCondition); //Ссылкаемя на объект в пул менеджере
            AttackCondition info = obj.GetComponent<AttackCondition>();

            obj.SetActive(true); //set it active when we first get it
            info.ResetInfo(this, characterState.characterControl);

            if (!AttackManager.Instance.currentAttacks.Contains(info)) //Making a list of current attacks
            {
                AttackManager.Instance.currentAttacks.Add(info);
            }

        }
    
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            RegisterAttack(characterState, animator, stateInfo);
            DeregisterAttack(characterState, animator, stateInfo);
            CheckCombo(characterState, animator, stateInfo);
        }

        public void RegisterAttack(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (startAttackTime <= stateInfo.normalizedTime && endAttackTime > stateInfo.normalizedTime)
            {
                foreach (AttackCondition info in AttackManager.Instance.currentAttacks)
                {
                    if (info == null)
                    {
                        continue;
                    }

                    if (!info.isRegistered && info.attackAbility == this)
                    {
                        if (debug)
                        {
                            Debug.Log(this.name + " registered: " + stateInfo.normalizedTime);
                        }

                        info.Register(this);
                    }

                }
            }
        }

        public void DeregisterAttack(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (stateInfo.normalizedTime >= endAttackTime)
            {
                foreach (AttackCondition info in AttackManager.Instance.currentAttacks)
                {
                    if (info == null)
                    {
                        continue;
                    }
                    if (info.attackAbility == this && !info.isFinished)
                    {
                        info.isFinished = true;
                        info.GetComponent<PoolObject>().TurnOff();

                        foreach (CharacterControl c in CharacterManager.Instance.characters)
                        {
                            if (c.DAMAGE_DETECTOR_DATA.blockedAttack == info)
                            {
                                c.DAMAGE_DETECTOR_DATA.blockedAttack = null;
                            }
                        }

                        if (debug)
                        {
                            Debug.Log(this.name + " deregistered: " + stateInfo.normalizedTime);
                        }

                    }

                }
            }
        }

        public void CheckCombo (CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (stateInfo.normalizedTime >= comboStartTime)   //to define when we press. wich procent of the animation i want to set combos start time
            {
                if (stateInfo.normalizedTime <= comboEndTime)
                {
                    if (characterState.characterControl.PLAYER_ATTACK_DATA.attackTriggered)
                    {
                        animator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Attack],true);
                    }    
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Attack], false);
            ClearAttack();
        }

        private void ClearAttack()
        {
            _finishedAttacks.Clear();

            foreach (AttackCondition info in AttackManager.Instance.currentAttacks)
            {
                if (info == null || info.attackAbility == this) 
                {
                    _finishedAttacks.Add(info);
                }
            }

            foreach (AttackCondition info in _finishedAttacks)
            {
                if (AttackManager.Instance.currentAttacks.Contains(info))
                {
                    AttackManager.Instance.currentAttacks.Remove(info);
                }
            }
        }
    }
}
