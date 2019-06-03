using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{

    private Background background;
    private HashSet<Vector2Int> emptyCells = new HashSet<Vector2Int>();



    public event Action<GameObject> OnSnakeHeadSpawned;

    public event Action<GameObject> OnSnakeBodyPartSpawned;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnSnakeHead(GameObject gameObject, Vector3 vector3)
    {
        if (IsNewGridPositionValid(vector3))
        {
            Vector2Int vector2 = new Vector2Int((int) vector3.x, (int) vector3.y);
            GameObject snakeHead = Instantiate(gameObject, vector3, Quaternion.identity);
            emptyCells.Remove(vector2);
            OnSnakeHeadSpawned?.Invoke(snakeHead);
            
        }
        
    }

    public void SpawnSnakeBodyPart(GameObject gameObject, Vector3 vector3)
    {
        if (IsNewGridPositionValid(vector3))
        {
            Vector2Int vector2 = new Vector2Int((int) vector3.x, (int) vector3.y);
            GameObject snakeBodyPart = Instantiate(gameObject, vector3, Quaternion.identity);
            emptyCells.Remove(vector2);
            OnSnakeBodyPartSpawned?.Invoke(snakeBodyPart);
            
        }
    }

    public void Initialize(Background background)
    {
        this.background = background;
        int borderOffset = 1;
        int backgroundMinX = (int) this.background.GetComponent<SpriteRenderer>().bounds.min.x + borderOffset;
        int backgroundMaxX = (int) this.background.GetComponent<SpriteRenderer>().bounds.max.x;
        int backgroundMinY = (int) this.background.GetComponent<SpriteRenderer>().bounds.min.y + borderOffset;
        int backgroundMaxY = (int) this.background.GetComponent<SpriteRenderer>().bounds.max.y;
        
        Debug.Log("Initialize empty cells");
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

    
//    TODO change to use colliders for border?
    public bool IsNewGridPositionValid(Vector3 vector3)
    {
        Vector2Int vector2 = new Vector2Int((int) vector3.x, (int) vector3.y);
        return emptyCells.Contains(vector2);
    }
}
