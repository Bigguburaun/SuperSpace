using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSpawner : MonoBehaviour
{
    public Powerup powerupPrefab;
    private float spawnRate = 2.0f;
    private float[] spawnPosition = { 0f, 7.65f };
    private float spawnerX = 2.5f;
    private float spawnerY = 7.65f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(powerSpawn), 0, Random.Range(5.0f, 5.0f * spawnRate));
        spawnerX = transform.position.x;
        spawnerY = transform.position.y;

    }

    void powerSpawn()
    {
        //Debug.Log("here");
        Vector3 spawnPoint = Vector3.zero;
        spawnPoint.x = Random.Range(-spawnerX, spawnerX);
        spawnPoint.y = spawnerY;
        Powerup powerup = Instantiate(this.powerupPrefab, spawnPoint, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
