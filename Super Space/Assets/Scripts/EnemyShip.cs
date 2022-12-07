using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Bullet bulletPrefab;
    public Sprite[] sprites;

    private float sinCenterX;
    private float movementSpeed = 1f;
    private float amplitude = 3f;
    private float frequency = 5f;
    private float maxLifetime = 30f;
    private int health = 3;
    public int type;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        this.type = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[this.type];
        sinCenterX = transform.position.x;
        // Destroy the asteroid after it reaches its max lifetime
        Destroy(gameObject, maxLifetime);
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        float sin = Mathf.Sin(pos.y * frequency) * amplitude;
        pos.x = sinCenterX + sin;
        pos.y -= movementSpeed * Time.fixedDeltaTime;
        transform.position = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enemy Hit");

        if (collision.gameObject.CompareTag("Bullet"))
        {
            enemyHit();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemy Trigger Hit");

        if (collision.gameObject.CompareTag("Bullet"))
        {
            enemyHit();
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.gameObject.tag = "EnemyBullet";
        bullet.Project(-transform.up);
    }

    private void enemyHit()
    {
        Debug.Log("Enemy Hit");
        health--;
        Debug.Log(health);
        if(health <= 0)
        {
            FindObjectOfType<GameManager>().EnemyDied(this);
            Destroy(gameObject);
        }
    }
}
