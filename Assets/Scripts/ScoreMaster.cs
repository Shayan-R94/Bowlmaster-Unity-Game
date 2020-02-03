using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreMaster {

    public static List<int> ScoreCumulative(List<int> rolls)
    {
        List<int> cumulativeScores = new List<int>();
        int runningTotal = 0;

        foreach (int frameScore in ScoreFrames(rolls))
        {
            runningTotal += frameScore;
            cumulativeScores.Add(runningTotal);
        }
        return cumulativeScores;
    }

    public static List<int> ScoreFrames(List<int> rolls)
    {
        List<int> frames = new List<int>();

        // Index i points to 2nd bowl of frame.
        for(int i = 1; i < rolls.Count; i += 2)
        {
            // Prevents 11th frame score.
            if(frames.Count == 10)
            {
                break;
            }
            if (rolls[i - 1] + rolls[i] < 10)
            {
                frames.Add(rolls[i - 1] + rolls[i]);
            }

            // Ensure at least 1 look-ahead available.
            if(rolls.Count - i <= 1)
            {
                break;
            }

            // Strike.
            if(rolls[i - 1] == 10)
            {
                // Strike frame has just one bowl.
                i--;
                frames.Add(10 + rolls[i + 1] + rolls[ i + 2]);
            } else if(rolls[i - 1] + rolls[i] == 10)
            {
                // Spare.
                frames.Add(10 + rolls[i + 1]);
            }
        }

        return frames;
    }
}
