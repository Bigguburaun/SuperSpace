using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private string[] levels;
    public Ship Player;
    public float spawnTimer = 3.0f;
    public int lives = 3;
    public int score = 0;
    public int currentWave = 1;
    public Text scoreText;
    public Text powerText;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {


    }
    // adjust any values relating to player
    public void SetPower(string power)
    {
        powerText.text = "Power:" + power;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
    // set player inactive and determine gameover
    public void PlayerDied()
    {
        RestartLevel();

    }
    // turn on in-game pause
    public void PlayerPaused()
    {

    }
    // set pause menu inactive
    public void PlayerUnpaused()
    {

    }

    public void EnemyDied(EnemyShip enemy)
    {
        //explosionEffect.transform.position = enemy.transform.position;
        //destroySoundEffect.play();
        //explosionEffect.Play();

        if (enemy.type > 2)
        {
            SetScore(score + 100);
        }
        else if (enemy.type > 1)
        {
            SetScore(score + 50); 
        }
        else
        {
            SetScore(score + 25);
        }
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = "Score:" + (score.ToString());
    }

    private void RestartLevel()
    {
        foreach (Bullet b in GameObject.FindObjectsOfType<Bullet>())
        {
            Destroy(b.gameObject);
        }
        foreach (Powerup p in GameObject.FindObjectsOfType<Powerup>())
        {
            Destroy(p.gameObject);
        }
        foreach (EnemyShip e in GameObject.FindObjectsOfType<EnemyShip>())
        {
            Destroy(e.gameObject);
        }
        score = 0;
        // SceneManager wouldn't work without the UnityEngine.SceneManagement prefix but should
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void NextLevel()
    {
        //string sceneName = levels[currentLevel];
        //SceneManager.LoadScene(sceneName);
    }

    private void GameOver()
    {

    }
}
