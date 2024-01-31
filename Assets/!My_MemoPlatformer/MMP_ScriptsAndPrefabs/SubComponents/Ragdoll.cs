using System.Collections.Generic;
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

        private void Start()
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
            subComponentProcessor.arrSubComponents[(int)SubComponentType.RAGDOLL] = this;
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
            throw new System.NotImplementedException();
        }

        public void SetupBodyParts()
        {
            var bodyParts = new List<Collider>();
            var colliders = Control.gameObject.GetComponentsInChildren<Collider>();

            foreach (var collider in colliders)
            {
                if (collider != Control.boxCollider)  //not a boxCollider
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

            if (Control.skinnedMeshAnimator.avatar == null)
            {
                return;
            }

            //change components layers from character to DeadBody to prevent unnessesary collisions.
            var bodypartsTransforms_Array = Control.gameObject.GetComponentsInChildren<Transform>();
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
            Control.RIGID_BODY.useGravity = false;
            Control.RIGID_BODY.velocity = Vector3.zero;
            Control.gameObject.GetComponent<BoxCollider>().enabled = false;
            Control.skinnedMeshAnimator.enabled = false;
            Control.skinnedMeshAnimator.avatar = null;

            //Turn off ledge colliders
            Control.LEDGE_GRAB_DATA.DisableLedgeColliders();

            //turn off ai
            if (Control.aiController != null)
            {
                Control.aiController.gameObject.SetActive(false);
                Control.navMeshObstacle.enabled = false;
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

            if (Control.DAMAGE_DATA.damageTaken != null)
            {
                //take damage from ragdoll
                var incomingVelocity = Control.DAMAGE_DATA.damageTaken.INCOMING_VELOCITY;
                var damagedPart = Control.DAMAGE_DATA.damageTaken.DAMAGED_TG;

                if (Vector3.SqrMagnitude(incomingVelocity) > 0.0001f)
                {
                    if (DebugContainer_Data.Instance.debug_Ragdoll)
                    {
                        Debug.Log(Control.gameObject.name + ": taking damage from ragdoll");
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
            if (Control.DAMAGE_DATA.damageTaken == null)
            {
                return;
            }

            if (Control.DAMAGE_DATA.damageTaken.ATTACKER == null)
            {
                return;
            }

            var damageData = Control.DAMAGE_DATA;

            var forwardDir = damageData.damageTaken.ATTACKER.transform.forward;
            var rightDir = damageData.damageTaken.ATTACKER.transform.right;
            var upDir = damageData.damageTaken.ATTACKER.transform.up;

            var body = Control.DAMAGE_DATA.damageTaken.DAMAGED_TG.GetComponent<Rigidbody>();
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
