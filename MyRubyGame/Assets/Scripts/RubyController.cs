using System;
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
    public GameObject projectilePrefab;

    AudioSource audioSource;
    public AudioClip projectileThrownClip;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        Debug.Log(String.Format("Now Ruby will play {0} by {1}", clip, audioSource));
        audioSource.PlayOneShot(clip);
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

        if (Input.GetButtonDown("Fire1"))
        {
            LaunchProjectile();
            PlaySound(projectileThrownClip);
        }

        // if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                if (hit.collider != null)
                {
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        character.DisplayDialog();
                    }
                }
            }
        }
    }

    void LaunchProjectile()
    {
        GameObject projectileObject = Instantiate(
            projectilePrefab,
            rigidbody2d.position + Vector2.up * 0.5f,
            Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(rigidbody2d.position, lookDirection, 300);

        animator.SetTrigger("Launch");
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        // position = position + moveSpeed * move * TimeoutException.fixedDeltaTime;
        position.x = position.x + moveSpeed * horizontal * Time.fixedDeltaTime;
        position.y = position.y + moveSpeed * vertical * Time.fixedDeltaTime;
        rigidbody2d.MovePosition(position);
    }

    protected override void OnTakeHit(float newHealth)
    {
        animator.SetTrigger("Hit");
        UIHealthBar.instance.SetValue(newHealth / maxHealth);
    }

    protected override void OnDead()
    {
        animator.SetTrigger("Hit");
        UIHealthBar.instance.SetValue(0);
    }

    protected override void OnGainHealth(float newHealth)
    {
        UIHealthBar.instance.SetValue(newHealth / maxHealth);
    }

    protected override void OnFullHealth()
    {
        UIHealthBar.instance.SetValue(1);
    }
}
