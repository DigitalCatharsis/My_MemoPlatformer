using My_MemoPlatformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class DeathAnimationManager : Singleton<DeathAnimationManager>
    {

        private DeathAnimationLoader _deathAnimationLoader;
        private List<RuntimeAnimatorController> _Candidates = new List<RuntimeAnimatorController>();


        void SetupDeathAnimationLoader()
        {
            if (_deathAnimationLoader == null)
            {
                GameObject obj = Instantiate(Resources.Load("DeathAnimationLoader", typeof(GameObject)) as GameObject);
                DeathAnimationLoader loader = obj.GetComponent<DeathAnimationLoader>();
                _deathAnimationLoader = loader;
            }
        }

        public RuntimeAnimatorController GetAnimator(GeneralBodyPart generalBodyPart, AttackInfo info)
        {
            SetupDeathAnimationLoader();

            _Candidates.Clear();

            foreach (DeathAnimationData data in _deathAnimationLoader.DeathAnimationDataList)
            {
                if (info.deathType == data.deathType)
                {
                    if (info.deathType != DeathType.NONE)
                    {
                        _Candidates.Add(data.Animator);
                    }
                    else if (!info.mustCollide)
                    {
                        _Candidates.Add(data.Animator);
                    }
                    else
                    {
                        foreach (GeneralBodyPart part in data.GeneralBodyParts)
                        {
                            if (part == generalBodyPart)
                            {
                                _Candidates.Add(data.Animator);
                                break;
                            }
                        }
                    }
                }                
            }
            return _Candidates[Random.Range(0, _Candidates.Count)];
        }
    }
}