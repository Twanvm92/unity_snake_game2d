using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private SnakePartsController snakePartsController;
	[SerializeField] private SnakeController snakeController;
	[SerializeField] private AiSnakeController aiSnakeController;
	[SerializeField] private AiSnakeController2 aiSnakeController2;
	[SerializeField] private SpawnerController spawnerController;
	[SerializeField] private Background background;
	[SerializeField] private FoodController foodController;
	[SerializeField] private ScoreManager scoreManager;
	[SerializeField] private SnakeAgent snakeAgent;
	[SerializeField] private SnakeAgent2 snakeAgent2;
	[SerializeField] private SnakeAcademy snakeAcademy;
	private float snakeSpeed;
	private float initialWaitTime = 1.0f;
	private float snakeSpeedIncrease = 0.045f;
	
	
	private void Start()
	{
		snakeSpeed = 0.4f;
		spawnerController.Initialize(background);
		foodController.Initialize(spawnerController);
//		TODO is shut off to test playerbrain
//		comment this to stop player snake from showing
		snakeController.Initialize(snakePartsController, spawnerController, foodController, scoreManager);
		
		aiSnakeController.Initialize(snakePartsController, spawnerController, foodController, scoreManager);
		if (aiSnakeController2 != null )
		{
            aiSnakeController2.Initialize(snakePartsController, spawnerController, foodController, scoreManager);
		}

		if (snakeAgent2 != null)
		{
            snakeAgent2.Initialize(aiSnakeController2);
		}
		snakeAgent.Initialize(aiSnakeController);
		
		snakeAcademy.Initialize(snakeAgent, snakeAgent2);
		
		
//		GameObject food = foodController.InitializeFood();
//		aiSnakeController.food = food;
		
//		TODO your own method instead of RestartGame?
		snakeController.OnPlayerHitWallOrSnake += RestartGame;
		aiSnakeController.OnAiHitWallOrSnake += RestartGameAi;
		scoreManager.OnScoreReachedBoundary += GameWon;
		scoreManager.OnScoreChanged += score => ChangeSnakeSpeed(score);
		

//		comment this line to not let the player snake move anymore
		StartCoroutine(MoveSnakeInInterval(initialWaitTime));

//		StartCoroutine(MoveAiSnakeInInterval(initialWaitTime));
//		comment this to not let the Ai snake move on interval anymore
		StartCoroutine(RequestDecisionOnInterval(initialWaitTime));

	}

	private void ChangeSnakeSpeed(int score)
	{
		if (score % 2 == 0 && snakeSpeed > 0.15)
		{
			snakeSpeed -= snakeSpeedIncrease;
		}
	}

	IEnumerator MoveSnakeInInterval(float f)
	{
		yield return new WaitForSeconds(f);
		while( true )
		{
            MoveSnakeHeadDirection();
			yield return new WaitForSeconds(snakeSpeed) ;
		}
	}
	
	IEnumerator MoveAiSnakeInInterval(float f)
	{
		yield return new WaitForSeconds(f);
		while( true )
		{
            MoveAiSnakeHeadDirection();
			yield return new WaitForSeconds(snakeSpeed) ;
		}
	}
	
	IEnumerator RequestDecisionOnInterval(float f)
	{
		yield return new WaitForSeconds(f);
		while( true )
		{
			snakeAgent.RequestDecision();
			snakeAgent.RequestAction();
			yield return new WaitForSeconds(snakeSpeed) ;
		}
	}

  //TODO Opposite direction possible if step keys are spammed fast enough
  public void SnakeMoveNorth()
  {
    snakeController.Move((int) SnakeController.headDirections.North);
  }

  public void SnakeMoveEast()
  {
    snakeController.Move((int) SnakeController.headDirections.East);
  }

  public void SnakeMoveSouth()
  {
    snakeController.Move((int) SnakeController.headDirections.South);
  }

  public void SnakeMoveWest()
  {
    snakeController.Move((int) SnakeController.headDirections.West);
  }


  // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
	        snakeController.Move((int) SnakeController.headDirections.North);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
	        snakeController.Move((int) SnakeController.headDirections.East);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
	        snakeController.Move((int) SnakeController.headDirections.South);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
	        snakeController.Move((int) SnakeController.headDirections.West);
        }
    }

    private void MoveSnakeHeadDirection()
    {
	    snakeController.Move();
    }
    
    private void MoveAiSnakeHeadDirection()
    {
	    aiSnakeController.Move();
    }

//    TODO Restart game/ win/lose screen?
    private void RestartGame()
    {
	    Debug.Log("Player snake hit something!");
	    SceneManager.LoadScene( SceneManager.GetActiveScene().name );
	}
    
//    TODO Restart game/ win/lose screen?
    private void RestartGameAi()
    {
	    Debug.Log("Ai hit something!");
	    SceneManager.LoadScene( SceneManager.GetActiveScene().name );
	}

    private void GameWon()
    {
	    Debug.Log("You won the game!");
    }
}
