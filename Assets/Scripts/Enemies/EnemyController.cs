using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    private CharacterAnimator characterAnimatorHead;
    private CharacterAnimator characterAnimatorBody;
    private Direction moveDirection = Direction.Down;
    
    [SerializeField] private float maxChaseSpeed = 3.0f;
    [SerializeField] private float minChaseSpeed = 1.0f;
    [SerializeField] private Health health;
    
    [SerializeField] private RenderInfo[] headRender;
    [SerializeField] private RenderInfo[] bodyRender;
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private float fps = 10.0f;
    [SerializeField] protected float damageValue = 10;

    private float chaseSpeed = 3.0f;
    private bool isChasing = false;
    private bool turnBased = false;

    [SerializeField] protected Bullet bulletPrefab;
    protected bool canMove = true;
    protected PlayerController player;
    protected bool isMoving = false;
    

    public bool isDead => health.isDead;

    protected virtual void Awake()
    {
        player = GameManager.Instance.Player;
        isChasing = false;
        canMove = true;
        characterAnimatorHead = new CharacterAnimator();
        characterAnimatorBody = new CharacterAnimator();
        characterAnimatorHead.Init(headRenderer, (int) fps);
        characterAnimatorBody.Init(bodyRenderer, (int) fps);
    }
    
    public void StartChasing()
    {
        isChasing = true;
        turnBased = false;
        chaseSpeed = Random.Range(minChaseSpeed, maxChaseSpeed);
    }
    
    public void StartTurnBased()
    {
        isChasing = false;
        turnBased = true;
    }

    protected abstract void Move();

    protected virtual void Shoot()
    {
        var bullet = Instantiate<Bullet>(bulletPrefab, transform.position, Quaternion.identity);
        bullet.direction = (player.transform.position - transform.position).normalized;
        bullet.Shoot();
    }
    
    protected virtual void Update()
    {
        if (isChasing)
        {
            Chase(chaseSpeed);
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
        AnimationUpdate();
    }
    private void AnimationUpdate()
    {
        characterAnimatorHead.Update(headRender, moveDirection, isMoving);
        characterAnimatorBody.Update(bodyRender, moveDirection, isMoving);
    }
    
    protected virtual void Chase(float moveSpeed)
    {
        if (!canMove)
        {
            return;
        }
        var direction = player.transform.position - transform.position;
        direction = direction.normalized;
        isMoving = true;

        if (direction.x > 0)
        {
            moveDirection = Direction.Right;
        }
        else if (direction.x < 0)
        {
            moveDirection = Direction.Left;
        }
        else if (direction.y > 0)
        {
            moveDirection = Direction.Up;
        }
        else if (direction.y < 0)
        {
            moveDirection = Direction.Down;
        }
        transform.Translate(direction * Time.deltaTime * moveSpeed);
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
