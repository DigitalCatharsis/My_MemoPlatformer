using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AIAttacks : MonoBehaviour
    {
        private int _attackIndex;
        private AIController _aiController;

        private void Start()
        {
            _aiController = GetComponent<AIController>();
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
            if (_aiController._aIConditions.TargetIsOnRightSide())
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
            if (_aiController._aIConditions.IsFacingTarget() && control.PLAYER_ANIMATION_DATA.IsRunning(typeof(MoveForward)))
            {
                control.ATTACK_DATA.attackTriggered = true;
                control.attack = false;
            }
        }
        private void SetRandomFlyingKick()
        {
            if (UnityEngine.Random.Range(0f, 1f) < _aiController.flyingKickProbability)
            {
                //TODO: ??????????????
                _aiController.aIController_Data.doFlyingKick = true;
            }
            else
            {
                _aiController.aIController_Data.doFlyingKick = false;
            }
        }

        public IEnumerator _RandomizeNextAttack()
        {
            while (true)
            {
                _attackIndex = UnityEngine.Random.Range(0, _aiController.aIController_Data.listGroundAttacks.Count);
                yield return new WaitForSeconds(2f);
            }
        }

        public void Attack()
        {
            _aiController.aIController_Data.listGroundAttacks[_attackIndex](_aiController.Control);
        }
    }
}