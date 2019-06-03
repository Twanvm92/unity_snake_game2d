using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private SnakePartsController snakePartsController;
	[SerializeField] private SnakeController snakeController;
	[SerializeField] private SpawnerController spawnerController;
	[SerializeField] private Background background;

	private void Start()
	{
		spawnerController.Initialize(background);
		snakeController.Initialize(snakePartsController, spawnerController);
//		TODO your own method instead of RestartGame?
		snakeController.OnPlayerHitWallOrSnake += RestartGame;
		
		InvokeRepeating("MoveSnakeHeadDirection", 1.0f, 0.5f);
	}

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

//    TODO Restart game/ win/lose screen?
    private void RestartGame()
    {
	    SceneManager.LoadScene( SceneManager.GetActiveScene().name );
	}
}
