using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace My_MemoPlatformer
{
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
    }

    public enum MMP_Scenes
    {
        L_CharacterSelect,
        L_Level_Start,
        L_Level_Sample_PathFinding,
        L_Level_Sample_Hammer,
        L_LeveL_LedgeGrab,
        L_Level_Sample_Demo,
    }


    public class CharacterControl : MonoBehaviour
    {
        [Header("Input")]
        public bool moveUp;
        public bool moveDown;
        public bool moveRight;
        public bool moveLeft;
        public bool turbo;
        public bool jump;
        public bool attack;

        [Header("SubComponents")]
        public LedgeChecker ledgeChecker;
        public AnimationProgress animationProgress;
        public AIProgress aiProgress;
        public DamageDetector damageDetector;
        public List<GameObject> bottomSpheres = new List<GameObject>();
        public List<GameObject> frontSpheres = new List<GameObject>();
        public AIController aiController;
        public BoxCollider boxCollider;
        public NavMeshObstacle navMeshObstacle;

        [Header("Gravity")]
        public float gravityMultipliyer;
        public float pullMultipliyer;
        public ContactPoint[] contactPoints;

        [Header("Setup")]
        public PlayableCharacterType playableCharacterType;
        public Animator skinnedMeshAnimator;
        public List<Collider> ragdollParts = new List<Collider>();
        public GameObject rightHandAttack;
        public GameObject leftHandAttack;

        private List<TriggerDetector> _triggerDetectors = new List<TriggerDetector>();
        private Dictionary<string, GameObject> _childObjects = new Dictionary<string, GameObject>();

        private Rigidbody _rigid;
        public Rigidbody Rigid_Body
        {
            get
            {
                if (_rigid == null)
                {
                    _rigid = GetComponent<Rigidbody>();
                }
                return _rigid;
            }
        }

        private void Awake()
        {
            ledgeChecker = GetComponentInChildren<LedgeChecker>();
            animationProgress = GetComponent<AnimationProgress>();
            aiProgress = GetComponentInChildren<AIProgress>();
            damageDetector = GetComponentInChildren<DamageDetector>();
            aiController = GetComponentInChildren<AIController>();
            boxCollider = GetComponent<BoxCollider>();
            navMeshObstacle = GetComponentInChildren<NavMeshObstacle>();

            SetColliderSpheres();

            RegisterCharacter();
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

        public List<TriggerDetector> GetAllTriggers()
        {
            if (_triggerDetectors.Count == 0)
            {
                TriggerDetector[] arr = this.gameObject.GetComponentsInChildren<TriggerDetector>();

                foreach (TriggerDetector d in arr)
                {
                    _triggerDetectors.Add(d);
                }
            }

            return _triggerDetectors;
        }

        public void SetRagdollParts()
        {
            ragdollParts.Clear();

            Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>(); //Get all the colliders in the hierarchy

            foreach (Collider c in colliders)
            {
                if (c.gameObject != this.gameObject)  //if the collider that we found is not the same as in the charactercontrol (//not a boxcolllider itself)
                {
                    if (c.gameObject.GetComponent<LedgeChecker>() == null)
                    {
                        //thats means its a ragdoll
                        c.isTrigger = true;
                        ragdollParts.Add(c);
                        c.attachedRigidbody.interpolation = RigidbodyInterpolation.Interpolate;  //убрать дрожжание
                        c.attachedRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; //расчет физики, предотвращение прохождения сквозь объекты

                        CharacterJoint joint = c.GetComponent<CharacterJoint>();
                        if (joint != null)
                        {
                            joint.enableProjection = true; //https://docs.unity3d.com/Manual/RagdollStability.html
                        }

                        if (c.GetComponent<TriggerDetector>() == null)
                        {
                            c.gameObject.AddComponent<TriggerDetector>();
                        }
                    }
                }
            }
        }

        public void TurnOnRagdoll()
        {
            //change components layers from character to DeadBody to prevent unnessesary collisions.
            Transform[] arr = GetComponentsInChildren<Transform>();
            foreach (Transform t in arr)
            {
                t.gameObject.layer = LayerMask.NameToLayer(MMP_Layers.DeadBody.ToString());
            }

            //save bodypart positions to prevent teleporting
            foreach (Collider c in ragdollParts)
            {
                TriggerDetector det = c.GetComponent<TriggerDetector>();
                det.lastPosition = c.gameObject.transform.localPosition;
                det.lastRotation = c.gameObject.transform.localRotation;
            }

            //turn off animator, avatar, smth else
            Rigid_Body.useGravity = false;
            Rigid_Body.velocity = Vector3.zero;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            skinnedMeshAnimator.enabled = false;
            skinnedMeshAnimator.avatar = null;

            //turn on ragdoll
            foreach (Collider c in ragdollParts)
            {
                c.isTrigger = false;

                TriggerDetector det = c.GetComponent<TriggerDetector>();
                c.transform.localPosition = det.lastPosition;
                c.transform.localRotation = det.lastRotation;

                c.attachedRigidbody.velocity = Vector3.zero;
            }
        }

        private void SetColliderSpheres()
        {            
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(Resources.Load("ColliderEdge", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                bottomSpheres.Add(obj);
                obj.transform.parent = this.transform;
            }

            Reposition_BottomSpheres();

            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(Resources.Load("ColliderEdge", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                frontSpheres.Add(obj);
                obj.transform.parent = this.transform;
            }

            Reposition_FrontSpheres();
        }

        public void Reposition_FrontSpheres()
        {
            float bottom = boxCollider.bounds.center.y - boxCollider.bounds.size.y /2; // в центре внизу. 
            float top = boxCollider.bounds.center.y + boxCollider.bounds.size.y /2; // в центре вверху. ;
            float front = boxCollider.bounds.center.z + boxCollider.bounds.size.z /2; // в центре спереди. ;
            //float back = boxCollider.bounds.center.z - boxCollider.bounds.size.z; // в центре сзади. ;;

            frontSpheres[0].transform.localPosition = new Vector3(0f, bottom + 0.05f, front) - this.transform.position;
            frontSpheres[1].transform.localPosition = new Vector3(0f, top, front) - this.transform.position;

            float interval = (top - bottom + 0.05f) / 9;

            for (int i = 2; i < frontSpheres.Count; i++)
            {
                frontSpheres[i].transform.localPosition = new Vector3(0f, bottom + (interval * (i - 1)), front)
                     - this.transform.position;
            }
        }

        public void Reposition_BottomSpheres()
        {
            float bottom = boxCollider.bounds.center.y - boxCollider.bounds.size.y /2; // в центре внизу. 
            //float top = boxCollider.bounds.center.y + boxCollider.bounds.size.y; // в центре вверху. ;
            float front = boxCollider.bounds.center.z + boxCollider.bounds.size.z /2; // в центре спереди. ;
            float back = boxCollider.bounds.center.z - boxCollider.bounds.size.z /2; // в центре сзади. ;;

            bottomSpheres[0].transform.localPosition = new Vector3(0f, bottom, back) - this.transform.position;
            bottomSpheres[1].transform.localPosition = new Vector3(0f, bottom, front) - this.transform.position;

            float interval = (front - back) / 4;

            for (int i = 2; i < bottomSpheres.Count; i++)
            {
                bottomSpheres[i].transform.localPosition = new Vector3(0f, bottom, back + (interval * (i - 1)))
                     - this.transform.position;
            }
        }

        public void UpdateBoxColliderSize()
        {
            if (!animationProgress.updatingBoxCollider)
            {
                return;
            }

            if (Vector3.SqrMagnitude(boxCollider.size - animationProgress.targetSize) > 0.01f)
            {
                boxCollider.size = Vector3.Lerp(boxCollider.size, animationProgress.targetSize, Time.deltaTime * animationProgress.sizeSpeed);

                animationProgress.updatingSpheres = true;
            }
        }

        public void UpdateBoxColliderCenter()
        {
            if (!animationProgress.updatingBoxCollider)
            {
                return;
            }

            if (Vector3.SqrMagnitude(boxCollider.center - animationProgress.targetCenter) > 0.01f)
            {
                boxCollider.center = Vector3.Lerp(boxCollider.center, animationProgress.targetCenter, Time.deltaTime * animationProgress.centerSpeed);

                animationProgress.updatingSpheres = true;
            }
        }

        private void FixedUpdate()
        {
            //fall
            if (!animationProgress.cancelPull)
            {
                if (Rigid_Body.velocity.y < 0f)
                {
                    Rigid_Body.velocity += (-Vector3.up * gravityMultipliyer);
                }

                if (Rigid_Body.velocity.y > 0f && !jump)
                {
                    Rigid_Body.velocity += (-Vector3.up * pullMultipliyer);
                }
            }

            //Spheres
            animationProgress.updatingSpheres = false;
            UpdateBoxColliderSize();
            UpdateBoxColliderCenter();
            if (animationProgress.updatingSpheres)
            {
                Reposition_FrontSpheres();
                Reposition_BottomSpheres();
            }

            //ragdoll
            if (animationProgress.ragdollTriggered)
            {
                TurnOnRagdoll();
                animationProgress.ragdollTriggered = false;
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
            foreach (Collider c in ragdollParts)
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
    }
}