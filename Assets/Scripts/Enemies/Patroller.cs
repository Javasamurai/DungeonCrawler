using UnityEngine;

public class Patroller : EnemyController
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float moveDistance = 3.0f;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Move()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        if (transform.position.x > moveDistance)
        {
            moveSpeed = -moveSpeed;
        }
        else if (transform.position.x < -moveDistance)
        {
            moveSpeed = -moveSpeed;
        }
    }

    protected override void Shoot()
    {
        
    }
}
