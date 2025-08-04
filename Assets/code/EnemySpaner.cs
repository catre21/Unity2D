using UnityEngine;
using System.Collections;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemies;
    [SerializeField] private Transform[] SpawnPoints;
    [SerializeField] private float timBetWeenSpawns = 1.5f;
    void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());
    }
    private IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timBetWeenSpawns);
            GameObject enemy = Enemies[Random.Range(0, Enemies.Length)];
            Transform spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
            Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        }
    }
 
}
