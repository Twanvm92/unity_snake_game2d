using System;
using UnityEngine;
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
//        transform.position = new Vector3(headPosition.x, headPosition.y);
    }

//    TODO Restart game/ win/lose screen?
    private void RestartGame()
    {
	    SceneManager.LoadScene( SceneManager.GetActiveScene().name );
	}
}
