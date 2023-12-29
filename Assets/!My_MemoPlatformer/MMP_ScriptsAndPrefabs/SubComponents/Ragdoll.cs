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
            var colliders = control.gameObject.GetComponentsInChildren<Collider>(); //Get all the colliders in the hierarchy

            foreach (var c in colliders)
            {
                if (c.gameObject != control.gameObject)  //if the collider that we found is not the same as in the charactercontrol (//not a boxcolllider itself)
                {
                    if (c.gameObject.GetComponent<LedgeChecker>() == null && c.gameObject.GetComponent<LedgeCollider>() == null)
                    {
                        //thats means its a ragdoll
                        c.isTrigger = true;
                        bodyParts.Add(c);
                        c.attachedRigidbody.interpolation = RigidbodyInterpolation.None;  //убрать дрожжание //Окей, если каждая часть будет интерполированной, то начинается вакханалия
                        c.attachedRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; //расчет физики, предотвращение прохождения сквозь объекты

                        CharacterJoint joint = c.GetComponent<CharacterJoint>();
                        if (joint != null)
                        {
                            joint.enableProjection = true; //https://docs.unity3d.com/Manual/RagdollStability.html
                        }

                        if (c.GetComponent<TriggerDetector>() == null)
                        {
                            c.gameObject.AddComponent<TriggerDetector>();
                        }
                    }
                }
            }

            ragdoll_Data.arrBodyParts = new Collider[bodyParts.Count];

            for (int i = 0; i < ragdoll_Data.arrBodyParts.Length; i++)
            {
                ragdoll_Data.arrBodyParts[i] = bodyParts[i];
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
            var arr = control.gameObject.GetComponentsInChildren<Transform>();
            foreach (var t in arr)
            {
                t.gameObject.layer = LayerMask.NameToLayer(MMP_Layers.DeadBody.ToString());
            }

            //save bodypart positions to prevent teleporting
            for (int i = 0; i < ragdoll_Data.arrBodyParts.Length; i++)
            {
                var det = ragdoll_Data.arrBodyParts[i].GetComponent<TriggerDetector>();
                det.lastPosition = ragdoll_Data.arrBodyParts[i].gameObject.transform.position;
                det.lastRotation = ragdoll_Data.arrBodyParts[i].gameObject.transform.rotation;
            }

            //turn off animator, avatar
            control.RIGID_BODY.useGravity = false;
            control.RIGID_BODY.velocity = Vector3.zero;
            control.gameObject.GetComponent<BoxCollider>().enabled = false;
            control.skinnedMeshAnimator.enabled = false;
            control.skinnedMeshAnimator.avatar = null;

            //Turn off ledge colliders
            control.LEDGE_GRAB_DATA.LedgeCollidersOff();

            //turn off ai
            if (control.aiController != null)
            {
                control.aiController.gameObject.SetActive(false);
                control.navMeshObstacle.enabled = false;
            }

            //turn on ragdoll
            for (int i = 0; i < ragdoll_Data.arrBodyParts.Length; i++)
            {
                ragdoll_Data.arrBodyParts[i].isTrigger = false;
                ragdoll_Data.arrBodyParts[i].attachedRigidbody.isKinematic = true;
            }

            for (int i = 0; i < ragdoll_Data.arrBodyParts.Length; i++)
            {
                TriggerDetector det = ragdoll_Data.arrBodyParts[i].GetComponent<TriggerDetector>();
                ragdoll_Data.arrBodyParts[i].attachedRigidbody.MovePosition(det.lastPosition);
                ragdoll_Data.arrBodyParts[i].attachedRigidbody.MoveRotation(det.lastRotation);
                ragdoll_Data.arrBodyParts[i].attachedRigidbody.velocity = Vector3.zero;
            }

            for (int i = 0; i < ragdoll_Data.arrBodyParts.Length; i++)
            {
                ragdoll_Data.arrBodyParts[i].attachedRigidbody.isKinematic = false;
            }

            ragdoll_Data.ClearExistingVelocity();

            if (control.DAMAGE_DATA.damageTaken != null)
            {
                //take damage from ragdoll
                var incomingVelocity = control.DAMAGE_DATA.damageTaken.INCOMING_VELOCITY;
                var damagedPart = control.DAMAGE_DATA.damageTaken.DAMAGE_TG;

                if (Vector3.SqrMagnitude(incomingVelocity) > 0.0001f)
                {
                    if (DebugContainer.Instance.debug_Ragdoll)
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

        Collider GetBodyPart(string name)
        {
            for (int i = 0; i < ragdoll_Data.arrBodyParts.Length; i++)
            {
                if (ragdoll_Data.arrBodyParts[i].name.Contains(name))
                {
                    return ragdoll_Data.arrBodyParts[i];
                }
            }

            return null;
        }
        void AddForceToDamagedPart(RagdollPushType pushType)
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

            var body = control.DAMAGE_DATA.damageTaken.DAMAGE_TG.GetComponent<Rigidbody>();
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
            for (int i = 0; i < ragdoll_Data.arrBodyParts.Length; i++)
            {
                ragdoll_Data.arrBodyParts[i].attachedRigidbody.velocity = Vector3.zero;
            }
        }
    }
}
