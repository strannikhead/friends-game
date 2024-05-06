using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public static int lives = 3;
    public static int score = 0;
    public static int levelScore = 0;

    public static void Reset()
    {
        lives = 3;
        score = 0;
        levelScore = 0;
    }
}
