using UnityEngine;

namespace My_MemoPlatformer
{
    public class CollisionDetection : MonoBehaviour
    {
        public static GameObject GetCollidingObject(CharacterControl control, GameObject start, Vector3 dir, float blockDistance, ref Vector3 collisionPoint)
        {
            collisionPoint = Vector3.zero;

            //draw DebugLine
            Debug.DrawRay(start.transform.position, dir * blockDistance, Color.yellow);

            //check collision
            RaycastHit hit;
            if (Physics.Raycast(start.transform.position, dir, out hit, blockDistance))
            {
                if (!IsOwnBodyPart(control, hit.collider)
                    && !IsIgnoringCharacter(control, hit.collider)
                    && !Ledge.IsLedgeChecker(hit.collider.gameObject)  // Проверка, что мы ничего не задеваем, включая Ledge (платформы, за котоыре можно зацепиться)
                    && !MeleeWeapon.IsWeapon(hit.collider.gameObject)
                    && !TrapSpikes.IsTrap(hit.collider.gameObject))
                {
                    collisionPoint = hit.point;
                    return hit.collider.transform.gameObject;
                }
                else  
                {
                    return null;
                }
            }
            else  //collide nothing
            {
                return null;
            }
        }
        public static bool IsIgnoringCharacter(CharacterControl control, Collider col)
        {
            if (!control.animationProgress.isIgnoreCharacterTime)
            {
                return false;
            }
            else
            {
                var blockingCharacter = CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);

                if (blockingCharacter == null)
                {
                    return false;
                }

                if (blockingCharacter == control)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static bool IsOwnBodyPart(CharacterControl control, Collider col)
        {
            if (col.transform.root.gameObject == control.gameObject)
            {
                return true;
            }


            var target = CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);

            if (target == null)
            {
                return false;
            }

            if (target.DAMAGE_DATA.IsDead())
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
