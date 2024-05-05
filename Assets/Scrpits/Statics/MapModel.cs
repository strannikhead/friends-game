using System.Collections.Generic;
using UnityEngine;

static class MapModel
{
    public static Dictionary<int, Node> nodes = new()
    {
        {1, new Node(new int[]{2}, 1, "FirstLevel") },
        {2, new Node(new int[]{1,3}, 2, "FirstLevel") },
        {3, new Node(new int[]{2,4}, 3, "SecondLevel") },
        {4, new Node(new int[]{3}, 4, "ThirdLevel") },
    };
    public static MapNode playerPos;

    public static void Initialize(MapNode[] mapNodes, MapNode start)
    {
        Debug.Log(mapNodes.Length);
        var length = mapNodes.Length;
        playerPos = start;
        for (int i = 0; i < length; i++)
        {
            nodes[i+1].MapNode = mapNodes[i];
        }
    }
}

class Node
{
    public int[] NeiborIds;
    public int id;
    public bool isVisited;
    public bool isEnabled;
    public string SceneName;
    public MapNode MapNode;

    public Node(int[] neiborIds, int id, string sceneName)
    {
        NeiborIds = neiborIds;
        this.id = id;
        SceneName = sceneName;
    }
}