using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 movement;
    public float rotation = 0.0f;
    public float speed = 10.0f;
    private Vector2 inputDirection;
    public Vector2 test;

    public ProjectileBehavior ProjectilePrefab;
    public Transform LaunchOffset;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    public float horizontalInput;
    public float verticalInput;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = Vector2.SmoothDamp(rb.velocity, Vector2.zero, ref test, 0.3f);

            if (Mathf.Abs(rb.angularVelocity) > 0)
            {
                rb.angularDrag = 10;
            }

        }
        else
        {
            rb.angularDrag = 0;
        }
            //movement = new Vector2(0, Input.GetAxis("Vertical"));

            //inputDirection = movement.normalized;

            //horizontalInput = Input.GetAxis("Horizontal");
            //verticalInput = Input.GetAxis("Vertical");

            //transform.Translate(Vector2.up * Time.deltaTime * 2 * verticalInput);
            //transform.Rotate(0, 0, 1 * Time.deltaTime * 80 * horizontalInput);
        

        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
        }

    }

    void FixedUpdate()
    {
        //if (!Input.GetKey(KeyCode.Space))
        //{
            if (rb.angularVelocity > 100)
            {
                rb.angularVelocity = 100;
            }
            if (rb.angularVelocity < -100)
            {
                rb.angularVelocity = -100;
            }
            //rb.rotation -= Input.GetAxis("Horizontal") * 10;
            //rb.angularVelocity = -Input.GetAxis("Horizontal") * 50;
            rb.AddTorque(-Input.GetAxis("Horizontal") / 10, ForceMode2D.Impulse);

            rotation = Mathf.Deg2Rad * ((rb.rotation + 90) % 360);
            movement = new Vector2(Mathf.Cos(rotation) * Input.GetAxis("Vertical"), Mathf.Sin(rotation) * Input.GetAxis("Vertical"));

            rb.AddForce(movement * speed);
            //rb.MovePosition((Vector3)rb.position + transform.up * speed * inputDirection.y * Time.deltaTime);
        //}

    }



}



