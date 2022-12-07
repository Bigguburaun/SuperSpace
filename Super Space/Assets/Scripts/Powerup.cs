using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer _spriteRenderer;
    private float maxLifeTime = 30f;
    public string powerType = "Default";

    public void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        int power = Random.Range(0, sprites.Length);
        _spriteRenderer.sprite = sprites[power];
        switch (power)
        {
            case 3: 
                powerType = "Missle"; 
                break; 
            case 2: 
                powerType = "Spread"; 
                break;
            case 1: 
                powerType = "Multi" ; 
                break;
            default: 
                powerType = "Default"; 
                break;
        }
        Destroy(gameObject, maxLifeTime);

    }

}
