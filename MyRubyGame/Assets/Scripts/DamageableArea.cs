using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableArea : MonoBehaviour
{
    public float damageValue = 1f;
    public AudioClip damageClip;

    // Start is called before the first frame update
    void OnTriggerStay2D(Collider2D other)
    {
        var damageBearer = other.GetComponent<DamageableController>();

        if (damageBearer != null)
        {
            damageBearer.ChangeHealth(-damageValue, true);
        }

        var ruby = other.GetComponent<RubyController>();

        if (ruby != null) {
            ruby.PlaySound(damageClip);
        }
    }
}
