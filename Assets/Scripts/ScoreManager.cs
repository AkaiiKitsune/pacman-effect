﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



public class ScoreManager : MonoBehaviour
{
    [Header("Level Reference")]
    [SerializeField] private LevelParser level;
    [SerializeField] public PowerUp power;

    [SerializeField] private int score;
    [SerializeField] private int highScore;
    [SerializeField] private UIManager UIManager;


    public void InitScore ()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        score = 0;
        UIManager.TextScore.text = UIManager.AddPoint(score);
        UIManager.TextHighScore.text = UIManager.AddPoint(highScore);
        UIManager.SetProgressionMax();
    }

    public void KeepScore ()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        score = PlayerPrefs.GetInt("KeepScore");
        UIManager.TextScore.text = UIManager.AddPoint(score);
        UIManager.TextHighScore.text = UIManager.AddPoint(highScore);
        UIManager.SetProgressionMax();
    }

    public void AddScore (TileType type, Transform PacMan)
    {
        
        if (type == TileType.Ball)
        {
            AudioManager.PlaySound("wakka");
            score += 10;
            UIManager.TextScore.text = UIManager.AddPoint(score);
            UIManager.GetProgression();
        }
        else if (type == TileType.Super)
        {
            AudioManager.PlaySound("wakka");
            score += 50;
            UIManager.TextScore.text = UIManager.AddPoint(score);
            UIManager.GetProgression();
            power.SuperPacman();
        }
        else if (type == TileType.Fruit)
        {
            AudioManager.PlaySound("wakka");
            score += 100;
            UIManager.TextScore.text = UIManager.AddPoint(score);
            UIManager.AddMultiPoint();
            UIManager.GetProgression();
        }
        if (score > highScore)
        {
            highScore = score;
            UIManager.TextHighScore.text = UIManager.AddPoint(highScore);
            SaveScore();
        }
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    public void SaveScoreWin()
    {
        PlayerPrefs.SetInt("KeepScore", score);
    }

    public int ShowScore () => score;
    public int ShowHighScore() => highScore;
    
   
}
