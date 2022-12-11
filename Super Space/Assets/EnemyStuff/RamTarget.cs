using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamTarget : MonoBehaviour
{
    Transform target;
    Rigidbody2D rb;
    public float speed;
    public float distance;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindWithTag("Player") != null)
            target = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            var addAngle = 270;
            var dir = target.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + addAngle;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            if (Vector2.Distance(target.position, transform.position) > distance)
            {
                rb.velocity = transform.up * (speed + (Vector2.Distance(target.position, transform.position) / 5));
            }
            else if (Vector2.Distance(target.position, transform.position) < distance)
            {
                rb.velocity = transform.up * (speed / 2);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if (damageable != null && collider.gameObject.layer == 8)
        {
            damageable.OnHit(damage);
            Destroy(gameObject);
        }
    }
}
