using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    // Update is called once per frame
    void LateUpdate()
    {
        if(target != null)
            transform.position = target.position + offset;
    }
}
