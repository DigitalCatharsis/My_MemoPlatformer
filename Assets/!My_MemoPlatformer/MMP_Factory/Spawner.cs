using UnityEngine;

namespace My_MemoPlatformer
{
    public class Spawner : Singleton<Spawner>
    {
        public CharacterFactory CharacterFactory;
        public VFXFactory vFXFactory;
        public DataFactory dataFactory;

        private void Awake()
        {
            CharacterFactory = new CharacterFactory();
            vFXFactory = new VFXFactory();
            dataFactory = new DataFactory();
        }
    }
}


