using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{

    private int bestScore;

    public bool isNewBestScore(int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
            return true;
        }
        else
        {
            return false;
        }
    }
}
