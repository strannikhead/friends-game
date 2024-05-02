using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calculator : MonoBehaviour
{
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
        countedTime = 0;
        elapsedTime = TimeSystem.ElapsedTime;
        levelInfo = Levels.levels
            [SceneManager
            .GetActiveScene()
            .GetRootGameObjects()
            .Select(x => x.gameObject.GetComponent<ScenePointer>())
            .Where(x => x != null)
            .FirstOrDefault()
            .player
            .GetComponent<MainMapPlayer>()
            .location
            .sceneName];
        timeTreshold = levelInfo.TimeTreshold;
        timeBonus = levelInfo.MaxTimeBonus;
    }

    void FixedUpdate()
    {
        if (countedTime < elapsedTime)
        {
            var delta = Time.deltaTime * fillingSpeed;
            countedTime += delta;
            timeBonus = timeBonus > 0 ? timeBonus - delta * levelInfo.TimeRatio : 0;
        }
        else if (!isScored)
        {
            ScoreSystem.score += (int)timeBonus;
            TimeSystem.Reset();
            isScored = true;
        }
    }
}
