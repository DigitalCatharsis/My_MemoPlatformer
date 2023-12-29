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

        public BlockingObj_Data BLOCKING_OBJ_DATA => subComponentProcessor.blockingObj_Data;
        public LedgeGrab_Data LEDGE_GRAB_DATA => subComponentProcessor.ledgeGrab_Data;
        public Ragdoll_Data RAGDOLL_DATA => subComponentProcessor.ragdoll_Data;
        public ManualInput_Data MANUAL_INPUT_DATA => subComponentProcessor.manualInput_Data;
        public BoxCollider_Data BOX_COLLIDER_DATA => subComponentProcessor.boxCollider_Data;
        public Damage_Data DAMAGE_DATA => subComponentProcessor.damage_Data;
        public MomentumCalculator_Data MOMENTUM_DATA => subComponentProcessor.momentumCalculator_Data;
        public Rotation_Data ROTATION_DATA => subComponentProcessor.rotation_Data;
        public Jump_Data JUMP_DATA => subComponentProcessor.jump_Data;
        public CollisionSpheres_Data COLLISION_SPHERE_DATA => subComponentProcessor.collisionSpheres_Data;
        public InstaKill_Data INSTA_KILL_DATA => subComponentProcessor.instaKill_Data;
        public Ground_Data GROUND_DATA => subComponentProcessor.ground_Data;
        public Attack_Data ATTACK_DATA => subComponentProcessor.attack_Data;
        public Animation_Data ANIMATION_DATA => subComponentProcessor.animation_Data;

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
            GROUND_DATA.BoxColliderContacts = collision.contacts;
        }

        private void InitCharactersStates(Animator animator)  //Передает стейтам аниматора CharacterControl референс
        {
            CharacterState[] arr = animator.GetBehaviours<CharacterState>();

            foreach (CharacterState c in arr)
            {
                c.characterControl = this;   //For AIState check awake in AiController
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
                        return animationProgress.holdingWeapon.triggerDetector.gameObject;
                    }
                default:
                    return null;
            }
        }
    }
}