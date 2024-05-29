using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escape : MonoBehaviour
{
    private GameObject UI;
    private GameObject player;
    private GameObject eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        UI = FindAnyObjectByType<Canvas>().gameObject;
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
            // review(26.05.2024): Может, инапсулируем логику с Game внутри Game?
            Game.lives += 1;
            Game.levelScore = 0;
            TimeSystem.Stop();
            UI.SetActive(false);
            player.SetActive(false);
            eventSystem?.SetActive(false);
            // review(26.05.2024): Как тебе идея инкапсулировать работу с MapModel внутри MapModel? Тогда, возможно, даже не понадобится наружу выставлять playerPos
            MapModel.playerPos.node.isVisited = true;
            MapModel.playerPos.EnableNeibors();
            SceneManager.LoadScene("ScoreScene", LoadSceneMode.Additive);
        }
    }

}
