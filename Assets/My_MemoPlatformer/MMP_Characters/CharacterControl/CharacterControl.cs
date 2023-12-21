using My_MemoPlatformer.Datasets;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.AI;

namespace My_MemoPlatformer
{
    //1. Looking for key pressing in PlayerInput
    //2. The pressed keys saved up to the VirtualInputManager, and bind up
    //3. ManualInput Relay that keys to the CharacterControl
    //4. CharacterControl contains actual fields about character moving etc.
    public enum TransitionParameter
    {
        Move,
        Jump,
        ForceTransition,
        Grounded,
        Attack,
        ClickAnimation,
        TransitionIndex,
        Turbo,
        Turn,
        LockTransition,
    }

    public enum MMP_Scenes
    {
        L_CharacterSelect,
        L_Level_Start,
        L_Level_Day
    }

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
        public AnimationProgress animationProgress;
        public AIProgress aiProgress;
        public DamageDetector damageDetector;
        public CollisionSpheres collisionSpheres;
        public AIController aiController;
        public BoxCollider boxCollider;
        public NavMeshObstacle navMeshObstacle;
        public InstaKill instakill;

        public DataProcessor dataProcessor;

        public BlockingObjData BlockingObjData => subComponentProcessor.blockingObjData;
        public LedgeGrab_Data LedgeGrabData => subComponentProcessor.ledgeGrabData;
        public RagdollData RagdollData => subComponentProcessor.ragdollData;

        public Dataset Air_Control => dataProcessor.GetDataset(typeof(AirControl_Dataset));
        
        public Dictionary<BoolData, GetBool> boolDic = new Dictionary<BoolData, GetBool>();
        public delegate bool GetBool();

        [Header("Gravity")]
        public ContactPoint[] contactPoints;

        [Header("Setup")]
        public PlayableCharacterType playableCharacterType;
        public Animator skinnedMeshAnimator;
        public GameObject rightHand_Attack;
        public GameObject leftHand_Attack;
        public GameObject leftFoot_Attack;
        public GameObject rightFoot_Attack;

        private Dictionary<string, GameObject> _childObjects = new Dictionary<string, GameObject>();

        private Rigidbody _rigidBody;
        public Rigidbody Rigid_Body
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
            damageDetector = GetComponentInChildren<DamageDetector>();
            boxCollider = GetComponent<BoxCollider>();
            navMeshObstacle = GetComponentInChildren<NavMeshObstacle>();
            instakill = GetComponentInChildren<InstaKill>();

            collisionSpheres = GetComponentInChildren<CollisionSpheres>();
            collisionSpheres.owner = this;
            collisionSpheres.SetColliderSpheres();

            dataProcessor = this.gameObject.GetComponentInChildren<DataProcessor>();
            System.Type[] arr = { typeof(AirControl_Dataset) };
            dataProcessor.InitializeSets(arr);

            aiController = GetComponentInChildren<AIController>();
            if (aiController == null)
            {
                if (navMeshObstacle != null)
                {
                    navMeshObstacle.carving = true;
                }
            }

            RegisterCharacter();
        }        
        public void AddForceToDamagedPart(bool zeroZelocity)
        {
            if (damageDetector.damagedTrigger != null)
            {
                if (zeroZelocity)
                {
                    foreach (Collider c in RagdollData.bodyParts)
                    {
                        c.attachedRigidbody.velocity = Vector3.zero;
                    }
                }

                damageDetector.damagedTrigger.GetComponent<Rigidbody>().
                     AddForce(damageDetector.attacker.transform.forward * damageDetector.attack.forwardForce +
                          damageDetector.attacker.transform.right * damageDetector.attack.rightForce +
                             damageDetector.attacker.transform.up * damageDetector.attack.upForce);
            }
        }

        public void CacheCharacterControl(Animator animator)  //Передает стейтам аниматора CharacterControl референс
        {
            CharacterState[] arr = animator.GetBehaviours<CharacterState>();

            foreach (CharacterState c in arr)
            {
                c.characterControl = this;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            contactPoints = collision.contacts;
        }

        private void RegisterCharacter()
        {
            if (!CharacterManager.Instance.characters.Contains(this))
            {
                CharacterManager.Instance.characters.Add(this);
            }
        }

        public void UpdateBoxColliderSize()
        {
            if (!animationProgress.IsRunning(typeof(UpdateBoxCollider)))
            {
                return;
            }

            if (Vector3.SqrMagnitude(boxCollider.size - animationProgress.targetSize) > 0.00001f)
            {
                boxCollider.size = Vector3.Lerp(boxCollider.size, animationProgress.targetSize, Time.deltaTime * animationProgress.sizeSpeed);

                animationProgress.updatingSpheres = true;
            }
        }

        public void UpdateBoxColliderCenter()
        {
            if (!animationProgress.IsRunning(typeof(UpdateBoxCollider)))
            {
                return;
            }

            if (Vector3.SqrMagnitude(boxCollider.center - animationProgress.targetCenter) > 0.0001f)
            {
                boxCollider.center = Vector3.Lerp(boxCollider.center, animationProgress.targetCenter, Time.deltaTime * animationProgress.centerSpeed);

                animationProgress.updatingSpheres = true;
            }
        }
        private void Update()
        {
            subComponentProcessor.UpdateSubComponents();
        }

        private void FixedUpdate()
        {
            subComponentProcessor.FixedUpdateSubComponents();

            var cancelPull = Air_Control.GetBool((int)AirControlBool.CANCEL_PULL);

            //fall
            if (!cancelPull)
            {
                if (Rigid_Body.velocity.y > 0f && !jump)
                {
                    Rigid_Body.velocity -= (Vector3.up * Rigid_Body.velocity.y * 0.1f);    //Высота прыжка в зависимости от длительности нажатия
                }
            }

            //Spheres
            animationProgress.updatingSpheres = false;
            UpdateBoxColliderSize();
            UpdateBoxColliderCenter();
            if (animationProgress.updatingSpheres)
            {
                collisionSpheres.Reposition_FrontSpheres();
                collisionSpheres.Reposition_BottomSpheres();
                collisionSpheres.Reposition_BackSpheres();
                collisionSpheres.Reposition_UpSpheres();

                if (animationProgress.isLanding)  //prevent bug when idle after catching corner of platform
                {
                    if (_debug)
                    {
                        Debug.Log("repositioning y");
                    }
                    Rigid_Body.MovePosition(new Vector3(0f, animationProgress.landingPosition.y, this.transform.position.z));
                }
            }

            var maxFallVelocity = Air_Control.GetVecto3((int)AirControlVector3.MAX_FALL_VELOCITY);

            //slow down wallslide
            if (maxFallVelocity.y != 0f)
            {
                if (Rigid_Body.velocity.y <= maxFallVelocity.y)
                {
                    Rigid_Body.velocity = maxFallVelocity;
                }
            }
        }

        public void MoveForward(float speed, float speedGraph)
        {
            //Debug.Log($"Speed:{speed}#\tSpeedGraph:{speedGraph}#\tTime.DeltaTime:{Time.deltaTime}# \tTick: {Time.frameCount}");
            transform.Translate(Vector3.forward * speed * speedGraph * Time.deltaTime);
        }

        public void FaceForward(bool forward)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(MMP_Scenes.L_CharacterSelect.ToString()))
            {
                return;
            }

            if (!skinnedMeshAnimator.enabled)   //to prevent rotating after death
            {
                return;
            }

            if (forward)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

        public bool IsFacingForward()
        {
            if (transform.forward.z > 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Collider GetBodyPart(string name)
        {
            foreach (Collider c in RagdollData.bodyParts)
            {
                if (c.name.Contains(name))
                {
                    return c;
                }
            }
            return null;
        }

        public GameObject GetChildObj(string name)
        {

            if (_childObjects.ContainsKey(name)) //check if Dictionary already has the object i am looking for
            {
                return _childObjects[name];
            }

            Transform[] ar = this.gameObject.GetComponentsInChildren<Transform>();

            foreach (Transform t in ar)
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