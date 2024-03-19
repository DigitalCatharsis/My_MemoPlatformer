using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class Interaction : SubComponent
    {
        public Interaction_Data interaction_Data;
        public override void OnComponentEnabled()
        {
            interaction_Data = new Interaction_Data
            {
                GetTouchingWeapon = GetTouchingWeapon,
                SpawnPointHitParticle = SpawnPointHitParticle,
                SpawnParticle = SpawnParticle,
            };

            subComponentProcessor.interaction_Data = interaction_Data;
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {

        }
        public MeleeWeapon GetTouchingWeapon()
        {
            foreach (KeyValuePair<TriggerDetector, List<Collider>> data in control.DAMAGE_DATA.collidingWeapons_Dictionary)
            {
                var w = data.Value[0].gameObject.GetComponent<MeleeWeapon>();
                return w;
            }

            return null;
        }

        public void SpawnPointHitParticle(CharacterControl attacker, VFXType effectsType)
        {
            var vfx = PoolManager.Instance.GetObject
                (
                    effectsType,
                    PoolManager.Instance.vfxPoolDictionary,
                    position: control.DAMAGE_DATA.damageTaken.DAMAGED_TG.triggerCollider.bounds.center,
                    Quaternion.identity
                );

            vfx.SetActive(true);

            if (attacker.ROTATION_DATA.IsFacingForward())
            {
                vfx.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                vfx.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

        //aoe or for parents

        public void SpawnParticle(CharacterControl control, VFXType objectType, string parentObjectName, bool stickObjectToParent, float delayBeforeStart)
        {
            StartCoroutine(SpawnParticle_Routine(control, objectType, parentObjectName, stickObjectToParent, delayBeforeStart));
        }

        public IEnumerator SpawnParticle_Routine(CharacterControl control, VFXType objectType, string parentObjectName, bool stickObjectToParent, float delayBeforeStart)
        {
            yield return new WaitForSeconds(delayBeforeStart);
            var _spawnedVFX = PoolManager.Instance.GetObject(objectType, PoolManager.Instance.vfxPoolDictionary, Vector3.zero, Quaternion.identity);

            if (!string.IsNullOrEmpty(parentObjectName))
            {
                var p = control.GetChildObj(parentObjectName);
                _spawnedVFX.transform.parent = p.transform;
                _spawnedVFX.transform.localPosition = Vector3.zero;
                _spawnedVFX.transform.localRotation = Quaternion.identity;
            }

            if (!stickObjectToParent)
            {
                _spawnedVFX.transform.parent = null;
            }

            _spawnedVFX.SetActive(true);
            yield break;
        }
    }
}