using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int playerScore;
    private int aiScore;
    private int scoreBoundary;
	public Text scoreDisplay;

    public event Action OnScoreReachedBoundary;
    public event Action<int> OnScoreChanged;

    private void Update()
	{
		scoreDisplay.text = playerScore.ToString();
	}

    public void UpdatePlayerScore()
    {
        playerScore++;
        Debug.Log(" Player Score: "  + playerScore);
        OnScoreChanged?.Invoke(playerScore + aiScore);

        if (playerScore >= 10)
        {
            OnScoreReachedBoundary?.Invoke();
        }
    }

    public void UpdateAiScore()
    {
        aiScore++;
        Debug.Log("Ai Score: "  + aiScore);
        OnScoreChanged?.Invoke(aiScore + playerScore);

        if (aiScore >= 10)
        {
            OnScoreReachedBoundary?.Invoke();
        }
	    
    }

    public void Reset()
    {
        aiScore = 0;
        playerScore = 0;
    }
    
    
}
