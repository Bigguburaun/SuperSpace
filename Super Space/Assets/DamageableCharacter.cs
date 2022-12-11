using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableCharacter : MonoBehaviour, IDamageable
{
    public float _health = 3;
    //bool isAlive = true;
    Collider2D physicsCollider;
    Rigidbody2D rb;
    Animator animator;
    private float invincibleTimeElapsed = 0f;
    public bool _invincible;
    public float invincibiltyTime = 0f;
    //public GameObject healthText;

    public void OnHit(float damage)
    {
        if (!Invincible)
        {
            Health -= damage;
            if (gameObject.tag != "player")
                Invincible = true;
        }
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        //Figure out knockback later
        Health -= damage;
    }

    public float Health
    {
        set
        {
            /*
            if (value < _health)
            {
                animator.SetTrigger("hit");

                RectTransform textTransform = Instantiate(healthText).GetComponent<RectTransform>();
                textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                Canvas canvas = GameObject.FindObjectOfType<Canvas>();
                textTransform.SetParent(canvas.transform);
            }
            */
            _health = value;

            if (_health <= 0)
            {
                if (gameObject.tag != "Player")
                    OnObjectDestroyed();
                //animator.SetBool("isAlive", false);
            }
        }
        get
        {
            return _health;
        }
    }

    public void OnObjectDestroyed()
    {
        Destroy(gameObject);
    }

    public bool Invincible
    {
        get { return _invincible; }
        set
        {
            _invincible = value;

            if (_invincible == true)
                invincibleTimeElapsed = 0f;
        }
    }
    

    void FixedUpdate()
    {
        if (Invincible)
        {
            invincibleTimeElapsed += Time.deltaTime;

            if(invincibleTimeElapsed > invincibiltyTime)
            {
                Invincible = false;
            }
        }
    }
}
