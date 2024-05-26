using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Levels
{
    public static Dictionary<string, LevelInfo> levels = new Dictionary<string, LevelInfo> // review(26.05.2024): Давай с заглавной буквы + readonly
    {
        {"FirstLevel", new LevelInfo(1000, 19, 20)},
        {"SecondLevel", new LevelInfo(1000, 10, 10)},
        {"ThirdLevel", new LevelInfo(1000, 60, 10)},
        {"FourthLevel", new LevelInfo(1000, 30, 5) }
    };
}

// review(26.05.2024): Давай в отдельный файл
public record LevelInfo(int MaxTimeBonus, float TimeTreshold, float TimeRatio)
{

}
