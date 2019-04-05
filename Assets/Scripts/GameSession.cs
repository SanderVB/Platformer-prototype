using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;

    int score = 0;
    int lives = 3;

    private void Awake()
    {
        int gameSessionCount = FindObjectsOfType<GameSession>().Length;
        if (gameSessionCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        UIUpdate();
    }

    private void UIUpdate()
    {
        scoreText.text = score.ToString("00000");
        livesText.text = lives.ToString();
    }

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UIUpdate();
    }

    public int GetLives()
    {
        return lives;
    }

    public void AddToLives(int livesToAdd)
    {
        lives += livesToAdd;
        UIUpdate();
    }

    public void PlayerDied()
    {
        AddToLives(-1);
        if (lives > 0)
        {
            FindObjectOfType<LevelLoader>().RestartLevel();
        }
        else
        {
            FindObjectOfType<MusicPlayer>().ResultSound(false);
            FindObjectOfType<LevelLoader>().GameOver();
        }
    }
}
