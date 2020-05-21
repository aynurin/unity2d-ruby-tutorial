using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageableController : MonoBehaviour
{
    protected abstract int MaxHealth { get; }

    float currentHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = MaxHealth;
    }

    public bool ChangeHealth(float amount, bool continued)
    {
        float actualDamage = amount;
        if (continued) {
            actualDamage = actualDamage * Time.deltaTime;
        }
        var newHealth = Mathf.Clamp(currentHealth + actualDamage, 0, MaxHealth);
        if (newHealth == 0) {
            OnDead();
        } else if (actualDamage < 0) {
            OnTakeHit(newHealth);
        } else if (newHealth == MaxHealth) {
            OnFullHealth();
        } else if (actualDamage > 0) {
            OnGainHealth(newHealth);
        }
        Debug.Log(String.Format("{4}: {3}, {2} - {0}/{1}", newHealth, MaxHealth, 
            actualDamage, amount, this.GetType().Name));
        if (newHealth != currentHealth) {
            currentHealth = newHealth;
            return true;
        } else {
            return false;
        }
    }

    protected virtual void OnTakeHit(float newHealth) {}
    protected virtual void OnDead() {}
    protected virtual void OnGainHealth(float newHealth) {}
    protected virtual void OnFullHealth() {}
}
