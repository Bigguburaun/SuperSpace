using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    private float spawnRate = 2f;
    private float spawnNum = 1f;
    private float spawnerX = 0f;
    private float spawnerY = 0;

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating(nameof(enemySpawn), 0, Random.Range(5.0f, 5.0f * spawnRate));
    }

    private void enemySpawn()
    {
        for (int i = 0; i < spawnNum; i++)
        {
            spawnerX = Random.Range(-0.1f, 1.1f);
            spawnerY = Random.Range(0, 2);
            if (Random.Range(0, 2) == 0)
                (spawnerX, spawnerY) = (spawnerY, spawnerX);

            Vector3 spawnPoint = Vector3.zero;
            spawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(spawnerX, spawnerY, 1f));
            GameObject enemy = Instantiate(this.enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoint, Quaternion.identity);
            //yield on a new YieldInstruction that waits for 3 seconds.
            //yield return new WaitForSeconds(0.5f);
            wait();
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(0.5f);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
