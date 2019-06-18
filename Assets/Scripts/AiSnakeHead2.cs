using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSnakeHead2 : MonoBehaviour
{
    
    public event Action OnBorderAiSnakeCollision;
    public event Action<bool> OnAiSnakeAteFood;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border") || other.CompareTag("SnakeBody") || other.CompareTag("SnakeHead") || other.CompareTag("AiSnakeHead"))
        {
            OnBorderAiSnakeCollision?.Invoke();
        }
        else if (other.CompareTag("Food"))
        {

//            Destroy(other.gameObject);
            OnAiSnakeAteFood?.Invoke(true);
        }

    }
    
}
