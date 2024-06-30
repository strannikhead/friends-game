using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// review(29.06.2024):  Это больше похоже на какой-то TimeCounter
public class Calculator : MonoBehaviour
{
    [SerializeField]
    private GameObject button;
    public float elapsedTime;
    private LevelInfo levelInfo;
    public float timeTreshold;
    public float countedTime;
    public float timeBonus;
    private float fillingSpeed = 9;
    private bool isScored = false;
    // Start is called before the first frame update
    void Start()
    {
        button.SetActive(false);
        countedTime = 0;
        elapsedTime = TimeSystem.ElapsedTime;
        levelInfo = Levels.LevelsDict[MapModel.PlayerPosition.sceneName];
        timeTreshold = levelInfo.TimeTreshold;
        timeBonus = levelInfo.MaxTimeBonus;
    }

    void FixedUpdate()
    {
        if (countedTime < elapsedTime)
        {
            var delta = Time.deltaTime * fillingSpeed;
            countedTime += delta;
            //(29.05.2024) refactored // review(26.05.2024): Это все равно потенциально не спасает от отрицательного бонуса. Давай вместо этой проверки сделаем 
            // review(26.05.2024): timeBonus = Math.Max(0, timeBonus - DELTA);
            timeBonus = Math.Max(0, timeBonus - delta * levelInfo.TimeRatio);
        }
        else if (!isScored)
        {
            Game.score += (int)timeBonus;
            TimeSystem.Reset();
            isScored = true;
            button.SetActive(true);
        }
    }
}
