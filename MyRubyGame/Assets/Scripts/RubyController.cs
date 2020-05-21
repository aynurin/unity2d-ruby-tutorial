﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : DamageableController
{
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public int maxHealth = 5;
    protected override int MaxHealth { get => maxHealth; }

    Rigidbody2D rigidbody2d;

    float horizontal;
    float vertical;
    Vector2 move;

    public float moveSpeed = 3f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        // position = position + moveSpeed * move * TimeoutException.fixedDeltaTime;
        position.x = position.x + moveSpeed * horizontal * Time.fixedDeltaTime;
        position.y = position.y + moveSpeed * vertical * Time.fixedDeltaTime;
        rigidbody2d.MovePosition(position);
    }

    protected override void OnTakeHit(float newHealth) {
        animator.SetTrigger("Hit");
    }

    protected virtual void OnDead() {
        animator.SetTrigger("Hit");
    }
}