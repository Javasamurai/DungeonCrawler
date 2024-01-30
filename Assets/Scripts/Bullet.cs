using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    public Vector3 direction;
    private bool active;

    private void Start()
    {
        Destroy(gameObject, 5);
    }
    
    private void Update()
    {
        if (!active) return;
        
        transform.Translate(direction * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bullet collided with " + other.gameObject.name);
        var enemyController = other.gameObject.GetComponent<EnemyController>();
        
        if (enemyController != null)
        {
            enemyController.DealDamage(10);
            Destroy(gameObject);
        }
    }

    public void Shoot()
    {
        active = true;
    }
}