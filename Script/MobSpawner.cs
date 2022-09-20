using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{

    public GameObject[] enemy = new GameObject[2];

    int EnemyNum;
    int SpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        EnemyNum = Random.Range(0,2);
        GameObject RandomSpawn = Instantiate(enemy[EnemyNum]);
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            SpawnTime = Random.Range(5, 15);
            yield return new WaitForSeconds(SpawnTime);
            EnemyNum = Random.Range(0, 2);
            GameObject RandomSpawn = Instantiate(enemy[EnemyNum]);
        }
    }
}
