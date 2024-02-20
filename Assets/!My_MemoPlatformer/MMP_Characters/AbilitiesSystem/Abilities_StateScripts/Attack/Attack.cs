using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
        public float startAttackTime; //Is % of the animation duration
        public float endAttackTime; //Is % of the animation duration
        public List<AttackPartType> attackParts = new List<AttackPartType>();
        public bool mustCollide;
        public bool mustFaceAttacker;
        public float lethalRange;
        public int maxHits;
        public float damage;

        public NormalRagdollVelocity normalRagdollVelocity;
        public CollateralRagdollVelocity collateralRagdollVelocity;

        [Header("Death Particles")]
        public bool useDeathParticles;
        public VFXType particleType;

        [Space(10)]
        public CollateralDamageInfo collateralDamageInfo;

        private List<AttackCondition> _finishedAttacks = new List<AttackCondition>();
        private GameObject _spawnedAttackCondition;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.Attack_Data.attackTriggered = false;

            _spawnedAttackCondition = PoolManager.Instance.GetObject(DataType.AttackCondition, PoolManager.Instance.dataPoolDictionary, Vector3.zero, Quaternion.identity);
            var info = _spawnedAttackCondition.GetComponent<AttackCondition>();

            if (AttackManager.Instance.activeAttacks == null)
            {
                AttackManager.Instance.activeAttacks = new GameObject();
                AttackManager.Instance.activeAttacks.name = "ActiveAttacks";
                AttackManager.Instance.activeAttacks.transform.position = Vector3.zero;
                AttackManager.Instance.activeAttacks.transform.rotation = Quaternion.identity;
            }

            if (info.transform.parent == null)
            {
                info.transform.parent = AttackManager.Instance.activeAttacks.transform;
            }

            _spawnedAttackCondition.SetActive(true);
            info.ResetInfo(this, characterState.characterControl);

            if (!AttackManager.Instance.currentAttacks.Contains(info))
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
                foreach (var info in AttackManager.Instance.currentAttacks)
                {
                    if (info == null)
                    {
                        continue;
                    }

                    if (!info.isRegistered && info.attackAbility == this)
                    {
                        if (DebugContainer_Data.Instance.debug_Attack)
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
                        info.GetComponent<DataPoolObject>().TurnOff();

                        foreach (var control in CharacterManager.Instance.characters)
                        {
                            if (control.DAMAGE_DATA.blockedAttack == info)
                            {
                                control.DAMAGE_DATA.blockedAttack = null;
                            }
                        }

                        if (DebugContainer_Data.Instance.debug_Attack)
                        {
                            Debug.Log(this.name + " de-registered: " + stateInfo.normalizedTime);
                        }
                    }
                }
            }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            ClearAttack();
        }
        public void ClearAttack()
        {
            _finishedAttacks.Clear();

            foreach (AttackCondition info in AttackManager.Instance.currentAttacks)
            {
                if (info == null || info.attackAbility == this /*info.isFinished*/)
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
