using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatFire : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform LaunchOffset;
    public float fireRate;
    public float size;
    public float speed;
    public float damage;
    private float timeElapsed = 0f;
    public float decayTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= fireRate)
        {
            GameObject projectile = Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
            projectile.layer = 7;
            var behavior = projectile.GetComponent<NormalProjectileBehavior>();
            behavior.type = 1;
            behavior.size = size;
            behavior.speed = speed;
            behavior.damage = damage;
            behavior.decayTime = decayTime;
            timeElapsed = 0;
        }
    }
}
