//using System.Collections;
//using System.Collections.Generic;
// upper two unnecesary with lifetime and boundarys in scene
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Sprite[] sprites;
    private Rigidbody2D _rigidbody;
    public float speed = 100f;
    private float maxlifetime = 10.0f;
    private SpriteRenderer _spriteRenderer;
    private string[] bullets = { "Default", "Multi", "Spread", "Missile" };
    public int type;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer.sprite = sprites[type];
    }

    public void Project(Vector2 Direction)
    {
        _rigidbody.AddForce(Direction * speed);

        Destroy(this.gameObject, this.maxlifetime);
    }
    // needs rigidbody to gen for this automatic OnCollisionEnter function
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Hit Something");
        Destroy(this.gameObject);
    }
}
