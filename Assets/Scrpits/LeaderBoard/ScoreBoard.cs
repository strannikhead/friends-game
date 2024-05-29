using System;
using System.Collections.Generic;

[Serializable]
public class ScoreBoard
{
    public List<ScoreEntry> highScores;

    public ScoreBoard()
    {
        highScores = new List<ScoreEntry>();
    }
}
