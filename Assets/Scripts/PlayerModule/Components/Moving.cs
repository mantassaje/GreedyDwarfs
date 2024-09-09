using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Moving : MonoBehaviour
{
    public float MaxSpeed = 25;
    public float VerticalLimit = 50;
    public float HorizontalLimit = 50;

    public Rigidbody2D Body { get; private set; }

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Call in  FixedUpdate.
    /// </summary>
    public void MoveRight()
    {
        Rigidbody2DHelper.MoveRight(Body, MaxSpeed);
    }

    /// <summary>
    /// Call in  FixedUpdate.
    /// </summary>
    public void MoveLeft()
    {
        Rigidbody2DHelper.MoveLeft(Body, MaxSpeed);
    }

    /// <summary>
    /// Call in  FixedUpdate.
    /// </summary>
    public void MoveTop()
    {
        Rigidbody2DHelper.MoveTop(Body, MaxSpeed);
    }

    /// <summary>
    /// Call in  FixedUpdate.
    /// </summary>
    public void MoveBottom()
    {
        Rigidbody2DHelper.MoveBottom(Body, MaxSpeed);
    }
}
