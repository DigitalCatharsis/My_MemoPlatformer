using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace My_MemoPlatformer
{
    //1. Looking for key pressing in PlayerInput
    //2. The pressed keys saved up to the VirtualInputManager, and bind up
    //3. ManualInput Relay that keys to the CharacterControl
    //4. CharacterControl contains actual fields about character moving etc.
    public class CharacterControl : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] bool _debug;

        [Header("Input")]
        public bool moveUp;
        public bool moveDown;
        public bool moveRight;
        public bool moveLeft;
        public bool turbo;
        public bool jump;
        public bool attack;
        public bool block;

        [Header("SubComponents")]
        public SubComponentProcessor subComponentProcessor;

        //have to dispose
        public AnimationProgress animationProgress;
        public AIProgress aiProgress;
        public AIController aiController;
        public BoxCollider boxCollider;
        public NavMeshObstacle navMeshObstacle;

        public BlockingObj_Data BLOCKING_OBJ_DATA => subComponentProcessor.blockingObjData;
        public LedgeGrab_Data LEDGE_GRAB_DATA => subComponentProcessor.ledgeGrabData;
        public Ragdoll_Data RAGDOLL_DATA => subComponentProcessor.ragdollData;
        public ManualInput_Data MANUAL_INPUT_DATA => subComponentProcessor.manualInput_Data;
        public BoxCollider_Data BOX_COLLIDER_DATA => subComponentProcessor.boxCollider_Data;
        public DamageDetector_Data DAMAGE_DETECTOR_DATA => subComponentProcessor.damageDetector_Data;
        public MomentumCalculator_Data MOMENTUM_CACULATOR_DATA => subComponentProcessor.momentumCalculator_Data;
        public PlayerRotation_Data PLAYER_ROTATION_DATA => subComponentProcessor.playerRotation_Data;
        public PlayerJump_Data PLAYER_JUMP_DATA => subComponentProcessor.playerJump_Data;
        public CollisionSpheres_Data COLLISION_SPHERES_DATA => subComponentProcessor.collisionSpheres_Data;
        public InstaKill_Data INSTAKILL_DATA => subComponentProcessor.instaKill_Data;
        public PlayerGround_Data PLAYER_GROUND_DATA => subComponentProcessor.playerGround_Data;
        public PlayerAttack_Data PLAYER_ATTACK_DATA => subComponentProcessor.playerAttack_Data;
        public PlayerAnimation_Data PLAYER_ANIMATION_DATA => subComponentProcessor.playerAnimation_Data;

        [Header("Setup")]
        public PlayableCharacterType playableCharacterType;
        public Animator skinnedMeshAnimator;
        public GameObject rightHand_Attack;
        public GameObject leftHand_Attack;
        public GameObject leftFoot_Attack;
        public GameObject rightFoot_Attack;

        private Dictionary<string, GameObject> _childObjects = new Dictionary<string, GameObject>();

        private Rigidbody _rigidBody;
        public Rigidbody RIGID_BODY
        {
            get
            {
                if (_rigidBody == null)
                {
                    _rigidBody = GetComponent<Rigidbody>();
                }
                return _rigidBody;
            }
        }

        private void Awake()
        {
            subComponentProcessor = GetComponentInChildren<SubComponentProcessor>();
            animationProgress = GetComponent<AnimationProgress>();
            aiProgress = GetComponentInChildren<AIProgress>();
            boxCollider = GetComponent<BoxCollider>();
            navMeshObstacle = GetComponentInChildren<NavMeshObstacle>();

            aiController = GetComponentInChildren<AIController>();
            if (aiController == null)
            {
                if (navMeshObstacle != null)
                {
                    navMeshObstacle.carving = true;
                }
            }

            RegisterCharacter();
            InitCharactersStates(skinnedMeshAnimator);
        }

        private void Update()
        {
            subComponentProcessor.UpdateSubComponents();
        }

        private void FixedUpdate()
        {
            subComponentProcessor.FixedUpdateSubComponents();
        }

        private void OnCollisionStay(Collision collision)
        {
            PLAYER_GROUND_DATA.BoxColliderContacts = collision.contacts;
        }

        private void InitCharactersStates(Animator animator)  //Передает стейтам аниматора CharacterControl референс
        {
            CharacterState[] arr = animator.GetBehaviours<CharacterState>();

            foreach (CharacterState c in arr)
            {
                c.characterControl = this;   //For AI check awake in AiController
            }
        }
        private void RegisterCharacter()
        {
            if (!CharacterManager.Instance.characters.Contains(this))
            {
                CharacterManager.Instance.characters.Add(this);
            }
        }

        public void MoveForward(float speed, float speedGraph)
        {
            //Debug.Log($"Speed:{speed}#\tSpeedGraph:{speedGraph}#\tTime.DeltaTime:{Time.deltaTime}# \tTick: {Time.frameCount}");
            transform.Translate(Vector3.forward * speed * speedGraph * Time.deltaTime);
        }

        public GameObject GetChildObj(string name)
        {

            if (_childObjects.ContainsKey(name)) //check if Dictionary already has the object i am looking for
            {
                return _childObjects[name];
            }

            var ar = this.gameObject.GetComponentsInChildren<Transform>();

            foreach (var t in ar)
            {
                if (t.gameObject.name.Equals(name))
                {
                    _childObjects.Add(name, t.gameObject);
                    return t.gameObject;
                }
            }
            return null;
        }

        public GameObject GetAttackingPart(AttackPartType attackPart)
        {
            switch (attackPart)
            {
                case AttackPartType.LEFT_HAND:
                    {
                        return leftHand_Attack;
                    }
                case AttackPartType.RIGHT_HAND:
                    {
                        return rightHand_Attack;
                    }
                case AttackPartType.RIGHT_FOOT:
                    {
                        return rightFoot_Attack;
                    }
                case AttackPartType.LEFT_FOOT:
                    {
                        return leftFoot_Attack;
                    }
                case AttackPartType.MELEE_WEAPON:
                    {
                        return animationProgress.HoldingWeapon.triggerDetector.gameObject;
                    }
                default:
                    return null;
            }
        }
    }
}