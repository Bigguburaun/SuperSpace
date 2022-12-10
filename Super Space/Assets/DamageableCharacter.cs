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
    //public GameObject healthText;

    public void OnHit(float damage)
    {
        Health -= damage;
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

            if(_health <= 0)
            {
                if (gameObject.tag != "player")
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
}
