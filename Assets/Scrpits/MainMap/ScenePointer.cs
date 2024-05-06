using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePointer : MonoBehaviour
{
    public GameObject player;
    private GameObject map;
    private GameObject sceneCamera;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<MainMapPlayer>().gameObject;
        map = GameObject.Find("Map");
        sceneCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.loadedSceneCount == 1)
        {
            player.SetActive(true);
            map.SetActive(true);
            sceneCamera.SetActive(true);
        }
    }
}
