using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEditor.Build;
using UnityEngine;

public class SnakeAgent : Agent
{
    public ISnakeController snakeController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
     int action;
     if (brain.name == "SnakeBrain")
     {
      action = Mathf.FloorToInt(vectorAction[0]) + 1;
     }
     else
     {
      action = Mathf.FloorToInt(vectorAction[0]);
      
     }
//        TODO make sure to make player action size 4 and learning brain size 3 but add + 1 to all actions so its between 1-3
      snakeController.Move(action);

    }

    public override void CollectObservations()
    {
//        TODO check if cell below right left or up is empty in empty cells and check if new vector is not same as food vector
        SpawnerController spawnerController = snakeController.spawnerController;
        HashSet<Vector2Int> emptyCells = spawnerController.emptyCells;
        Vector3 snakeHeadVector = snakeController.snakeHeadObject.transform.position;
        var snakeHeadX = snakeHeadVector.x;
        var snakeHeadY = snakeHeadVector.y;
        var currentSnakeHeadDirec = snakeController.snakeHeadDirection;
        int snakeHeadDirNorth = (int) ISnakeController.headDirections.North;
        int snakeHeadDirEast = (int) ISnakeController.headDirections.East;
        int snakeHeadDirSouth = (int) ISnakeController.headDirections.South;
        int snakeHeadDirWest = (int) ISnakeController.headDirections.West;
        var foodVector = ISnakeController.food.transform.position;
        var foodX = (int) foodVector.x;
        var foodY = (int) foodVector.y;
        
        
        int newSnakeHeadXNorth = (int) (snakeHeadX + snakeController.snakeStepMapping[snakeHeadDirNorth][0]);
        int newSnakeHeadYNorth = (int) (snakeHeadY + snakeController.snakeStepMapping[snakeHeadDirNorth][1]);
        int newSnakeHeadXEast = (int) (snakeHeadX + snakeController.snakeStepMapping[snakeHeadDirEast][0]);
        int newSnakeHeadYEast = (int) (snakeHeadY + snakeController.snakeStepMapping[snakeHeadDirEast][1]);
        int newSnakeHeadXSouth = (int) (snakeHeadX + snakeController.snakeStepMapping[snakeHeadDirSouth][0]);
        int newSnakeHeadYSouth = (int) (snakeHeadY + snakeController.snakeStepMapping[snakeHeadDirSouth][1]);
        int newSnakeHeadXWest = (int) (snakeHeadX + snakeController.snakeStepMapping[snakeHeadDirWest][0]);
        int newSnakeHeadYWest = (int) (snakeHeadY + snakeController.snakeStepMapping[snakeHeadDirWest][1]);

        Vector2Int coordLeftOfSnake = new Vector2Int(newSnakeHeadXWest, newSnakeHeadYWest);
        Vector2Int coordRightOfSnake = new Vector2Int(newSnakeHeadXEast, newSnakeHeadYEast);
        Vector2Int coordNorthOfSnake = new Vector2Int(newSnakeHeadXNorth, newSnakeHeadYNorth);
        Vector2Int coordSouthOfSnake = new Vector2Int(newSnakeHeadXSouth, newSnakeHeadYSouth);
        Vector2Int foodCoordinatesSet = new Vector2Int(foodX, foodY);

        bool snakeHeadDirectionIsNorth = currentSnakeHeadDirec == snakeHeadDirNorth;
        bool snakeHeadDirectionIsEast = currentSnakeHeadDirec == snakeHeadDirEast;
        bool snakeHeadDirectionIsSouth = currentSnakeHeadDirec == snakeHeadDirSouth;
        bool snakeHeadDirectionIsWest = currentSnakeHeadDirec == snakeHeadDirWest;
        bool dangerLeft = (snakeHeadDirectionIsNorth && !emptyCells.Contains(coordLeftOfSnake) &&
                           coordLeftOfSnake != foodCoordinatesSet) ||
                          (snakeHeadDirectionIsEast && !emptyCells.Contains(coordNorthOfSnake) &&
                           coordNorthOfSnake != foodCoordinatesSet) ||
                          (snakeHeadDirectionIsSouth && !emptyCells.Contains(coordRightOfSnake) &&
                           coordRightOfSnake != foodCoordinatesSet) ||
                          (snakeHeadDirectionIsWest && !emptyCells.Contains(coordSouthOfSnake) &&
                           coordSouthOfSnake != foodCoordinatesSet);
        bool dangerStraight = (snakeHeadDirectionIsNorth && !emptyCells.Contains(coordNorthOfSnake) &&
                           coordNorthOfSnake != foodCoordinatesSet) ||
                          (snakeHeadDirectionIsEast && !emptyCells.Contains(coordRightOfSnake) &&
                           coordRightOfSnake != foodCoordinatesSet) ||
                          (snakeHeadDirectionIsSouth && !emptyCells.Contains(coordSouthOfSnake) &&
                           coordSouthOfSnake != foodCoordinatesSet) ||
                          (snakeHeadDirectionIsWest && !emptyCells.Contains(coordLeftOfSnake) &&
                           coordLeftOfSnake != foodCoordinatesSet);
        bool dangerRight = (snakeHeadDirectionIsNorth && !emptyCells.Contains(coordRightOfSnake) &&
                           coordRightOfSnake != foodCoordinatesSet) ||
                          (snakeHeadDirectionIsEast && !emptyCells.Contains(coordSouthOfSnake) &&
                           coordSouthOfSnake != foodCoordinatesSet) ||
                          (snakeHeadDirectionIsSouth && !emptyCells.Contains(coordLeftOfSnake) &&
                           coordLeftOfSnake != foodCoordinatesSet) ||
                          (snakeHeadDirectionIsWest && !emptyCells.Contains(coordNorthOfSnake) &&
                           coordNorthOfSnake != foodCoordinatesSet);
        
                          
        AddVectorObs(dangerLeft ? 1.0f : 0.0f);
        AddVectorObs(dangerStraight ? 1.0f : 0.0f);
        AddVectorObs(dangerRight? 1.0f : 0.0f);
        
//        add state observation one by one
        AddVectorObs(snakeHeadDirectionIsWest ? 1.0f : 0.0f); // snake head direction left
        AddVectorObs(snakeHeadDirectionIsEast ? 1.0f : 0.0f); // snake head direction right
        AddVectorObs(snakeHeadDirectionIsNorth ? 1.0f : 0.0f); // snake head direction up
        AddVectorObs(snakeHeadDirectionIsSouth ? 10.0f : 0.0f); // snake head direction down
//        Check if food is there when ai makes action to make on food (which makes the food disappear?)
        AddVectorObs(snakeHeadX > foodX ? 1.0f : 0.0f); // food left of snake
        AddVectorObs(snakeHeadX < foodX ? 1.0f : 0.0f); // food right of snake
        AddVectorObs(snakeHeadY < foodY ? 1.0f : 0.0f); // food above snake
        AddVectorObs(snakeHeadY > foodY ? 1.0f : 0.0f); // food below snake
        
        
    }
    

    public override void AgentReset()
    {
        Debug.Log("Agent Reset");
        snakeController.Reset();
        
    }

    public void Initialize(ISnakeController snakeController)
    {
        this.snakeController = snakeController;
        this.snakeController.OnSnakeHitWallOrSnake += snakeType =>
        {
         SetReward(snakeType, ISnakeController.rewardType.Die);
         SetDone(snakeType);
        };
        this.snakeController.OnSnakeAteFood += snakeType =>
        {
         SetReward(snakeType, ISnakeController.rewardType.Food);
        };
        this.snakeController.OnEmptyStepTaken += snakeType => SetReward(snakeType, ISnakeController.rewardType.Step);


    }

    private void SetDone(ISnakeController.snakeTypes snakeType)
    {
     Debug.Log("SetDone called!");
     if (snakeType == ISnakeController.snakeTypes.Ai)
     {

      Done();
      if (!agentParameters.resetOnDone)
      {
        snakeController.DestroyCurrentSnake();
      }
     }
    }

//    TODO rewards should be normalized (so in range [-1, 1])
    private void SetReward(ISnakeController.snakeTypes snakeType, ISnakeController.rewardType rewardType)
    {
     if (snakeType == ISnakeController.snakeTypes.Ai)
     {
      if (rewardType == ISnakeController.rewardType.Food)
      {
       AddReward(1f);
      }
//      else if (rewardType == ISnakeController.rewardType.Step)
//      {
//       AddReward(-0.001f);
//      }
      else if (rewardType == ISnakeController.rewardType.Die)
      {
       AddReward(-1f);
      }
     }
    }
}
