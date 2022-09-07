using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public enum TransitionParameter
    {
        Move,
        Jump,
        ForceTransition,
        Grounded,
        Attack,
    }


    public class CharacterControl : MonoBehaviour
    {
        // [SerializeField] private Camera _Playercamera;
        //private Vector3 _cameraOffset = new Vector3(0, 2.0f, -12.0f);

        [SerializeField] private Animator SkinnedMeshAnimator;
        public bool MoveRight;
        public bool MoveLeft;
        public bool Jump;
        public bool Attack;

        [SerializeField] private GameObject ColliderEdgePrefab;
        public List<GameObject> bottomSpheres = new List<GameObject>();
        public List<GameObject> frontSpheres = new List<GameObject>();
        public List<Collider> RagdollParts = new List<Collider>();
        public List<Collider> CollidingParts = new List<Collider>();

        [SerializeField] public float gravityMultipliyer;
        [SerializeField] public float pullMultipliyer;


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
            bool SwitchBack = false;

            if (!IsFacingForward()) 
            {
                SwitchBack = true;
            }

            FaceForward(true);
            SetRagdollParts();
            SetColliderSpheres();

            if (SwitchBack)
            {
                FaceForward(false);
            }
        }

        private void OnTriggerEnter(Collider col) //callback function whenever somth touches or enters or otuches raggdoll body parts
        {
            if (RagdollParts.Contains(col))
            {
                return;
            }

            CharacterControl control = col.transform.root.GetComponent<CharacterControl>();

            if (control == null) 
            {
                return ;
            }

            if (col.gameObject == control.gameObject) //not a boxcolllider itself
            {
                return;
            }

            if (!CollidingParts.Contains(col))
            {
                CollidingParts.Add(col);
            }

        }

        private void OnTriggerExit(Collider col)
        {
            if (CollidingParts.Contains(col))
            {
                CollidingParts.Remove(col);
            }
        }


        private void SetRagdollParts()
        {
            Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>(); //Get all the colliders in the hierarchy

            foreach(Collider c in colliders)
            {
                if (c.gameObject != this.gameObject)  //if the collider that we found is not the same as in the charactercontrol
                {
                    c.isTrigger = true;
                    RagdollParts.Add(c);
                } 
            }
        }

        //private IEnumerator Start()
        //{
        //    yield return new WaitForSeconds(5f);
        //    Rigid_Body.AddForce(200f * Vector3.up);
        //    yield return new WaitForSeconds(0.5f);
        //    TurnOnRagdoll();
        //}

        private void TurnOnRagdoll()
        {
            Rigid_Body.useGravity = false;
            Rigid_Body.velocity = Vector3.zero;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            SkinnedMeshAnimator.enabled = false;
            SkinnedMeshAnimator.avatar = null;

            foreach (Collider c in RagdollParts)
            {
                c.isTrigger = false;
                c.attachedRigidbody.velocity = Vector3.zero;
            }
        }

        private void FixedUpdate()
        {
            if (Rigid_Body.velocity.y < 0f)
            {
                Rigid_Body.velocity += (-Vector3.up * gravityMultipliyer);
            }

            if (Rigid_Body.velocity.y > 0f && !Jump)
            {
                Rigid_Body.velocity += (-Vector3.up * pullMultipliyer);
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
        private GameObject CreateEdgeSphere (Vector3 pos)
        {
            GameObject obj = Instantiate(ColliderEdgePrefab,pos,Quaternion.identity);
            return obj;
        }

        private void SetCamera()
        {
            //  _Playercamera.transform.position = transform.position + _cameraOffset;
        }

        private void LateUpdate()
        {
            SetCamera();
        }

        public void MoveForward(float speed, float speedGraph)
        {
            transform.Translate(Vector3.forward * speed * speedGraph * Time.deltaTime);
        }

        public void FaceForward (bool forward)
        {
            if (forward)
            {
                transform.rotation = Quaternion.Euler(0f,0f,0f);
                
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

    }
}