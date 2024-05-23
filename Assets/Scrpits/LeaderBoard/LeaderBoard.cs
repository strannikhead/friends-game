using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
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
            var score = i < recordCount ? highScores[i] : new ScoreEntry($"Строки в json'e нет", (5 - i) * 100);
            var scoreEntry = Instantiate(scoreRecordTemplate, scoreBoardContainer);
            scoreEntry.SetActive(true);
            var scoreText = scoreEntry.GetComponentInChildren<TextMeshProUGUI>();
            
            if (scoreText == null)
                Debug.Log("Кнопки не найдено");
            scoreText.text = $"{i + 1}. {score.playerName}: {score.score}";
        
            RectTransform scoreEntryRectTransform = scoreEntry.GetComponent<RectTransform>();
            scoreEntryRectTransform.anchoredPosition = new Vector2(0, -i * spacing);
        }
    }
}