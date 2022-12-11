using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateScore : MonoBehaviour
{
    public TMP_Text scoreText;

    public void AddScore(int score)
    {
        scoreText.text = (int.Parse(scoreText.text) + score).ToString();
    }
}
