using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public NormalProjectileBehavior ProjectilePrefab;
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
            ProjectilePrefab.type = 0;
            Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
        }
    }
}
