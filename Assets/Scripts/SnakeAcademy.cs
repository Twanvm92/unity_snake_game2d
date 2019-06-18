using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class SnakeAcademy : Academy
{
	private SnakeAgent2 snakeAgent2;
	private SnakeAgent snakeAgent;
	
	public override void InitializeAcademy()
	{
		Monitor.SetActive(true);
	}

	public override void AcademyReset()
	{
		Debug.Log("Snake academy reset!");
		snakeAgent.snakeController.spawnerController.Reset();
		snakeAgent.snakeController.foodController.Reset();
		
		base.AcademyReset();
		
		ISnakeController.food = snakeAgent.snakeController.foodController.InitializeFood();
	}

	public void Initialize(SnakeAgent snakeAgent, SnakeAgent2 snakeAgent2)
	{
		this.snakeAgent = snakeAgent;
		this.snakeAgent2 = snakeAgent2;
	}
}
