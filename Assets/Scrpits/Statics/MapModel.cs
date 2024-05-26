using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

static class MapModel
{
    private static Func<Dictionary<int, Node>> originalNodes = () => new() // review(26.05.2024): Статическая переменная с большой буквы
    {
        {1, new Node(new int[]{2}, 1, "FirstLevel") }, // review(26.05.2024): Было бы круто выделить класс Levels/LevelsNames, где определить корректные названия уровней, а то сейчас это строки как тут, так и в другом классе
        {2, new Node(new int[]{1,3}, 2, "FourthLevel") },
        {3, new Node(new int[]{2,4}, 3, "SecondLevel") },
        {4, new Node(new int[]{3}, 4, "ThirdLevel") },
    };
    public static Dictionary<int, Node> nodes = originalNodes(); // review(26.05.2024): А зачем понадобилось делать это функцией, а не просто каждый раз копировать дикт?
    public static MapNode playerPos; // review(26.05.2024): UpperCase + не экономь на символах

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

    public static void Reset()
    {
        nodes = originalNodes();
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