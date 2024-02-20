using UnityEngine;

namespace My_MemoPlatformer
{
    public class AILogistic : MonoBehaviour
    {
        private AIController _controller;
        private void Start()
        {
            _controller = GetComponent<AIController>();
        }

        public float TargetDistanceToEndSphere()
        {
            return Vector3.SqrMagnitude(_controller.pathfindingAgent.endSphere.transform.position - _controller.pathfindingAgent.target.transform.position);
        }
        public float GetStartSphereHeight()
        {
            var result = Mathf.Abs((_controller.Control.transform.position - _controller.pathfindingAgent.startSphere.transform.position).y);

            return result;
        }

        public float GetEndSphereHeight()
        {
            var result = Mathf.Abs((_controller.Control.transform.position - _controller.pathfindingAgent.endSphere.transform.position).y);

            return result;
        }

        public float AIDistanceToTarget()
        {
            var dist = Vector3.SqrMagnitude(_controller.pathfindingAgent.target.transform.position - _controller.Control.transform.position);
            return dist;
        }

        public float AIDistanceToStartSphere()
        {
            var dist = Vector3.SqrMagnitude(_controller.pathfindingAgent.startSphere.transform.position - _controller.Control.transform.position);
            return dist;    
        }

        public float AIDistanceToEndSphere()
        {
            var dist = Vector3.SqrMagnitude(_controller.pathfindingAgent.endSphere.transform.position - _controller.Control.transform.position);
            return dist;
        }
    }
}