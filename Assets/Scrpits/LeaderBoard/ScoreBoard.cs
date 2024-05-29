using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScoreBoard
{
    public List<ScoreEntry> highScores;

    public ScoreBoard()
    {
        highScores = new List<ScoreEntry>();
    }
}
