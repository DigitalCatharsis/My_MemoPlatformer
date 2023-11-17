using UnityEngine;

namespace My_MemoPlatformer
{
    public class MeleeWeapon : MonoBehaviour
    {
        public CharacterControl control;
        public Vector3 customPosition = new Vector3();
        public Vector3 customRotation = new Vector3();

        public static bool IsWeapon(GameObject obj)
        {
            if (obj.transform.root.gameObject.GetComponent<MeleeWeapon>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DropWeapon()
        {
            var weapon = control.animationProgress.HoldingWeapon;

            if (weapon != null)
            {
                weapon.transform.parent = null;

                if (control.IsFacingForward())
                {
                    weapon.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                }
                else
                {
                    weapon.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                }

                weapon.transform.position = control.transform.position + Vector3.up * 0.015f;

                control.animationProgress.HoldingWeapon = null;
                control = null;
            }
        }
    }
}
