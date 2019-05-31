using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private SnakePartsController snakePartsController;
	[SerializeField] private SnakeController snakeController;
	[SerializeField] private SpawnerController spawnerController;

	private void Start()
	{
		snakeController.Initialize(snakePartsController, spawnerController);
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

}
