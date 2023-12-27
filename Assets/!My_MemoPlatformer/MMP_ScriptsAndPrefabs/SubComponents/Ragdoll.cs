using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class Ragdoll : SubComponent
    {
        public Ragdoll_Data ragdoll_Data;

        private void Start()
        {
            ragdoll_Data = new Ragdoll_Data
            {
                ragdollTriggered = false,
                bodyParts = new List<Collider>(),
                GetBodypart = GetBodyPart,
                AddForceToDamagedPart = AddForceToDamagedPart,
            };

            SetupBodyParts();
            subComponentProcessor.ragdollData = ragdoll_Data;
            subComponentProcessor.subcomponentsDictionary.Add(SubComponentType.RAGDOLL, this);
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
            ragdoll_Data.bodyParts.Clear();

            var colliders = control.gameObject.GetComponentsInChildren<Collider>(); //Get all the colliders in the hierarchy

            foreach (Collider c in colliders)
            {
                if (c.gameObject != control.gameObject)  //if the collider that we found is not the same as in the charactercontrol (//not a boxcolllider itself)
                {
                    if (c.gameObject.GetComponent<LedgeChecker>() == null && c.gameObject.GetComponent<LedgeCollider>() == null)
                    {
                        //thats means its a ragdoll
                        c.isTrigger = true;
                        ragdoll_Data.bodyParts.Add(c);
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
        }

        private void ProcRagdoll()
        {
            ragdoll_Data.ragdollTriggered = false;
            
            if (control.skinnedMeshAnimator.avatar == null)
            {
                return;
            }

            //change components layers from character to DeadBody to prevent unnessesary collisions.
            Transform[] arr = control.gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform t in arr)
            {
                t.gameObject.layer = LayerMask.NameToLayer(MMP_Layers.DeadBody.ToString());
            }

            //save bodypart positions to prevent teleporting
            foreach (Collider c in ragdoll_Data.bodyParts)
            {
                TriggerDetector det = c.GetComponent<TriggerDetector>();
                det.lastPosition = c.gameObject.transform.position;
                det.lastRotation = c.gameObject.transform.rotation;
            }

            //turn off animator, avatar
            control.Rigid_Body.useGravity = false;
            control.Rigid_Body.velocity = Vector3.zero;
            control.gameObject.GetComponent<BoxCollider>().enabled = false;
            control.skinnedMeshAnimator.enabled = false;
            control.skinnedMeshAnimator.avatar = null;

            //Turn off ledge colliders
            control.LedgeGrab_Data.LedgeCollidersOff();

            //turn off ai
            if (control.aiController != null)
            {
                control.aiController.gameObject.SetActive(false);
                control.navMeshObstacle.carving = false; //we dont need carving when enemy is dead
            }

            //turn on ragdoll
            foreach (Collider c in ragdoll_Data.bodyParts)
            {
                c.isTrigger = false;

                TriggerDetector det = c.GetComponent<TriggerDetector>();
                c.attachedRigidbody.MovePosition(det.lastPosition);  //https://docs.unity3d.com/ScriptReference/Rigidbody.MovePosition.html
                c.attachedRigidbody.MoveRotation(det.lastRotation);  //https://docs.unity3d.com/ScriptReference/Rigidbody.MoveRotation.html

                c.attachedRigidbody.velocity = Vector3.zero;
            }

            AddForceToDamagedPart(false);
        }

        private Collider GetBodyPart(string name)
        {
            foreach (Collider c in ragdoll_Data.bodyParts)
            {
                if (c.name.Contains(name))
                {
                    return c;
                }
            }
            return null;
        }
        private void AddForceToDamagedPart(bool zeroZelocity)
        {
            if (control.DamageDetector_Data.damagedTrigger != null)
            {
                if (zeroZelocity)
                {
                    foreach (var c in ragdoll_Data.bodyParts)
                    {
                        c.attachedRigidbody.velocity = Vector3.zero;
                    }
                }

                control.DamageDetector_Data.damagedTrigger.GetComponent<Rigidbody>().
                     AddForce(control.DamageDetector_Data.attacker.transform.forward * control.DamageDetector_Data.attack.forwardForce +
                          control.DamageDetector_Data.attacker.transform.right * control.DamageDetector_Data.attack.rightForce +
                             control.DamageDetector_Data.attacker.transform.up * control.DamageDetector_Data.attack.upForce);
            }
        }
    }
}
