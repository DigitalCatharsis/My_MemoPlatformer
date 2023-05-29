using My_MemoPlatformer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;


namespace My_MemoPlatformer
{

   // [ExecuteInEditMode]

    public class LookAtPoint : MonoBehaviour
    {
        public GameObject objectToLook;
        public Vector3 lookAtPoint;
        public bool rotateToPoint;
        public Color traceColor;
        public float traceExistDuration = 0.1f;

        private void Awake()
        {
        }

        public void Update()
        {
            if (traceColor == null) 
            {
                traceColor = Color.magenta;
                traceColor.a = 1;
            }

            if (objectToLook != null)
            {
                lookAtPoint = objectToLook.transform.position;
            }

            if (rotateToPoint == true)
            {
                transform.LookAt(lookAtPoint);
            }
            //Debug.DrawLine(test.position, lookAtPoint, Color.red, 0.1f);
            Debug.DrawLine(transform.position, lookAtPoint, traceColor, traceExistDuration);
        }
    }
}