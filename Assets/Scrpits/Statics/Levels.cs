using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Levels
{
    public readonly static Dictionary<string, LevelInfo> LevelsDict = new Dictionary<string, LevelInfo> //(29.05.2024) refactored // review(26.05.2024): Давай с заглавной буквы + readonly
    {
        {"FirstLevel", new LevelInfo(1000, 19, 20)},
        {"SecondLevel", new LevelInfo(1000, 10, 10)},
        {"ThirdLevel", new LevelInfo(1000, 60, 10)},
        {"FourthLevel", new LevelInfo(1000, 30, 5) }
    };
}