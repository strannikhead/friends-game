using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public RectTransform scoreBoardContainer;
    public GameObject scoreRecordTemplate;
    private const float Spacing = 60f;
    

    void Start() => DisplayScores();

    void Update()
    {
        DisplayScores();
    }

    private void DisplayScores()
    {
        foreach (Transform child in scoreBoardContainer)
            Destroy(child.gameObject);
        
        var scoreManager = FindObjectOfType<ScoreManager>();
        var highScores = scoreManager.GetHighScores();
        var recordCount = highScores.Count;
        
        for (var i = 0; i < 5; i++)
        {
            var score = i < recordCount ? highScores[i] : new ScoreEntry($"unknown", (5 - i) * 100);
            var scoreEntry = Instantiate(scoreRecordTemplate, scoreBoardContainer);
            scoreEntry.SetActive(true);
            var scoreText = scoreEntry.GetComponentInChildren<TextMeshProUGUI>();

            // review(26.05.2024): Помимо просто лога стоит просто прекращать цикл, либо continue
            if (scoreText == null)
            {
                Debug.Log("Кнопки не найдено");
                break;
            }
            scoreText.text = $"{i + 1}. {score.playerName}: {score.score}";
        
            var scoreEntryRectTransform = scoreEntry.GetComponent<RectTransform>();
            scoreEntryRectTransform.anchoredPosition = new Vector2(0, -i * Spacing);
        }
    }
}