using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score instance;
    public Text s;
    public int score = 0;
    public board b { get; private set; }
    public static int highscore;
    public Text hi;

    public void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", highscore);
    }
    public void UpdateScore()
    {
        score += 100;
        s.text = "" + score;
        
    }

    public void ResetScore()
    {
        score = 0;
        s.text = "" + score;
    }
    public void Update()
    {
        if (score > highscore)
        {
            highscore = score;
            hi.text = "" + score;
            PlayerPrefs.SetInt("highscore", highscore);
        }
 
    }

}