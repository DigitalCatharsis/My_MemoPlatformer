using System.Collections;
using System.Drawing;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AIAttacks : MonoBehaviour
    {
        private CharacterControl _control;

        private void Start()
        {
            _control = GetComponentInParent<CharacterControl>();
        }

        public void NormalGroundAttack(CharacterControl control)
        {
            control.moveRight = false;
            control.moveLeft = false;

            control.ATTACK_DATA.attackTriggered = true;
            control.attack = false;
        }

        public void ForwardGroundAttack(CharacterControl control)
        {
            if (_control.AICONTROLLER_DATA.aIConditions.TargetIsOnRightSide())
            {
                control.moveRight = true;
                control.moveLeft = false;

                PrococessForwardGroundAttack(control);
            }
            else
            {
                control.moveRight = false;
                control.moveLeft = true;

                PrococessForwardGroundAttack(control);
            }
        }
        private void PrococessForwardGroundAttack(CharacterControl control)
        {
            if (_control.AICONTROLLER_DATA.aIConditions.IsFacingTarget() && control.PLAYER_ANIMATION_DATA.IsRunning(typeof(MoveForward)))
            {
                control.ATTACK_DATA.attackTriggered = true;
                control.attack = false;
            }
        }
        public bool ProceedFlyingKick(CharacterControl control)
        {
            if (control.AICONTROLLER_DATA.doFlyingKick && control.AICONTROLLER_DATA.aIConditions.TargetIsOnTheSamePlatform())
            {
                ColorDebugLog.Log("FlyingKick", KnownColor.Aqua);
                control.attack = true;
                return true;
            }
            else
            {
                control.attack = false;
                return false;
            }
        }
        public void SetRandomFlyingKick()
        {
            if (Random.Range(0f, 1f) < _control.AICONTROLLER_DATA.flyingKickProbability)
            {
                //TODO: ??????????????
                _control.AICONTROLLER_DATA.doFlyingKick = true;
            }
            else
            {
                _control.AICONTROLLER_DATA.doFlyingKick = false;
            }
        }
    }
}