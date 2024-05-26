using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    // review(26.05.2024): Все поля действительно должны быть публичными?
    public GameObject scoreEntryTemplate;
    public RectTransform scoreBoardContainer;
    public GameObject scoreRecordTemplate;
    public float spacing = 60f;
    

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

        // review(26.05.2024): Стоит удалять ненужные строки кода
        // версия на тексте
        // for (var i = 0; i < 5; i++)
        // {
        //     var score = i < recordCount ? highScores[i] : new ScoreEntry($"Строки в json'e нет", (5 - i) * 100);
        //     var scoreEntry = Instantiate(scoreEntryTemplate, scoreBoardContainer);
        //     scoreEntry.SetActive(true);
        //     var scoreText = scoreEntry.GetComponent<Text>();
        //     scoreText.text = $"{i + 1}. {score.playerName}: {score.score}";
        //
        //     RectTransform scoreEntryRectTransform = scoreEntry.GetComponent<RectTransform>();
        //     scoreEntryRectTransform.anchoredPosition = new Vector2(0, -i * spacing);
        // }
        
        // версия на кнопке
        for (var i = 0; i < 5; i++)
        {
            // review(26.05.2024): Лучше вместо честного ответа про json писать Unknown/Неизвестный
            var score = i < recordCount ? highScores[i] : new ScoreEntry($"Строки в json'e нет", (5 - i) * 100);
            var scoreEntry = Instantiate(scoreRecordTemplate, scoreBoardContainer);
            scoreEntry.SetActive(true);
            var scoreText = scoreEntry.GetComponentInChildren<TextMeshProUGUI>();

            // review(26.05.2024): Помимо просто лога стоит просто прекращать цикл, либо continue
            if (scoreText == null)
                Debug.Log("Кнопки не найдено");
            scoreText.text = $"{i + 1}. {score.playerName}: {score.score}";
        
            RectTransform scoreEntryRectTransform = scoreEntry.GetComponent<RectTransform>(); // review(26.05.2024): var
            scoreEntryRectTransform.anchoredPosition = new Vector2(0, -i * spacing);
        }
    }
}