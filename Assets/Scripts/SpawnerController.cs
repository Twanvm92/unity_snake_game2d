using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
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

    public void SpawnSnakeHead(GameObject gameObject, Vector3 vector)
    {
        GameObject snakeHead = Instantiate(gameObject, vector, Quaternion.identity);
        OnSnakeHeadSpawned?.Invoke(snakeHead);
    }

    public void SpawnSnakeBodyPart(GameObject gameObject, Vector3 vector)
    {
        GameObject snakeBodyPart = Instantiate(gameObject, vector, Quaternion.identity);
        OnSnakeBodyPartSpawned?.Invoke(snakeBodyPart);
        
    }
}
