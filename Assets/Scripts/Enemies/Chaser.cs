using UnityEngine;
using Random = UnityEngine.Random;

public class Chaser : EnemyController
{
    [SerializeField] float maxMoveSpeed = 3.0f;
    [SerializeField] float minMoveSpeed = 1.0f;
    private float moveSpeed = 5.0f;

    protected override void Awake()
    {
        base.Awake();
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }

    protected override void Move()
    {
        Chase(moveSpeed);
    }

    protected override void Shoot()
    {
    }
}
