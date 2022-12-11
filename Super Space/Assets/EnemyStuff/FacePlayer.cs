using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    Transform target;
    Rigidbody2D rb;

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
        }
    }
}
