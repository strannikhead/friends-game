using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private string filePath;
    private ScoreBoard scoreBoard;

    void Start()
    {
        filePath = Application.persistentDataPath + "/highscores.json";
        LoadScores();
    }

    void LoadScores()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            scoreBoard = JsonUtility.FromJson<ScoreBoard>(json);
        }
        else
        {
            scoreBoard = new ScoreBoard();
            SaveScores();
        }
    }

    public void AddScore(string playerName, int score)
    {
        ScoreEntry newScore = new ScoreEntry(playerName, score);
        scoreBoard.highScores.Add(newScore);

        scoreBoard.highScores.Sort((x, y) => -y.score.CompareTo(x.score));

        if (scoreBoard.highScores.Count > 5)
            scoreBoard.highScores = scoreBoard.highScores.GetRange(0, 5);

        SaveScores();
    }

    void SaveScores()
    {
        var json = JsonUtility.ToJson(scoreBoard, true);
        File.WriteAllText(filePath, json);
    }

    public List<ScoreEntry> GetHighScores()
    {
        return scoreBoard.highScores;
    }

    public void Reset()
    {
        scoreBoard = new ScoreBoard();
        SaveScores();
    }
}

[Serializable]
public class ScoreEntry
{
    public string playerName;
    public int score;

    public ScoreEntry(string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }
}

[Serializable]
public class ScoreBoard
{
    public List<ScoreEntry> highScores;

    public ScoreBoard()
    {
        highScores = new List<ScoreEntry>();
    }
}