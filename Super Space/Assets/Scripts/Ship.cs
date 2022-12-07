using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public Bullet bulletPrefab;
    private float bulletSpeed = 200f;
    private int bulletType = 1;
    public int currentPickups = 0;
    private bool shoot = false;
    private bool pause = false;
    private Vector2 moveDirection;
    private float moveSpeed = 4.0f;
    private bool speedUp;
    private float speedUpMultiplier = 2.0f;
    private float invulTime = 3f;
    public int health = 10;

    public void Awake()
    {
        bulletPrefab.type = bulletType;
        bulletPrefab.speed = bulletSpeed;
        rigidbody = GetComponent<Rigidbody2D>();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check movement and poistion of ship
        Vector2 pos = transform.position;
        moveDirection.x = Input.GetAxisRaw("HorizontalMove");
        moveDirection.y = Input.GetAxisRaw("VerticalMove");
        speedUp = Input.GetButton("SpeedUp"); // Add Speedup Axis in input manager if needed
        // remember to adjust axises in scene edit project settings

        pos += Time.deltaTime * moveSpeed * (speedUp ? speedUpMultiplier : 1) * moveDirection.normalized;
        
        // moves player at camera borders, kinematic doesn't allow for boundary stopping
        if (pos.x > 2.2f)
            pos.x = 2.2f;
        else if (pos.x < -2.2f)
            pos.x = -2.2f;

        if (pos.y > 4.5f)
            pos.y = 4.5f;
        else if (pos.y < -4.5f)
            pos.y = -4.5f;
        transform.position = pos;

        shoot = Input.GetKeyDown(KeyCode.Space);
        if (shoot)
        {
            Shoot();
        }
        pause = Input.GetKeyDown(KeyCode.L);
        if(pause)
        {
            Pause();
        }
    }
    private void Pause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            FindObjectOfType<GameManager>().PlayerPaused();
        }
        else
        {
            Time.timeScale = 0f;
            FindObjectOfType<GameManager>().PlayerUnpaused();
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(transform.up);
    }

    // Player ship has kinematic allowing it collect be hit/be hit but not move from collision
    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Powerup"))
        {
            string powName = collider.gameObject.GetComponent<Powerup>().powerType;
            applyEffect(powName);
            Destroy(collider.gameObject);
        }
        // Potentially just make enemy bullets also tagged as just "Enemy
        else if(collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("Player Hit");
            playerHit();

        }
    }

    private void TurnOnPlayerCollision()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    // Determine player health and respawn
    private void playerHit()
    {
        Debug.Log(health);
        this.health--;
        Debug.Log(health);

        if (this.health <= 0)
        {
            FindObjectOfType<GameManager>().SetHealth(this.health);
            this.gameObject.SetActive(false);
            // very slow, figure out ways to refernece global manager faster
            FindObjectOfType<GameManager>().PlayerDied();
        }
        else
        {
            FindObjectOfType<GameManager>().SetHealth(this.health);
            gameObject.layer = LayerMask.NameToLayer("Invincible");
            Invoke(nameof(TurnOnPlayerCollision), invulTime);
        }
    }

    private void resetPlayer()
    {
        health = 10;
        currentPickups = 0;
        //this.transform.position = {0, -4.5, 0};
    }

    private void applyEffect(string powName)
    {
        currentPickups++;
        switch (powName)
        {
            case "Missile":
                bulletPrefab.speed = 200f;
                bulletPrefab.type = 3;
                break;
            case "Spread":
                bulletPrefab.speed = 100f;
                bulletPrefab.type = 2; break;
            case "Multi":
                bulletPrefab.speed = 150f;
                bulletPrefab.type = 1; break;
            default:
                bulletPrefab.speed = 200f;
                bulletPrefab.type = 0; break;
        }
        FindObjectOfType<GameManager>().SetPower(powName);
    }
}
