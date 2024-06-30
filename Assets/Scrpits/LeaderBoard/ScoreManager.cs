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
            // review(29.06.2024): Дублирует ResetScores()
            scoreBoard = new ScoreBoard();
            SaveScores();
        }
    }

    public void AddScore(string playerName, int score)
    {
        scoreBoard.highScores = scoreBoard.highScores // review(29.06.2024): Изменять (и создавать) публичные поля - опасно и вносит путаницу. Стоило в ScoreBoard добавить метод типа Add(ScoreEntry entry)
            .Append(new ScoreEntry(playerName, score))
            .OrderByDescending(x => x.score)
            .Take(5) // review(29.06.2024): магическая константа. Что-то мне подсказывает, что она как-то связана с View (LeaderBoard)
            .ToList();

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