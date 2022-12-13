using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform LaunchOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject projectile = Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
            projectile.layer = 6;
            var behavior = projectile.GetComponent<NormalProjectileBehavior>();
            behavior.type = 0;
            //behavior.size = 10;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.LoadLevel("MainMenu");
        }
    }
}
