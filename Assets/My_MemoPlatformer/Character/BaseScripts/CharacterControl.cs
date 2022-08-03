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
        Grounded
    }


    public class CharacterControl : MonoBehaviour
    {
        // [SerializeField] private Camera _Playercamera;
        //private Vector3 _cameraOffset = new Vector3(0, 2.0f, -12.0f);

        [SerializeField] Animator _animator;
        public bool MoveRight;
        public bool MoveLeft;
        public bool Jump;
        [SerializeField] private GameObject ColliderEdgePrefab;
        public List<GameObject> bottomSpheres = new List<GameObject>();
        public List<GameObject> frontSpheres = new List<GameObject>();

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

            if (Rigid_Body.velocity.y > 0f && !Jump)
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

    }
}