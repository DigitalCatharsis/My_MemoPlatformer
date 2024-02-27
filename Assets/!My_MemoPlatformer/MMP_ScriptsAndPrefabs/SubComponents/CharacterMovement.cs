using UnityEngine;

namespace My_MemoPlatformer
{
    public class CharacterMovement : SubComponent
    {
        [SerializeField] private CharacterMovement_Data characterMovement_Data;

        private void OnEnable()
        {
            characterMovement_Data = new CharacterMovement_Data
            {
                isIgnoreCharacterTime = false,
                latestMoveForwardScript = null,
                latestMoveUpScript = null,
                NullifyUpVelocity = NullifyUpVelocity,
                IsFacingAtacker = IsFacingAtacker,
                IsForwardReversed = IsForwardReversed,
                MoveCharacterForward = MoveCharacterForward,
            };

            subComponentProcessor.characterMovement_Data = characterMovement_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.CHARACTER_MOVEMENT] = this;
        }

        public override void OnFixedUpdate()
        {
        }

        public override void OnUpdate()
        {
        }
        public void NullifyUpVelocity()
        {
            Control.rigidBody.velocity = new Vector3(Control.rigidBody.velocity.x, 0f, Control.rigidBody.velocity.z);
        }
        public bool IsFacingAtacker()
        {
            var vec = Control.DAMAGE_DATA.damageTaken.ATTACKER.transform.position - Control.transform.position;

            if (vec.z < 0f)
            {
                if (Control.ROTATION_DATA.IsFacingForward())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (vec.z > 0f)
            {
                if (Control.ROTATION_DATA.IsFacingForward())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        public bool IsForwardReversed()
        {
            if (characterMovement_Data.latestMoveForwardScript.moveOnHit)  //Move back when punched
            {
                if (IsFacingAtacker())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (characterMovement_Data.latestMoveForwardScript.Speed > 0f)
            {
                return false;
            }
            else if (characterMovement_Data.latestMoveForwardScript.Speed < 0f)
            {
                return true;
            }

            return false;
        }
        public void MoveCharacterForward(float speed, float speedGraph)
        {
            Control.gameObject.transform.Translate(Vector3.forward * speed * speedGraph * Time.deltaTime);
        }
    }
}

