using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public GameObject Enemy;
    Camera cam;

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
                instance = new GameManager();
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        cam = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Vector3 worldPoint = Input.mousePosition;
            worldPoint.z = Mathf.Abs(cam.transform.position.z);
            Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(worldPoint);
            mouseWorldPosition.z = 0f;
            Instantiate(Enemy, mouseWorldPosition, Quaternion.identity);

        }
    }
}
