using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [Range(-1f,1f)]
    public float scrollSpeed = 0.01f;
    private float offsetX;
    private float offsetY;
    private Material mat;
    public Rigidbody2D Player;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            offsetX += (Time.deltaTime * Player.velocity.x) / 200f;
            offsetY += (Time.deltaTime * Player.velocity.y) / 200f;
            mat.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
        }
    }
}
