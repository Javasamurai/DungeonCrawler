using UnityEngine;

public class Patroller : EnemyController
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float moveDistance = 3.0f;
    
    private const float SHOOT_RANGE = 2.0f;


    private Vector3 initialPosition;
    protected override void Awake()
    {
        base.Awake();
        initialPosition = transform.position;
    }

    protected override void Move()
    {
        if (!canMove)
        {
            return;
        }
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, initialPosition);
        
        if (distance > moveDistance)
        {
            moveSpeed = -moveSpeed;
        }
        isMoving = true;
    }

    protected override void Shoot()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < SHOOT_RANGE)
        {
            base.Shoot();
        }
    }
}
