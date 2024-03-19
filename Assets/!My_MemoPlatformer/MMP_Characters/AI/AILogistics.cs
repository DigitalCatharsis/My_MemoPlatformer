using UnityEngine;

namespace My_MemoPlatformer
{
    public class AILogistics : MonoBehaviour
    {
        private CharacterControl _control;

        private void Start()
        {
            _control = GetComponentInParent<CharacterControl>();
        }

        //public float TargetDistanceToEndSphere()
        //{
        //    return Vector3.SqrMagnitude(_control.AICONTROLLER_DATA.pathfindingAgent.endSphere.transform.position - _control.AICONTROLLER_DATA.pathfindingAgent.target.transform.position);
        //}
        public float GetStartSphereABSHeight()
        {
            var result = Mathf.Abs((_control.transform.position - _control.AICONTROLLER_DATA.pathfindingAgent.startSphere.transform.position).y);

            return result;
        }

        //public float GetEndSphereHeight()
        //{
        //    var result = Mathf.Abs((_control.transform.position - _control.AICONTROLLER_DATA.pathfindingAgent.endSphere.transform.position).y);

        //    return result;
        //}

        public float AIDistanceToTarget()
        {
            var dist = Vector3.SqrMagnitude(_control.AICONTROLLER_DATA.pathfindingAgent.target.transform.position - _control.transform.position);
            return dist;
        }

        public float AIDistanceToStartSphere()
        {
            var dist = Vector3.SqrMagnitude(_control.AICONTROLLER_DATA.pathfindingAgent.startSphere.transform.position - _control.transform.position);
            return dist;    
        }

        public float AIDistanceToEndSphere()
        {
            var dist = Vector3.SqrMagnitude(_control.AICONTROLLER_DATA.pathfindingAgent.endSphere.transform.position - _control.transform.position);
            return dist;
        }
    }
}