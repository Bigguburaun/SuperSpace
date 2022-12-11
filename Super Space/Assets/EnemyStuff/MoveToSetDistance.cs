using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToSetDistance : MonoBehaviour
{
    Transform target;
    Rigidbody2D rb;
    public float speed;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindWithTag("Player") != null)
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
            else if (Vector2.Distance(target.position, transform.position) < distance - 1)
            {
                rb.velocity = transform.up * -(speed / 2);
            }
        }
    }
}
