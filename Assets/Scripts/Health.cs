using System;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [NonSerialized] protected float health = 100;
    
    public bool isDead => health <= 0;
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();

    public void Reset()
    {
        health = 100;
    }
}
