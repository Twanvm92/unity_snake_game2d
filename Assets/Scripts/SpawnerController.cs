using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class SpawnerController : MonoBehaviour
{

    private Background background;
    public HashSet<Vector2Int> emptyCells = new HashSet<Vector2Int>();
    private int borderOffSet;
    private int backgroundMinX;
    private int backgroundMaxX;
    private int backgroundMinY;
    private int backgroundMaxY;



    public event Action<GameObject> OnSnakeHeadSpawned;
    public event Action<GameObject> OnAiSnakeHeadSpawned;
    public event Action<GameObject> OnAiSnakeHead2Spawned;

    public event Action<GameObject> OnPlayerSnakeBodyPartSpawned;
    public event Action<GameObject> OnAiSnakeBodyPartSpawned;
    public event Action<GameObject> OnAiSnake2BodyPartSpawned;

    public void SpawnSnakeHead(GameObject gameObject, Vector3 vector3)
    {
        GameObject snakeHead = Instantiate(gameObject, vector3, Quaternion.identity);

        if (snakeHead.CompareTag("SnakeHead"))
        {
           OnSnakeHeadSpawned?.Invoke(snakeHead); 
        }
        else if (snakeHead.CompareTag("AiSnakeHead"))
        {
            Debug.Log("Ai snake head spawned");
            OnAiSnakeHeadSpawned?.Invoke(snakeHead);
        }
        else if (snakeHead.CompareTag("AiSnakeHead2"))
        {
            Debug.Log("Ai snake head 2 spawned");
            OnAiSnakeHead2Spawned?.Invoke(snakeHead);
        }




    }

    public void SpawnPlayerSnakeBodyPart(GameObject gameObject, Vector3 vector3)
    {
        GameObject snakeBodyPart = Instantiate(gameObject, vector3, Quaternion.identity);
        OnPlayerSnakeBodyPartSpawned?.Invoke(snakeBodyPart);
            
    }

    public void SpawnAiSnakeBodyPart(GameObject gameObject, Vector3 vector3)
    {
        GameObject snakeBodyPart = Instantiate(gameObject, vector3, Quaternion.identity);
        OnAiSnakeBodyPartSpawned?.Invoke(snakeBodyPart);
            
    }
    
    public void SpawnAiSnake2BodyPart(GameObject gameObject, Vector3 vector3)
    {
        GameObject snakeBodyPart = Instantiate(gameObject, vector3, Quaternion.identity);
        OnAiSnake2BodyPartSpawned?.Invoke(snakeBodyPart);
            
    }
    
    public GameObject SpawnFood(GameObject gameObject)
    {
        Random randomizer = new Random();
        Vector2Int[] asArray = emptyCells.ToArray();
        Vector2Int randomEmptyCellVector = asArray[randomizer.Next(asArray.Length)];
        Vector3 vector3 = new Vector3(randomEmptyCellVector.x, randomEmptyCellVector.y);
        GameObject food = Instantiate(gameObject, vector3, Quaternion.identity);

        emptyCells.Remove(randomEmptyCellVector);
        
        return food;
    }

    public void Initialize(Background background)
    {
        this.background = background;
        borderOffSet = 1;
        backgroundMinX = (int) this.background.GetComponent<SpriteRenderer>().bounds.min.x + borderOffSet;
        backgroundMaxX = (int) this.background.GetComponent<SpriteRenderer>().bounds.max.x;
        backgroundMinY = (int) this.background.GetComponent<SpriteRenderer>().bounds.min.y + borderOffSet;
        backgroundMaxY = (int) this.background.GetComponent<SpriteRenderer>().bounds.max.y;
        
//        start i and j index at 1 instead of 0 and go until second last value of x and y to add a padding of 1 around the grid
        for (int i = backgroundMinX; i < backgroundMaxX; i++)
        {
            for (int j = backgroundMinY; j < backgroundMaxY; j++)
            {
                emptyCells.Add(new Vector2Int(i, j));
            }
        }

    }

    public void Reset()
    {
        
        Debug.Log("Spawnercontroller reset");
        emptyCells.Clear();
//        start i and j index at 1 instead of 0 and go until second last value of x and y to add a padding of 1 around the grid
        for (int i = backgroundMinX; i < backgroundMaxX; i++)
        {
            for (int j = backgroundMinY; j < backgroundMaxY; j++)
            {
                emptyCells.Add(new Vector2Int(i, j));
            }
        }
    }

    public void AddEmptyCell(Vector3 vector3)
    {
        Vector2Int vector2 = new Vector2Int((int) vector3.x, (int) vector3.y);
        emptyCells.Add(vector2);
    }

    public void RemoveEmptyCell(Vector3 vector3)
    {
        Vector2Int vector2 = new Vector2Int((int) vector3.x, (int) vector3.y);
        emptyCells.Remove(vector2);
    }

    public bool IsEmptyCell(Vector3 vector)
    {
        Vector2Int vector2 = new Vector2Int((int) vector.x, (int) vector.y);
        return emptyCells.Contains(vector2);
    }

    
}
