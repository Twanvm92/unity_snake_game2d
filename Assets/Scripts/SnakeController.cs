using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    private Vector2Int headPosition;
    private int snakeHeadDirection;
    private List<GameObject> snakeParts = new List<GameObject>();
    private SnakePartsController snakePartsController;
    private Deque<GameObject> snakeBody = new Deque<GameObject>();
    private SpawnerController spawnerController;
    private float oldSnakeHeadX;
    private float oldSnakeHeadY;
    public event Action OnPlayerHitWallOrSnake;

    private Dictionary<int, int[]> snakeStepMapping = new Dictionary<int, int[]>
    {
        [0] = new[] {0, 1},
        [1] = new[] {1, 0},
        [2] = new[] {0, -1},
        [3] = new[] {-1, 0}
    };

    public enum headDirections
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }
    
    private void Start()
    {
        headPosition = new Vector2Int(0, 8);
        snakeHeadDirection = (int) headDirections.North;

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Move(int direction)
    {
        if (Math.Abs(direction - snakeHeadDirection) != 2)
        {

            GameObject snakeHead = snakeBody.RemoveBack();
            oldSnakeHeadX = snakeHead.transform.position.x;
            oldSnakeHeadY = snakeHead.transform.position.y;
            PrepareToSpawnSnakeBodyPart();

//            update Snakehead to new coordinates
            var newSnakeHeadX = oldSnakeHeadX + snakeStepMapping[direction][0];
            var newSnakeHeadY = oldSnakeHeadY + snakeStepMapping[direction][1];
            Vector3 newSnakeHeadVector = new Vector3(newSnakeHeadX, newSnakeHeadY);
            if (spawnerController.IsNewGridPositionValid(newSnakeHeadVector))
            {
                snakeHead.transform.position = newSnakeHeadVector;
                snakeBody.AddBack(snakeHead);
            }
            else
            {
                OnPlayerHitWallOrSnake.Invoke();
                
            }
            
            
//          TODO  check if snake ate first
            RemoveSnakeTail();

            snakeHeadDirection = direction;

        }
        
    }

    private void InstantiateSnakeHead()
    {
        GameObject snakeHead = snakeParts.Find(i => i.CompareTag("SnakeHead"));
        Vector3 snakeHeadPos = new Vector3(headPosition.x, headPosition.y);
        spawnerController.SpawnSnakeHead(snakeHead, snakeHeadPos);
        
    }

    public void Initialize(SnakePartsController snakePartsController, SpawnerController spawnerController)
    {
        this.snakePartsController = snakePartsController;
		foreach (Transform snakePart in this.snakePartsController.transform)
		{
			snakeParts.Add(snakePart.gameObject);	
		}

        this.spawnerController = spawnerController;
        this.spawnerController.OnSnakeHeadSpawned += snakeHead => PersistSnakePart(snakeHead);
        this.spawnerController.OnSnakeBodyPartSpawned += snakeBodyPart => PersistSnakePart(snakeBodyPart);
        
        InstantiateSnakeHead();
    }

    private void PersistSnakePart(GameObject snakePart)
    {
        snakeBody.AddBack(snakePart);
    }

    private void PrepareToSpawnSnakeBodyPart()
    {
//        make sure to spawn a new body part at the old snake head position
//        only if the snake body is longer than just the head or the snake ate food
//        TODO Also check if food has been eaten with an OR! Otherwise snake never gets longer
        if (snakeBody.Count > 1)
        {
            Vector3 snakeBodyPartPos = new Vector3(oldSnakeHeadX, oldSnakeHeadY);
            GameObject  snakeBodyPart = snakeParts.Find(i => i.CompareTag("SnakeBody"));
            spawnerController.SpawnSnakeBodyPart(snakeBodyPart, snakeBodyPartPos);
        }
        
    }

    private void RemoveSnakeTail()
    {
        Debug.Log(snakeBody.Count);
        if (snakeBody.Count > 1)
        {
            GameObject snakeTail = snakeBody.RemoveFront();
            spawnerController.AddEmptyCell(snakeTail.transform.position);
            Destroy(snakeTail);
        }
    }
}
