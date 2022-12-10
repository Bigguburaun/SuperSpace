using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalProjectileBehavior : MonoBehaviour
{
    public float damage = 1;
    public float type; //Type so player/enemy bullets don't hit their owners
    void Start()
    {
        Destroy(gameObject, 3);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * Time.deltaTime * 30;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if ((type == 0 && collision.gameObject.tag != "Player") || (type == 1 && collision.gameObject.tag != "Enemy"))
        {
            if (damageable != null)
            {
                damageable.OnHit(damage);
            }
            Destroy(gameObject);
        }
    }

}
