using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Vector2 throwerPosition;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 throwerPosition, Vector2 direction, float force)
    {
        this.throwerPosition = throwerPosition;
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //we also add a debug log to know what the projectile touch
        Debug.Log("Projectile Collision with " + other.gameObject);
        var bot = other.collider.GetComponent<EnemyController>();
        if (bot != null)
        {
            bot.Fix();
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        var distance = (rigidbody2d.position - this.throwerPosition).sqrMagnitude;
        Debug.Log(distance);
        if (distance > 100.0f)
        {
            Destroy(gameObject);
        }
    }
}
