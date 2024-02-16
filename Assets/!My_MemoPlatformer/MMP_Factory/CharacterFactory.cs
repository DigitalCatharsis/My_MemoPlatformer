using UnityEngine;

namespace My_MemoPlatformer
{
    //TODO: HashManager
    public enum CharacterType
    {
        __B_YBot_Blue,
        __R_YBot_Red,
        __G_YBot_Green,
        __Y_YBot_Yellow,
    }
    public class CharacterFactory : MonoBehaviour, ICoreFactory<CharacterType> 
    {

        [SerializeField] private GameObject yBotBluePrefab;
        [SerializeField] private GameObject yBotRedPrefab;
        [SerializeField] private GameObject yBotGreenPrefab;
        [SerializeField] private GameObject yBotYellowPrefab;

        public CharacterFactory()
        {
            yBotBluePrefab = Resources.Load(CharacterType.__B_YBot_Blue.ToString()) as GameObject;
            yBotRedPrefab = Resources.Load(CharacterType.__R_YBot_Red.ToString()) as GameObject;
            yBotGreenPrefab = Resources.Load(CharacterType.__G_YBot_Green.ToString()) as GameObject;
            yBotYellowPrefab = Resources.Load(CharacterType.__Y_YBot_Yellow.ToString()) as GameObject;
        }

        public GameObject SpawnGameobject(CharacterType characterType, Vector3 position, Quaternion rotation)
        {
            switch (characterType)
            {
                case CharacterType.__B_YBot_Blue:
                    {
                        return Instantiate(yBotBluePrefab, position, rotation);
                    }
                case CharacterType.__Y_YBot_Yellow:
                    {
                        return Instantiate(yBotYellowPrefab, position, rotation);
                    }
                case CharacterType.__G_YBot_Green:
                    {
                        return Instantiate(yBotGreenPrefab, position, rotation);
                    }
                case CharacterType.__R_YBot_Red:
                    {
                        return Instantiate(yBotRedPrefab, position, rotation);
                    }
                default:  //Интересно, как заткнуть эту дыру грамотно....
                    {
                        return null;
                    }
            }
        }
    }

}
