using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapNode : MonoBehaviour
{
    [SerializeField]
    public string sceneName;
    [SerializeField]
    private List<MapNode> neighbors;
    public bool isEnabled;
    public bool isVisited;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enable()
    {
        if (isVisited)
        {
            return;
        }
        isEnabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Disable()
    {
        if (isVisited)
        {
            return;
        }
        isEnabled = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
    }

    public void EnableNeibors()
    {
        foreach (var neighbor in neighbors)
        {
            neighbor.Enable();
        }
    }

    public void DisableNeibors()
    {
        foreach(var neighbor in neighbors)
        {
            neighbor.Disable();
        }
    }

    public bool Equals(MapNode other)
    {
        return gameObject.transform.position == other.gameObject.transform.position;
    }

    public void LoadThisScene()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        FindAnyObjectByType<MainMapPlayer>().gameObject.SetActive(false);
        GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;
        GameObject.Find("Map").SetActive(false);
        isVisited = true;
        EnableNeibors();
        gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }
}
