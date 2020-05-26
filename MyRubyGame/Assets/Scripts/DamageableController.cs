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

    public ParticleSystem hitEffect;
    public ParticleSystem healthEffect;

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
            hitEffect?.Play();
            OnDead();
        } else if (actualDamage < 0) {
            hitEffect?.Play();
            OnTakeHit(newHealth);
        } else if (newHealth == MaxHealth) {
            if (newHealth > currentHealth) {
                GainHealthAnimation();
            }
            OnFullHealth();
        } else if (actualDamage > 0) {
            GainHealthAnimation();
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

    private void GainHealthAnimation() {
        // healthEffect?.Play();
        var particles = Instantiate(
            healthEffect,
            transform.position,
            Quaternion.identity);
        particles?.Play();
    }

    protected virtual void OnTakeHit(float newHealth) {}
    protected virtual void OnDead() {}
    protected virtual void OnGainHealth(float newHealth) {}
    protected virtual void OnFullHealth() {}
}
