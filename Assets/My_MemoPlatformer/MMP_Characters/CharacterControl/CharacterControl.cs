using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    public enum MMP_Scenes
    {
        L_CharacterSelect,
        L_LevelStart,
    }


    public class CharacterControl : MonoBehaviour
    {
        public PlayableCharacterType playableCharacterType;
        [SerializeField] public Animator skinnedMeshAnimator;
        public bool moveUp;
        public bool moveDown;
        public bool moveRight;
        public bool moveLeft;
        public bool jump;
        public bool attack;
        public LedgeChecker ledgeChecker;

        [SerializeField] private GameObject colliderEdgePrefab;
        public List<GameObject> bottomSpheres = new List<GameObject>();
        public List<GameObject> frontSpheres = new List<GameObject>();
        public List<Collider> ragdollParts = new List<Collider>();

        public float gravityMultipliyer;
        public float pullMultipliyer;

        private List<TriggerDetector> _triggerDetectors = new List<TriggerDetector>();
        private Dictionary<string, GameObject> _childObjects= new Dictionary<string, GameObject>();

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
            bool switchBack = false;

            if (!IsFacingForward())
            {
                switchBack = true;
            }

            FaceForward(true);
            SetColliderSpheres();

            if (switchBack)
            {
                FaceForward(false);
            }

            ledgeChecker = GetComponentInChildren<LedgeChecker>();

            RegisterCharacter();

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
                    //thats means its a ragdoll
                    c.isTrigger = true;
                    ragdollParts.Add(c);

                    if (c.GetComponent<TriggerDetector>() == null)
                    {
                        c.gameObject.AddComponent<TriggerDetector>();
                    }
                }
            }
        }



        private void TurnOnRagdoll()
        {
            Rigid_Body.useGravity = false;
            Rigid_Body.velocity = Vector3.zero;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            skinnedMeshAnimator.enabled = false;
            skinnedMeshAnimator.avatar = null;

            foreach (Collider c in ragdollParts)
            {
                c.isTrigger = false;
                c.attachedRigidbody.velocity = Vector3.zero;
            }
        }



        private void SetColliderSpheres()
        {
            BoxCollider box = GetComponent<BoxCollider>();

            float bottom = box.bounds.center.y - box.bounds.extents.y; // в центре внизу. 
            float top = box.bounds.center.y + box.bounds.extents.y; // в центре вверху. ;
            float front = box.bounds.center.z + box.bounds.extents.z; // в центре спереди. ;
            float back = box.bounds.center.z - box.bounds.extents.z; // в центре сзади. ;;

            GameObject bottomFront = CreateEdgeSphere(new Vector3(0f, bottom, front));
            GameObject bottomBack = CreateEdgeSphere(new Vector3(0f, bottom, back));
            GameObject topFront = CreateEdgeSphere(new Vector3(0f, top, front));
            //GameObject topBack = CreateEdgeSphere(new Vector3(0f, top, front));

            bottomFront.transform.parent = this.transform; //Делаем его дочерним
            bottomBack.transform.parent = this.transform;
            topFront.transform.parent = this.transform;

            bottomSpheres.Add(bottomFront);
            bottomSpheres.Add(bottomBack);

            frontSpheres.Add(bottomFront);
            frontSpheres.Add(topFront);
            //frontSpheres.Add(topBack);

            float horSec = (bottomFront.transform.position - bottomBack.transform.position).magnitude / 5f; //Получаем одну секцию длинны, деленной на пять            
            CreateMiddleSpheres(bottomFront, -this.transform.forward, horSec, 4, bottomSpheres);


            float verSec = (bottomFront.transform.position - topFront.transform.position).magnitude / 10f; //Получаем одну секцию длинны, деленной на 10
            CreateMiddleSpheres(bottomFront, this.transform.up, verSec, 9, frontSpheres);
        }
        private void FixedUpdate()
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

        public void CreateMiddleSpheres(GameObject start, Vector3 dir, float sec, int interation, List<GameObject> spheresList)
        {

            for (int i = 0; i < interation; i++)
            {
                Vector3 pos = start.transform.position + (dir * sec * (i + 1));  //Получаем секцию

                GameObject newObj = CreateEdgeSphere(pos); //Спавним в каждой секции сферу
                newObj.transform.parent = this.transform; //Делаем его дочерним
                spheresList.Add(newObj);  //добавляем в список
            }
        }
        private GameObject CreateEdgeSphere(Vector3 pos)
        {
            GameObject obj = Instantiate(colliderEdgePrefab, pos, Quaternion.identity);
            return obj;
        }

        public void MoveForward(float speed, float speedGraph)
        {
            transform.Translate(speed * speedGraph * Time.deltaTime * Vector3.forward);
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
                    _childObjects.Add(name,t.gameObject);
                    return t.gameObject;
                }

            }

            return null;

        }

    }
}