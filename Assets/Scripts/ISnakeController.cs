using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MLAgents;
using UnityEngine;

public abstract class ISnakeController : MonoBehaviour
{
    protected Vector2Int headPosition;
    public int snakeHeadDirection;
    protected List<GameObject> snakeParts = new List<GameObject>();
    private SnakePartsController snakePartsController;
    protected Deque<GameObject> snakeBody = new Deque<GameObject>();
    public SpawnerController spawnerController;
    public FoodController foodController;
    protected ScoreManager scoreManager;
    protected float oldSnakeHeadX;
    protected float oldSnakeHeadY;
    protected bool foodEaten;
    protected int initialHeadPositionX;
    protected int initialHeadPositionY;
    public static GameObject food;
    public GameObject snakeHeadObject;
    public event Action<snakeTypes> OnSnakeHitWallOrSnake;
    public event Action<snakeTypes> OnSnakeAteFood;
    public event Action<snakeTypes> OnEmptyStepTaken;

    public Dictionary<int, int[]> snakeStepMapping = new Dictionary<int, int[]>
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

    public enum snakeTypes
    {
        Player = 0,
        Ai = 1
    }

    public enum rewardType
    {
        Food = 0,
        Die = 1,
        Step = 2
    }

    public abstract void Move(int direction);

    public abstract void Move();

    protected abstract void InstantiateSnakeHead();

    public void Initialize(SnakePartsController snakePartsController, SpawnerController spawnerController,
        FoodController foodController, ScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
        this.snakePartsController = snakePartsController;
		foreach (Transform snakePart in this.snakePartsController.transform)
		{
			snakeParts.Add(snakePart.gameObject);	
		}

        this.foodController = foodController;
            
        this.spawnerController = spawnerController;
        this.spawnerController.OnSnakeHeadSpawned += snakeHead => PlayerSnakeHeadCreated(snakeHead);
        this.spawnerController.OnAiSnakeHeadSpawned += snakeHead => AiSnakeHeadCreated(snakeHead);
        this.spawnerController.OnAiSnakeHead2Spawned += snakeHead => AiSnakeHead2Created(snakeHead);
        this.spawnerController.OnPlayerSnakeBodyPartSpawned += snakeBodyPart => PersistPlayerSnakePart(snakeBodyPart);
        this.spawnerController.OnAiSnakeBodyPartSpawned += snakeBodyPart => PersistAiSnakePart(snakeBodyPart);
        this.spawnerController.OnAiSnake2BodyPartSpawned += snakeBodyPart => PersistAiSnake2Part(snakeBodyPart);
        
        InstantiateSnakeHead();
    }

    protected abstract void SnakeHitSomething();

    protected abstract void PlayerSnakeHeadCreated(GameObject gameObject);
    protected abstract void AiSnakeHeadCreated(GameObject gameObject);
    protected abstract void AiSnakeHead2Created(GameObject gameObject);

    protected void SetFoodEaten(bool foodEaten)
    {
        this.foodEaten = foodEaten;
    }

    protected abstract void PersistPlayerSnakePart(GameObject snakePart);

    protected abstract void PersistAiSnakePart(GameObject snakepart);
    protected abstract void PersistAiSnake2Part(GameObject snakepart);

    protected abstract void PrepareToSpawnPlayerSnakeBodyPart();

    protected abstract void PrepareToSpawnAiSnakeBodyPart();

    protected void RemoveSnakeTail()
    {
        if (snakeBody.Count > 1 && foodEaten != true)
        {
            GameObject snakeTail = snakeBody.RemoveFront();
            Destroy(snakeTail);
            spawnerController.AddEmptyCell(snakeTail.transform.position);
        }
    }

    public void Reset()
    {
        
        headPosition = new Vector2Int(initialHeadPositionX, initialHeadPositionY);
        snakeHeadDirection = (int) headDirections.North;

        DestroyCurrentSnake();

        foodEaten = false;
        InstantiateSnakeHead();
    }

    public void DestroyCurrentSnake()
    {
        
        snakeBody.ToList().ForEach(snakePart =>
        {
            spawnerController.AddEmptyCell(snakePart.transform.position);
            Destroy(snakePart);
        });
        snakeBody.Clear();
    }

    public abstract void UpdateSnakeHeadDirection(int action);

    protected void RaiseSnakeHitSomethingEvent(snakeTypes snakeType)
    {
        OnSnakeHitWallOrSnake?.Invoke(snakeType);
    }

    protected void RaiseSnakeAteFood(snakeTypes snakeType)
    {
        OnSnakeAteFood?.Invoke(snakeType);
    }

    protected void RaiseOnEmptyStepTaken(snakeTypes snakeType)
    {
        OnEmptyStepTaken?.Invoke(snakeType);
    }

}
