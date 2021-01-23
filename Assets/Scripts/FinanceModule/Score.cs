using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public int bestScore = 0;
    public int currentScore;
    public Text scoretext;
    public Text currenttext;

    void Start()
    {
        if (MapManager.Instance.currentGameData != null)
            bestScore = MapManager.Instance.currentGameData.getBestScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void isNewBestScore()
    {
        currentScore = MapHeat.Instance.ReturnHeatScore();
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
        }
    }

    public void setScoreText()
    {
        int tmp = bestScore;
        scoretext.text = "Best Score: " + tmp.ToString();
    }

    public void setCurrentScoreText()
    {
        currentScore = MapHeat.Instance.ReturnHeatScore();
        int tmp = currentScore;
        currenttext.text = "Current Score: " + tmp.ToString();
    }
}
