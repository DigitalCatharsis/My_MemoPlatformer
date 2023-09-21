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
    }
}