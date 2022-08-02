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

            float bottom = box.bounds.center.y - box.bounds.extents.y; // � ������ �����. 
            float top = box.bounds.center.y + box.bounds.extents.y; // � ������ ������. ;
            float front = box.bounds.center.z + box.bounds.extents.z; // � ������ �������. ;
            float back = box.bounds.center.z - box.bounds.extents.z; // � ������ �����. ;;

            GameObject bottomFront = CreateEdgeSphere(new Vector3(0f, bottom, front));
            GameObject bottomBack = CreateEdgeSphere(new Vector3(0f, bottom, back));

            bottomFront.transform.parent = this.transform; //������ ��� ��������
            bottomBack.transform.parent = this.transform;

            bottomSpheres.Add(bottomFront);
            bottomSpheres.Add(bottomBack);

            float sec = (bottomFront.transform.position - bottomBack.transform.position).magnitude / 5f; //�������� ���� ������ ������, �������� �� ����

            for (int i=0; i < 4; i++)
            {
                Vector3 pos = bottomBack.transform.position + (Vector3.forward) * sec * (i + 1);  //�������� ������

                GameObject newObj =  CreateEdgeSphere(pos); //������� � ������ ������ �����
                newObj.transform.parent = this.transform; //������ ��� ��������
                bottomSpheres.Add(newObj);  //��������� � ������
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