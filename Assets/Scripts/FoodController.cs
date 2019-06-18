using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    private SpawnerController spawnerController;

    private GameObject food;
    // Start is called before the first frame update

    public GameObject InitializeFood()
    {
        if (food == null)
        {
            food = spawnerController.SpawnFood(gameObject);
        }
        return food;
    }

    public void Initialize(SpawnerController spawnerController)
    {
        this.spawnerController = spawnerController;
    }
    

//    TODO does this really reset the current food object?
    public void Reset()
    {
        Debug.Log("foodcontroller reset");
        Destroy(food);
        food = null;
    }
}
