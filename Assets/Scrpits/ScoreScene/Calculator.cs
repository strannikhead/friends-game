using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        levelInfo = Levels.levels
            [MapModel.playerPos.sceneName]; // review(26.05.2024): Давай вернем индекс на место
        timeTreshold = levelInfo.TimeTreshold;
        timeBonus = levelInfo.MaxTimeBonus;
    }

    void FixedUpdate()
    {
        if (countedTime < elapsedTime)
        {
            var delta = Time.deltaTime * fillingSpeed;
            countedTime += delta;
            // review(26.05.2024): Это все равно потенциально не спасает от отрицательного бонуса. Давай вместо этой проверки сделаем 
            // review(26.05.2024): timeBonus = Math.Max(0, timeBonus - DELTA);
            timeBonus = timeBonus > 0 ? timeBonus - delta * levelInfo.TimeRatio : 0; 
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
