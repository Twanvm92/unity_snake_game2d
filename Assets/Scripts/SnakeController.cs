using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : ISnakeController
{
    public SnakeHead snakeHead;
    public event Action OnPlayerHitWallOrSnake;
    
    private void Start()
    {
        initialHeadPositionX = -4;
        initialHeadPositionY = 8;
        headPosition = new Vector2Int(initialHeadPositionX, initialHeadPositionY);
        snakeHeadDirection = (int) headDirections.North;

    }

    public override void Move(int direction)
    {
        UpdateSnakeHeadDirection(direction);
    }

    public override void Move()
    {
            GameObject snakeHead = snakeBody.RemoveBack();
            Vector3 oldSnakeHeadVector = snakeHead.transform.position;
            oldSnakeHeadX = oldSnakeHeadVector.x;
            oldSnakeHeadY = oldSnakeHeadVector.y;

//            update Snakehead to new coordinates
            var newSnakeHeadX = oldSnakeHeadX + snakeStepMapping[snakeHeadDirection][0];
            var newSnakeHeadY = oldSnakeHeadY + snakeStepMapping[snakeHeadDirection][1];
            Vector3 newSnakeHeadVector = new Vector3(newSnakeHeadX, newSnakeHeadY);
            

//                put prepareToSpawnSnakeBodyPart() after setting position of new snake head
//                otherwise foodEaten cannot be set to true through collision event before snake body part is spawned
            snakeHead.transform.position = newSnakeHeadVector;
            
            spawnerController.AddEmptyCell(oldSnakeHeadVector);
            spawnerController.RemoveEmptyCell(newSnakeHeadVector);
            
            PrepareToSpawnPlayerSnakeBodyPart();
            snakeBody.AddBack(snakeHead);
            
//          TODO  check if snake ate first
            RemoveSnakeTail();

            if (!foodEaten)
            {
                RaiseOnEmptyStepTaken(snakeTypes.Player);
            }
            
            SetFoodEaten(false);

    }

    protected override void InstantiateSnakeHead()
    {
        Debug.Log("Instantiating player snake head");
        GameObject snakeHead = snakeParts.Find(i => i.CompareTag("SnakeHead"));
        Vector3 snakeHeadPos = new Vector3(headPosition.x, headPosition.y);
        spawnerController.SpawnSnakeHead(snakeHead, snakeHeadPos);
        spawnerController.RemoveEmptyCell(snakeHeadPos);
        
        
    }

    protected override void SnakeHitSomething()
    {
        Debug.Log("Player snake hit something");
        RaiseSnakeHitSomethingEvent(snakeTypes.Player);
//        TODO adjusted for agent to get event on hit something
        OnPlayerHitWallOrSnake?.Invoke();
    }

    protected override void PlayerSnakeHeadCreated(GameObject gameObject)
    {
        snakeHead = gameObject.GetComponent<SnakeHead>();
        snakeHead.OnBorderSnakeCollision += SnakeHitSomething;
        snakeHead.OnSnakeAteFood += snakeAteFood =>
        {
            SetFoodEaten(snakeAteFood);
            RaiseSnakeAteFood(snakeTypes.Player);
            foodController.Reset();
            food = foodController.InitializeFood();
            scoreManager.UpdatePlayerScore();
            
        };
        snakeHeadObject = gameObject;
        PersistPlayerSnakePart(gameObject);
    }

    protected override void AiSnakeHeadCreated(GameObject gameObject)
    {
//        not implemented
    }

    protected override void AiSnakeHead2Created(GameObject gameObject)
    {
//        not implemented
    }

    protected override void PersistPlayerSnakePart(GameObject snakePart)
    {
        snakeBody.AddBack(snakePart);
    }

    protected override void PersistAiSnakePart(GameObject snakepart)
    {
//        not implemented
    }

    protected override void PersistAiSnake2Part(GameObject snakepart)
    {
//        not implemented
    }

    protected override void PrepareToSpawnPlayerSnakeBodyPart()
    {
//        make sure to spawn a new body part at the old snake head position
//        only if the snake body is longer than just the head or the snake ate food
//        TODO Also check if food has been eaten with an OR! Otherwise snake never gets longer
        if (snakeBody.Count > 0 || foodEaten)
        {
            Vector3 snakeBodyPartPos = new Vector3(oldSnakeHeadX, oldSnakeHeadY);
            GameObject  snakeBodyPart = snakeParts.Find(i => i.CompareTag("SnakeBody"));
            spawnerController.SpawnPlayerSnakeBodyPart(snakeBodyPart, snakeBodyPartPos);
            spawnerController.RemoveEmptyCell(snakeBodyPartPos);
        }
    }

    protected override void PrepareToSpawnAiSnakeBodyPart()
    {
//        not implemented
    }

    public override void UpdateSnakeHeadDirection(int action)
    {
        if (Math.Abs(action - snakeHeadDirection) != 2)
        {
            snakeHeadDirection = action;

        }
    }
}
