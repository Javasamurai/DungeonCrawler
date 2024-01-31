using UnityEngine;

public class Patroller : EnemyController
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float moveDistance = 3.0f;

    private Vector3 initialPosition;
    protected override void Awake()
    {
        base.Awake();
        initialPosition = transform.position;
    }

    protected override void Move()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        var distance = Vector3.Distance(transform.position, initialPosition);
        
        if (distance > moveDistance)
        {
            moveSpeed = -moveSpeed;
        }
        isMoving = true;
    }

    protected override void Shoot()
    {
        var distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 2.0f)
        {
            base.Shoot();
        }
    }
}
