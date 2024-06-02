using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    // review(26.05.2024): Почему поля, а не свойства?
    public static int lives = 3;
    public static int score = 0;
    public static int levelScore = 0;

    public static void Reset()
    {
        lives = 3;
        score = 0;
        levelScore = 0;
    }

    public static void EndLevel()
    {
        levelScore = 0;
        lives += 1;
    }
}
