using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        ScoreEntry newScore = new ScoreEntry(playerName, score); // review(26.05.2024): var

        // review(26.05.2024): Через LINQ проще :)
        // scoreBoard.highScores = scoreBoard.highScores
        //     .Append(new ScoreEntry(playerName, score))
        //     .OrderByDescending(x => x.score)
        //     .Take(5)
        //     .ToList();

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

    public void ResetRecords()
    {
        scoreBoard = new ScoreBoard();
        SaveScores();
    }
}

// review(26.05.2024): Стоит вынести все классы в отдельные файлы
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