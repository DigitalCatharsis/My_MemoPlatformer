using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/Attack")]
    public class Attack : StateData
    {

        public float startAttackTime; //Is % of the animation
        public float endAttackTime; //Is % of the animation
        public List<string> colliderNames = new List<string>();  //name of the bodypards that gonna carry the attack

        public bool mustCollide;
        public bool mustFaceAttacker;
        public float lethalRange;
        public int maxHits;
        //public List<RuntimeAnimatorController> deathAnimators = new List<RuntimeAnimatorController>(); //list of death anination associated with attack?

        private List<AttackInfo> finishedAttacks = new List<AttackInfo> ();

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(TransitionParameter.Attack.ToString(), false);

            //GameObject obj = Instantiate(Resources.Load("AttackInfo", typeof(GameObject))) as GameObject;
            GameObject obj = PoolManager.Instance.GetObject(PoolObjectType.ATTACKINFO); //Ссылкаемя на объект в пул менеджере
            AttackInfo info = obj.GetComponent<AttackInfo>();

            obj.SetActive(false); //set it active when we first get it
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
                        //Destroy(info.gameObject);
                        //info.deregister(this, characterState.GetCharacterControl(animator));
                    }

                }
            }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            ClearAttack();
        }
        private void ClearAttack()
        {

            foreach (AttackInfo info in AttackManager.Instance.currentAttacks)
            {
                if (info == null || info.isFinished)
                {
                    finishedAttacks.Add(info);
                }
            }

            foreach (AttackInfo info in finishedAttacks)
            {
                if (AttackManager.Instance.currentAttacks.Contains(info))
                {
                    AttackManager.Instance.currentAttacks.Remove(info);
                }
            }
        }

        //public RuntimeAnimatorController GetDeathAnimator()
        //{
        //    int index = Random.Range(0, deathAnimators.Count);
        //    return deathAnimators[index];
        //}
    }
}
