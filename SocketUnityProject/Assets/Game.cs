using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public MovementSystem[] _movementSystems;
    
    public void SetupServer(int playerNum)
    {
        _movementSystems = new MovementSystem[playerNum];
        for (int i = 0; i < _movementSystems.Length; i++)
        {
            _movementSystems[i] = new MovementSystem();
        }
    }
    
    public void TickServer(float deltaTime)
    {
        for (int i = 0; i < _movementSystems.Length; i++)
        {
            _movementSystems[i].Tick(deltaTime);
        }
    }
    
}
