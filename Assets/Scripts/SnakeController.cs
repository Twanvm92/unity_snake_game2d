using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private Vector2Int headPosition;
    private int snakeHeadDirection;
    private Queue<Vector2Int> snakeBody = new Queue<Vector2Int>();

    private enum headDirections
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }
    
    private void Awake()
    {
        headPosition = new Vector2Int(0, 8);
        snakeHeadDirection = (int) headDirections.North;
        snakeBody.Enqueue(headPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && snakeHeadDirection != (int) headDirections.South)
        {
            headPosition.y += 1;
            
            snakeHeadDirection = (int) headDirections.North;

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && snakeHeadDirection != (int) headDirections.West)
        {
            headPosition.x += 1;
            snakeHeadDirection = (int) headDirections.East;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && snakeHeadDirection != (int) headDirections.North)
        {
            headPosition.y -= 1;
            snakeHeadDirection = (int) headDirections.South;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && snakeHeadDirection != (int) headDirections.East)
        {
            headPosition.x -= 1;
            snakeHeadDirection = (int) headDirections.West;
        }
        transform.position = new Vector3(headPosition.x, headPosition.y);
    }
}
