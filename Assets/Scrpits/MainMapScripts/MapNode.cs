using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

class MapNode : MonoBehaviour
{
    public string sceneName => node.SceneName;
    [SerializeField]
    private List<MapNode> neighbors;
    [SerializeField]
    public int id;
    public Node node => MapModel.nodes[id];
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (node.isVisited)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            return;
        }
        if (node.isEnabled)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            return;
        }
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
    }

    public void Enable()
    {
        if (node.isVisited)
        {
            return;
        }
        node.isEnabled = true;
    }

    public void Disable()
    {
        if (node.isVisited)
        {
            return;
        }
        node.isEnabled = false;
    }

    public void EnableNeibors()
    {
        var neibors = node.NeiborIds.Select(x => MapModel.nodes[x]);
        foreach (var neighbor in neibors)
        {
            neighbor.isEnabled = true;
        }
    }

    public void DisableNeibors()
    {
        foreach(var neighbor in neighbors)
        {
            neighbor.node.isEnabled = false;
        }
    }

    public bool Equals(MapNode other)
    {
        return gameObject.transform.position == other.gameObject.transform.position;
    }

    public void LoadThisScene()
    {
        SceneManager.LoadScene(node.SceneName, LoadSceneMode.Additive);
        FindAnyObjectByType<MainMapPlayer>().gameObject.SetActive(false);
        StartCoroutine(TurnSceneOff());
    }

    private IEnumerator TurnSceneOff()
    {
        yield return new WaitForSeconds(1);
        GameObject.Find("Map").SetActive(false);
        GameObject.Find("Main Camera").SetActive(false);
    }
}
