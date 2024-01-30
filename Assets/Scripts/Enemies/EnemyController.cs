using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] protected float damageValue = 10;
    [SerializeField] protected Bullet bulletPrefab;
    [SerializeField] private Health health;
    public bool isDead => health.isDead;
    protected PlayerController player;
    
    protected virtual void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }
    
    protected abstract void Move();
    protected abstract void Shoot();

    protected virtual void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DoDamage(damageValue);
        }
    }

    public virtual void DealDamage(float damage)
    {
        health.TakeDamage(damage);
        if (isDead)
        {
            Die();
        }
    }

    public virtual void DoDamage(float damage)
    {
        player.TakeDamage(damageValue);
        player.Knockback(transform.position);
    }

    public virtual void Die()
    {
        Debug.Log("Enemy died");
        enabled = false;
    }
}
