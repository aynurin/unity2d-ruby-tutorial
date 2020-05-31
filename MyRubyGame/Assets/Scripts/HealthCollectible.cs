using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip collectedClip;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null && controller.ChangeHealth(1, false))
        {
            Destroy(gameObject);
            controller.PlaySound(collectedClip);
        }
    }
}
