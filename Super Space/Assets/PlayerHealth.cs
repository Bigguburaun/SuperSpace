using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public Image healthBar;
    public GameObject deathMenu;
    public IDamageable player;
    // Start is called before the first frame update
    void Start()
    {
        player = this.GetComponent<IDamageable>();
        player.Health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(player.Health / maxHealth, 0, 1);
        if (player.Health <= 0)
        {
            player.OnObjectDestroyed();
            deathMenu.SetActive(true);
        }
    }
}
