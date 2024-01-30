using System;
using UnityEngine;

public class Chaser : EnemyController
{
    [SerializeField] float moveSpeed = 5.0f;
    protected override void Move()
    {
        var direction = player.transform.position - transform.position;
        direction = direction.normalized;
        transform.Translate(direction * Time.deltaTime * moveSpeed);
    }

    protected override void Shoot()
    {
    }
}
