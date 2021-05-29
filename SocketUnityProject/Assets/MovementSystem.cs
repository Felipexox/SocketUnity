using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem
{
    public Vector3 Position;

    private Vector3 Direction;

    private float Velocity;
    
    public void Tick(float deltaTime)
    {
        UpdatePosition(deltaTime);
    }


    private void UpdatePosition(float deltaTime)
    {
        Position += Direction * Velocity * deltaTime;
    }
    
    public void Move(Vector3 dir, float velocity)
    {
        Direction = dir;
        Velocity = velocity;
    }

}
