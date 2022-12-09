using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    //Player Controls
    [SerializeField] private float flTurnSpeed = 0f;
    [SerializeField] private float flSpeed = 0f;
    [SerializeField] private float flMaxVelocity = 0f;

    private Vector2 movement;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        float yAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");

        ForwardMovement(yAxis);
        Rotate(transform, xAxis * -flTurnSpeed);
        ClampVelocity();

    }

    private void ClampVelocity()
    {
        float x = Mathf.Clamp(rb.velocity.x, -flMaxVelocity, flMaxVelocity);
        float y = Mathf.Clamp(rb.velocity.y, -flMaxVelocity, flMaxVelocity);

        rb.velocity = new Vector2(x, y);
    }

    private void ForwardMovement(float amount)
    {
        Vector2 force = transform.up * amount * flSpeed;

        rb.AddForce(force);
    }

    private void Rotate(Transform t, float amount)
    {
        t.Rotate(0, 0, amount * Time.deltaTime);
    }
}
