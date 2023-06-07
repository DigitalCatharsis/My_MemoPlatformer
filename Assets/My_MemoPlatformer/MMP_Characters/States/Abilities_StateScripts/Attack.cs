using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public enum AttackPartType
    {
        LEFT_HAND,
        RIGHT_HAND,
    }

    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/Attack")]
    public class Attack : StateData
    {

        public float startAttackTime; //Is % of the animation duration
        public float endAttackTime; //Is % of the animation duration
        //public List<string> colliderNames = new List<string>();  //name of the bodypards that gonna carry the attack
        public List<AttackPartType> attackParts = new List<AttackPartType>();
        public DeathType deathType;
        public bool mustCollide;
        public bool mustFaceAttacker;
        public float lethalRange;
        public int maxHits;
        public bool debug;

        private List<AttackInfo> _finishedAttacks = new List<AttackInfo> ();

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(TransitionParameter.Attack.ToString(), false);

            GameObject obj = PoolManager.Instance.GetObject(PoolObjectType.ATTACKINFO); //Ссылкаемя на объект в пул менеджере
            AttackInfo info = obj.GetComponent<AttackInfo>();

            obj.SetActive(true); //set it active when we first get it
            info.ResetInfo(this, characterState.GetCharacterControl(animator));

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
                foreach (AttackInfo info in AttackManager.Instance.currentAttacks)
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
                foreach (AttackInfo info in AttackManager.Instance.currentAttacks)
                {
                    if (info == null)
                    {
                        continue;
                    }
                    if (info.attackAbility == this && !info.isFinished)
                    {
                        info.isFinished = true;
                        info.GetComponent<PoolObject>().TurnOff();

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
            if (stateInfo.normalizedTime >= startAttackTime + ((endAttackTime - startAttackTime) /3f))   //to define when we press. wich procent of the animation i want to set combos start time
            {
                if (stateInfo.normalizedTime < endAttackTime + ((endAttackTime - startAttackTime) / 2f))
                {
                    CharacterControl control = characterState.GetCharacterControl(animator);
                    if (control.animationProgress.attackTriggered)
                    {
                        //Debug.Log("uppercut triggered");
                        animator.SetBool(TransitionParameter.Attack.ToString(),true);
                    }    
                }
            }
        }



        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(TransitionParameter.Attack.ToString(), false);
            ClearAttack();
        }
        private void ClearAttack()
        {
            _finishedAttacks.Clear();

            foreach (AttackInfo info in AttackManager.Instance.currentAttacks)
            {
                if (info == null || info.attackAbility == this) 
                {
                    _finishedAttacks.Add(info);
                }
            }

            foreach (AttackInfo info in _finishedAttacks)
            {
                if (AttackManager.Instance.currentAttacks.Contains(info))
                {
                    AttackManager.Instance.currentAttacks.Remove(info);
                }
            }
        }
    }
}
