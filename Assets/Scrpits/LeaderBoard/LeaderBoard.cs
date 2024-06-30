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
        
        var scoreManager = FindObjectOfType<ScoreManager>(); // review(29.06.2024): Правда ли, что scoreManager-а нужно искать на каждый update?
        var highScores = scoreManager.GetHighScores();
        var recordCount = highScores.Count;
        
        for (var i = 0; i < 5; i++) // review(29.06.2024): Почему 5? Выглядит как магическое число. Наверное, стоит выделить в поле
        {
            var score = i < recordCount ? highScores[i] : new ScoreEntry($"unknown",0);
            // review(29.06.2024): Как будто не хватает отдельного объекта, который бы инкапсулировал в себе логику инициализации
            var scoreEntry = Instantiate(scoreRecordTemplate, scoreBoardContainer); // review(29.06.2024): scoreObject или типа того. Просто в проекте уже есть ScoreEntry, и данное название может запутать
            scoreEntry.SetActive(true);
            var scoreText = scoreEntry.GetComponentInChildren<TextMeshProUGUI>();

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