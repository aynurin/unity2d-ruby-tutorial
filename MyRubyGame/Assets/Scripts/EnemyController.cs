using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : DamageableController
{
    public bool broken = true;
    public int maxHealth = 10;
    protected override int MaxHealth { get => maxHealth; }

    Rigidbody2D rigidbody2d;

    public bool moveVertical = true;

    float horizontal;
    float vertical;

    public float moveSpeed = 3f;
    public float damageValue = 10f;
    public float moveRange = 2f;

    float currentDirection = 1f;
    Vector2 startPosition;

    Animator animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rigidbody2d = GetComponent<Rigidbody2D>();
        startPosition = rigidbody2d.position;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }
        if (moveVertical)
        {
            MoveVertical();
        }
        else
        {
            MoveHorizontal();
        }
    }

    void MoveVertical()
    {
        Vector2 position = rigidbody2d.position;
        var range = moveRange / 2;
        if (position.y >= startPosition.y + range)
        {
            currentDirection = -moveSpeed / Math.Abs(moveSpeed);
        }
        else if (position.y <= startPosition.y - range)
        {
            currentDirection = moveSpeed / Math.Abs(moveSpeed);
        }
        animator.SetFloat("Move X", 0);
        animator.SetFloat("Move Y", currentDirection * moveSpeed);
        position.y = position.y + currentDirection * moveSpeed * Time.fixedDeltaTime;
        rigidbody2d.MovePosition(position);
    }

    void MoveHorizontal()
    {
        Vector2 position = rigidbody2d.position;
        var range = moveRange / 2;
        if (position.x >= startPosition.x + range)
        {
            currentDirection = -moveSpeed / Math.Abs(moveSpeed);
        }
        else if (position.x <= startPosition.x - range)
        {
            currentDirection = moveSpeed / Math.Abs(moveSpeed);
        }
        animator.SetFloat("Move Y", 0);
        animator.SetFloat("Move X", currentDirection * moveSpeed);
        position.x = position.x + currentDirection * moveSpeed * Time.fixedDeltaTime;
        rigidbody2d.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-damageValue, false);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
    }
}
