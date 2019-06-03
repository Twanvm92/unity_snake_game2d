using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerNieuw : MonoBehaviour
{
  [SerializeField] private SnakePartsController snakePartsController;
  [SerializeField] private SnakeController snakeController;
  [SerializeField] private SpawnerController spawnerController;

  private void Start()
  {
    snakeController.Initialize(snakePartsController, spawnerController);
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
}
