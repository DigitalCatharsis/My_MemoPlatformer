using UnityEngine;
namespace My_MemoPlatformer
{
    public class Ledge : MonoBehaviour
    {
        public Vector3 offset;

        public static bool IsLedgeCollider(GameObject obj)
        {
            if(obj.GetComponent<LedgeCollider>() == null)
            {
                return false;
            }
            return true;
        }        
        public static bool IsCharacter(GameObject obj)
        {
            if(obj.transform.root.GetComponent<CharacterControl >() == null)
            {
                return false;
            }
            return true;
        }
    }
}