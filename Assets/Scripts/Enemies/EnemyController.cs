using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] protected float damageValue = 10;
    [SerializeField] protected Bullet bulletPrefab;
    [SerializeField] private Health health;
    public bool isDead => health.isDead;
    protected bool canMove = true;
    private bool isChasing = false;
    protected bool turnBased = false;
    protected PlayerController player;
    
    protected virtual void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        isChasing = false;
        canMove = true;
    }
    
    public void StartChasing()
    {
        isChasing = true;
        turnBased = false;
    }
    
    public void StartTurnBased()
    {
        isChasing = false;
        turnBased = true;
    }

    protected abstract void Move();
    protected abstract void Shoot();

    protected virtual void Update()
    {
        if (isChasing)
        {
            Chase();
        }
        else
        {
            Move();
        }
        canMove = true;
        if (turnBased)
        {
            canMove = player.isMoving;
        }
    }
    
    protected virtual void Chase()
    {
        if (!canMove)
        {
            return;
        }
        var direction = player.transform.position - transform.position;
        direction = direction.normalized;
        transform.Translate(direction * Time.deltaTime * 5);
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
