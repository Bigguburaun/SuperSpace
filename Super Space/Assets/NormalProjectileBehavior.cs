using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalProjectileBehavior : MonoBehaviour
{
    public float damage = 1;
    public float type; //Type so player/enemy bullets don't hit their owners
    public float size;
    public float speed;
    public float decayTime;
    void Start()
    {
        Destroy(gameObject, decayTime);
        gameObject.transform.localScale = new Vector3(size, size);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.OnHit(damage);
        }
        Destroy(gameObject);
    }

}
