using System;
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
    public int id;
    public Node node => MapModel.Nodes[id];
    [SerializeField]
    private List<MapNode> neighbors;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // review(26.05.2024): Тут как будто не хватает метода GetColorFromState, возвращающий цвет.
        // Сделать действительно можно, но очень уж геморно)) // review(26.05.2024): Почему бы не выделить enum NodeState { None, Visited, Enabled } ? Кмк так более описательно будет
        // review(29.06.2024): Вот я сделал не геморно, вроде. Даже особо ничего не менял
        var color = node.State switch
        {
            NodeState.None    => new Color(40/255f, 40/255f, 40/255f),
            NodeState.Visited => new Color(200/255f, 250/255f, 180/255f),
            NodeState.Enabled => Color.white,
            _                 => throw new ArgumentOutOfRangeException()
        };
        spriteRenderer.color = color;
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
        var neibors = node.NeiborIds.Select(x => MapModel.Nodes[x]);
        foreach (var neighbor in neibors)
        {
            // review(29.06.2024): Разве тут не должна быть проверка на visited?
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
        //refactored (30.05.2024) // review(26.05.2024): Кажется, что две MapNode равны, если равны их id
        return id == other.id;
    }

    public void LoadThisScene()
    {
        SceneManager.LoadScene(node.SceneName, LoadSceneMode.Additive);
        FindAnyObjectByType<MainMapPlayer>().gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("UI").SetActive(false);
        StartCoroutine(TurnSceneOff());
    }

    private IEnumerator TurnSceneOff()
    {
        yield return new WaitForSeconds(1);
        GameObject.Find("Map").SetActive(false);
        GameObject.Find("Main Camera").SetActive(false);
    }
}
