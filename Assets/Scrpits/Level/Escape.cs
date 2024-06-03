using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escape : MonoBehaviour
{
    private GameObject UI;
    private GameObject player;
    private GameObject eventSystem;
    [SerializeField]
    private string targetSceneName;
    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("UI");
        //UI = FindAnyObjectByType<Canvas>().gameObject;
        player = FindAnyObjectByType<Player>().gameObject;
        eventSystem = GameObject.Find("EventSystem");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            //(29.05.2024) refactored // review(26.05.2024): Может, инапсулируем логику с Game внутри Game?
            Game.EndLevel();
            TimeSystem.Stop();
            UI.SetActive(false);
            player.SetActive(false);
            eventSystem?.SetActive(false);
            //(29.05.2024) refactored // review(26.05.2024): Как тебе идея инкапсулировать работу с MapModel внутри MapModel? Тогда, возможно, даже не понадобится наружу выставлять playerPos
            MapModel.CompleteCurrentLevel();
            SceneManager.LoadScene(targetSceneName, LoadSceneMode.Additive);
        }
    }

}
