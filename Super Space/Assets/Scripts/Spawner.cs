using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public EnemyShip enemyPrefab;
    private float spawnRate = 2.0f;
    private float spawnNum = 1f;
    private float spawnerX = 2.5f;
    private float spawnerY = 7.65f;
    
    // Start is called before the first frame update
    private void Start()
    {
        spawnerX = transform.position.x;
        spawnerY = transform.position.y;
        InvokeRepeating(nameof(enemySpawn), 0, Random.Range(5.0f, 5.0f*spawnRate));
    }

    private void enemySpawn()
    {
        for(int i = 0; i < spawnNum; i++)
        {

            Vector3 spawnPoint = Vector3.zero;
            spawnPoint.x = Random.Range(-spawnerX, spawnerX);
            spawnPoint.y = spawnerY;
            EnemyShip enemy = Instantiate(this.enemyPrefab, spawnPoint, Quaternion.identity);
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
