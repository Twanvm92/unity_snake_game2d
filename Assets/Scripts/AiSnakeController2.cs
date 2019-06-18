using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSnakeController2 : ISnakeController
{
    public AiSnakeHead2 snakeHead;
    public event Action OnAiHitWallOrSnake;

    private void Awake()
    {
        initialHeadPositionX = -4;
        initialHeadPositionY = 8;
        headPosition = new Vector2Int(initialHeadPositionX, initialHeadPositionY);
        Debug.Log("ai snake Start() head position x and y: " + headPosition.x + " " + headPosition.y);
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
            
            PrepareToSpawnAiSnakeBodyPart();
            snakeBody.AddBack(snakeHead);
            
//          TODO  check if snake ate first
            RemoveSnakeTail();
            if (!foodEaten)
            {
                RaiseOnEmptyStepTaken(snakeTypes.Ai);
            } 
            
            SetFoodEaten(false);
    }

    protected override void InstantiateSnakeHead()
    {
        Debug.Log("ai snake head x and y: " + headPosition.x + ", " + headPosition.y);
        GameObject snakeHead = snakeParts.Find(i => i.CompareTag("AiSnakeHead2"));
        Vector3 snakeHeadPos = new Vector3(headPosition.x, headPosition.y);
        spawnerController.SpawnSnakeHead(snakeHead, snakeHeadPos);
        spawnerController.RemoveEmptyCell(snakeHeadPos);
        
        
    }

    protected override void SnakeHitSomething()
    {
        Debug.Log("Ai snake hit something");
        RaiseSnakeHitSomethingEvent(snakeTypes.Ai);
//        TODO adjusted for agent to get event on hit something
//        OnAiHitWallOrSnake?.Invoke();
    }

    protected override void PlayerSnakeHeadCreated(GameObject gameObject)
    {
//     not implemented   
    }

    protected override void AiSnakeHeadCreated(GameObject gameObject)
    {
//       not implemented 
    }

    protected override void AiSnakeHead2Created(GameObject gameObject)
    {
        snakeHead = gameObject.GetComponent<AiSnakeHead2>();
        snakeHead.OnBorderAiSnakeCollision += SnakeHitSomething;
        snakeHead.OnAiSnakeAteFood += snakeAteFood =>
        {
            SetFoodEaten(snakeAteFood);
            RaiseSnakeAteFood(snakeTypes.Ai);
            foodController.Reset();
            food = foodController.InitializeFood();
            scoreManager.UpdateAiScore();
            
        };
        snakeHeadObject = gameObject;
        PersistAiSnake2Part(gameObject);
    }

    protected override void PersistPlayerSnakePart(GameObject snakePart)
    {
//        not implemented
    }

    protected override void PersistAiSnakePart(GameObject snakePart)
    {
//        not implemented
    }

    protected override void PersistAiSnake2Part(GameObject snakepart)
    {
        snakeBody.AddBack(snakepart);
    }

    protected override void PrepareToSpawnPlayerSnakeBodyPart()
    {
//        not implemented
    }

    protected override void PrepareToSpawnAiSnakeBodyPart()
    {
//        make sure to spawn a new body part at the old snake head position
//        only if the snake body is longer than just the head or the snake ate food
//        TODO Also check if food has been eaten with an OR! Otherwise snake never gets longer
        if (snakeBody.Count > 0 || foodEaten)
        {
            Vector3 snakeBodyPartPos = new Vector3(oldSnakeHeadX, oldSnakeHeadY);
            GameObject  snakeBodyPart = snakeParts.Find(i => i.CompareTag("SnakeBody"));
            spawnerController.SpawnAiSnake2BodyPart(snakeBodyPart, snakeBodyPartPos);
            spawnerController.RemoveEmptyCell(snakeBodyPartPos);
        }
    }

    public override void UpdateSnakeHeadDirection(int action)
    {

        if (action == 1)
        {
            if (snakeHeadDirection == 0)
            {
                snakeHeadDirection += 3;
            }
            else
            {
                snakeHeadDirection -= 1;
            }
        }
        else if (action == 2)
        {
            if (snakeHeadDirection == 3)
            {
                snakeHeadDirection -= 3;
            }
            else
            {
                snakeHeadDirection += 1;
            }
        }

//        0 is the default action for not doing anything on the playerbrain
//        TODO make sure to make player action size 4 and learning brain size 3 but add + 1 to all actions so its between 1-3
        if (action != 0)
        {
            Move();
        } 
//     if action == 3 it means going straight so direction can remain the same.
    }
}
