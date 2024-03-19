using System.Collections;
using System.Linq;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private int _maxEnemyNumber;
        [SerializeField] private float _timeBetweenSpawn;


        [Header("SpawnPosition offset from player")]
        [SerializeField] private float zRangeMax;
        [SerializeField] private float zRangeMin;
        [SerializeField] private float yRangeMax;
        [SerializeField] private float yRangeMin;
        [SerializeField] private float colliderRadius;
        [SerializeField] private Transform startPoint;

        [ShowOnly][SerializeField] private int _currentEnemiesNumber;

        private void Start()
        {
            StartCoroutine(SpawnEnemy_Routine());
        }

        private void FixedUpdate()
        {
        }

        private int CountAliveEnemiesOnScene()
        {
            var enemies = FindObjectsOfType<CharacterControl>();
            var temp = CharacterManager.Instance.GetPlayableCharacter();
            enemies = enemies.Where(val => val != CharacterManager.Instance.GetPlayableCharacter().gameObject).ToArray();

            int enemiesNumber = 0;

            foreach (var character in enemies)
            {

                if (character != CharacterManager.Instance.GetPlayableCharacter() & !character.DAMAGE_DATA.IsDead())
                {
                    enemiesNumber++;
                }
            }

            return enemiesNumber;
        }

        private IEnumerator SpawnEnemy_Routine()
        {
            yield return new WaitForEndOfFrame();
            _currentEnemiesNumber = CountAliveEnemiesOnScene();
            while (true)
            {
                if (_currentEnemiesNumber < _maxEnemyNumber)
                {
                    _currentEnemiesNumber++;

                    var playableCharacterPosition = CharacterManager.Instance.GetPlayableCharacter().transform.position;
                    PoolManager.Instance.GetObject(
                            CharacterType.__B_YBot_Blue,
                            PoolManager.Instance.characterPoolDictionary,
                            position: FindFreeSpaceForSpawn(
                                    startPoint.transform.position,
                                    startPoint.transform.position.z + zRangeMax,
                                    startPoint.transform.position.z + zRangeMin,
                                    startPoint.transform.position.y + yRangeMax,
                                    startPoint.transform.position.y + yRangeMin,
                                    colliderRadius),
                            Quaternion.identity);
                    yield return new WaitForSeconds(_timeBetweenSpawn);
                    _currentEnemiesNumber = CountAliveEnemiesOnScene();
                }
                else
                {
                    yield return new WaitForSeconds(_timeBetweenSpawn);
                    _currentEnemiesNumber = CountAliveEnemiesOnScene();
                }
            }
        }

        private Vector3 FindFreeSpaceForSpawn(Vector3 startPoint, float zRangeMax, float zRangeMin, float yRangeMax, float yRangeMin, float colliderRadius)
        {
            Vector3 resultPosition = new Vector3();
            var isOverlapping = true;
            while (isOverlapping)
            {
                var zPos = UnityEngine.Random.Range(zRangeMin, zRangeMax);
                var yPos = UnityEngine.Random.Range(yRangeMin, yRangeMax);

                resultPosition = new Vector3(0f, startPoint.y + yPos, startPoint.z + zPos);

                isOverlapping = Physics.CheckSphere(resultPosition, colliderRadius);
            }
            return resultPosition;
        }
    }
}
