using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace My_MemoPlatformer
{
    public enum RagdollPushType
    {
        NORMAL,
        DEAD_BODY,
    }

    public class Ragdoll : SubComponent
    {
        public Ragdoll_Data ragdoll_Data;

        public override void OnComponentEnabled()
        {
            ragdoll_Data = new Ragdoll_Data
            {
                ragdollTriggered = false,
                flyingRagdollData = new FlyingRagdollData(),

                GetBodypart = GetBodyPart,
                AddForceToDamagedPart = AddForceToDamagedPart,
                ClearExistingVelocity = ClearExistingVelocity,
            };

            SetupBodyParts();
            subComponentProcessor.ragdoll_Data = ragdoll_Data;
        }

        public override void OnFixedUpdate()
        {
            if (ragdoll_Data.ragdollTriggered)
            {
                ProcRagdoll();
            }
        }

        public override void OnUpdate()
        {
        }

        public void SetupBodyParts()
        {
            var bodyParts = new List<Collider>();
            var colliders = control.gameObject.GetComponentsInChildren<Collider>();

            foreach (var collider in colliders)
            {
                if (collider != control.boxCollider)  //not a boxCollider
                {
                    if (collider.gameObject.GetComponent<LedgeChecker>() == null 
                        && collider.gameObject.GetComponent<LedgeCollider>() == null
                        && collider.gameObject.name != "UpBlockChecker")
                    {
                        //thats means its a ragdoll
                        collider.isTrigger = true;
                        bodyParts.Add(collider);
                        collider.attachedRigidbody.interpolation = RigidbodyInterpolation.None;  //убрать дрожжание //Окей, если каждая часть будет интерполированной, то начинается вакханалия
                        collider.attachedRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; //расчет физики, предотвращение прохождения сквозь объекты
                        collider.attachedRigidbody.useGravity = false;

                        CharacterJoint joint = collider.GetComponent<CharacterJoint>();
                        if (joint != null)
                        {
                            joint.enableProjection = true; //https://docs.unity3d.com/Manual/RagdollStability.html
                        }

                        if (collider.GetComponent<TriggerDetector>() == null)
                        {
                            collider.gameObject.AddComponent<TriggerDetector>();
                        }
                    }
                }
            }

            ragdoll_Data.arrBodyPartsColliders = new Collider[bodyParts.Count];

            for (int i = 0; i < ragdoll_Data.arrBodyPartsColliders.Length; i++)
            {
                ragdoll_Data.arrBodyPartsColliders[i] = bodyParts[i];
            }
        }

        private void ProcRagdoll()
        {
            ragdoll_Data.ragdollTriggered = false;

            if (control.skinnedMeshAnimator.avatar == null)
            {
                return;
            }

            //change components layers from character to DeadBody to prevent unnessesary collisions.
            var bodypartsTransforms_Array = control.gameObject.GetComponentsInChildren<Transform>();
            foreach (var transform in bodypartsTransforms_Array)
            {
                transform.gameObject.layer = LayerMask.NameToLayer(MMP_Layers.DeadBody.ToString());
            }

            //save bodypart positions to prevent teleporting
            for (int i = 0; i < ragdoll_Data.arrBodyPartsColliders.Length; i++)
            {
                var triggerDetector = ragdoll_Data.arrBodyPartsColliders[i].GetComponent<TriggerDetector>();
                triggerDetector.lastPosition = ragdoll_Data.arrBodyPartsColliders[i].gameObject.transform.position;
                triggerDetector.lastRotation = ragdoll_Data.arrBodyPartsColliders[i].gameObject.transform.rotation;
            }

            //turn off animator, avatar
            control.rigidBody.useGravity = false;
            control.rigidBody.velocity = Vector3.zero;
            control.gameObject.GetComponent<BoxCollider>().enabled = false;
            control.skinnedMeshAnimator.enabled = false;
            control.skinnedMeshAnimator.avatar = null;

            //Turn off ledge colliders
            control.LEDGE_GRAB_DATA.DisableLedgeColliders();

            //turn off ai
            if (control.AICONTROLLER_DATA.aiType == AI_Type.Bot)
            {
                control.AICONTROLLER_DATA.aiType = AI_Type.None;
                control.navMeshObstacle.enabled = false;
            }

            //turn on ragdoll
            for (int i = 0; i < ragdoll_Data.arrBodyPartsColliders.Length; i++)
            {
                ragdoll_Data.arrBodyPartsColliders[i].isTrigger = false;
                ragdoll_Data.arrBodyPartsColliders[i].attachedRigidbody.isKinematic = true;
            }

            for (int i = 0; i < ragdoll_Data.arrBodyPartsColliders.Length; i++)
            {
                var triggerDetector = ragdoll_Data.arrBodyPartsColliders[i].GetComponent<TriggerDetector>();
                ragdoll_Data.arrBodyPartsColliders[i].attachedRigidbody.MovePosition(triggerDetector.lastPosition);
                ragdoll_Data.arrBodyPartsColliders[i].attachedRigidbody.MoveRotation(triggerDetector.lastRotation);
                ragdoll_Data.arrBodyPartsColliders[i].attachedRigidbody.isKinematic = false;
                ragdoll_Data.arrBodyPartsColliders[i].attachedRigidbody.velocity = Vector3.zero;
                ragdoll_Data.arrBodyPartsColliders[i].attachedRigidbody.useGravity = true;   /////////////?????????????????????????WORTH IT?
            }

            for (int i = 0; i < ragdoll_Data.arrBodyPartsColliders.Length; i++)
            {
                ragdoll_Data.arrBodyPartsColliders[i].attachedRigidbody.isKinematic = false;
            }

            ragdoll_Data.ClearExistingVelocity();

            if (control.DAMAGE_DATA.damageTaken != null)
            {
                //take damage from ragdoll
                var incomingVelocity = control.DAMAGE_DATA.damageTaken.INCOMING_VELOCITY;
                var damagedPart = control.DAMAGE_DATA.damageTaken.DAMAGED_TG;

                if (Vector3.SqrMagnitude(incomingVelocity) > 0.0001f)
                {
                    if (DebugContainer_Data.Instance.debug_Ragdoll)
                    {
                        Debug.Log(control.gameObject.name + ": taking damage from ragdoll");
                    }
                    damagedPart.rigidBody.AddForce(incomingVelocity * 0.7f);
                }

                //take damage from attack
                else
                {
                    ragdoll_Data.AddForceToDamagedPart(RagdollPushType.NORMAL);
                }
            }
        }

        private Collider GetBodyPart(string name)
        {
            for (int i = 0; i < ragdoll_Data.arrBodyPartsColliders.Length; i++)
            {
                if (ragdoll_Data.arrBodyPartsColliders[i].name.Contains(name))
                {
                    return ragdoll_Data.arrBodyPartsColliders[i];
                }
            }

            return null;
        }
        private void AddForceToDamagedPart(RagdollPushType pushType)
        {
            if (control.DAMAGE_DATA.damageTaken == null)
            {
                return;
            }

            if (control.DAMAGE_DATA.damageTaken.ATTACKER == null)
            {
                return;
            }

            var damageData = control.DAMAGE_DATA;

            var forwardDir = damageData.damageTaken.ATTACKER.transform.forward;
            var rightDir = damageData.damageTaken.ATTACKER.transform.right;
            var upDir = damageData.damageTaken.ATTACKER.transform.up;

            var body = control.DAMAGE_DATA.damageTaken.DAMAGED_TG.GetComponent<Rigidbody>();
            var attack = damageData.damageTaken.ATTACK;

            if (pushType == RagdollPushType.NORMAL)
            {
                body.AddForce(
                    forwardDir * attack.normalRagdollVelocity.forwardForce +
                    rightDir * attack.normalRagdollVelocity.rightForce +
                    upDir * attack.normalRagdollVelocity.upForce);
            }
            else if (pushType == RagdollPushType.DEAD_BODY)
            {
                body.AddForce(
                    forwardDir * attack.collateralRagdollVelocity.forwardForce +
                    rightDir * attack.collateralRagdollVelocity.rightForce +
                    upDir * attack.collateralRagdollVelocity.upForce);
            }
        }

        void ClearExistingVelocity()
        {
            for (int i = 0; i < ragdoll_Data.arrBodyPartsColliders.Length; i++)
            {
                ragdoll_Data.arrBodyPartsColliders[i].attachedRigidbody.velocity = Vector3.zero;
            }
        }
    }
}
