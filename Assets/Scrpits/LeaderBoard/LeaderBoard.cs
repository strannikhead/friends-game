using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public GameObject scoreEntryTemplate;
    public RectTransform scoreBoardContainer;
    public float spacing = 60f;

    void Start() => DisplayScores();

    public void DisplayScores()
    {
        // Очистить старые записи, если они есть
        foreach (Transform child in scoreBoardContainer)
            Destroy(child.gameObject);

        // Получить текущие рекорды из ScoreManager
        var scoreManager = FindObjectOfType<ScoreManager>();
        var highScores = scoreManager.GetHighScores();
        var recordCount = highScores.Count;
        // Создать новую строку для каждого рекорда
        for (var i = 0; i < 5; i++)
        {
            var score = i < recordCount ? highScores[i] : new ScoreEntry($"TestPlayer{i + 1}", (5 - i) * 100);
            var scoreEntry = Instantiate(scoreEntryTemplate, scoreBoardContainer);
            scoreEntry.SetActive(true);
            var scoreText = scoreEntry.GetComponent<Text>();
            scoreText.text = $"{i + 1}. {score.playerName}: {score.score}";

            // Расположить строку с учетом отступа
            RectTransform scoreEntryRectTransform = scoreEntry.GetComponent<RectTransform>();
            scoreEntryRectTransform.anchoredPosition = new Vector2(0, -i * spacing);
        }
    }
}