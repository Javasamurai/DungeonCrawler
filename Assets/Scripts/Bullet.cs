using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    public Vector3 direction;
    private bool active;
    private bool isPlayerBullet;
    private float scale = 1;

    private void Start()
    {
        Destroy(gameObject, 5);
    }
    
    private void Update()
    {
        if (!active) return;
        
        transform.Translate(direction * Time.deltaTime * speed * scale);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemyController = other.gameObject.GetComponent<EnemyController>();
        var playerController = other.gameObject.GetComponent<PlayerController>();
        
        if (enemyController != null && isPlayerBullet)
        {
            enemyController.DealDamage(10);
            Destroy(gameObject);
        }
        else if (playerController != null && !isPlayerBullet)
        {
            playerController.TakeDamage(10);
            Destroy(gameObject);
        }
    }

    public void Shoot(float _scale = 1, bool _isPlayerBullet = false)
    {
        this.isPlayerBullet = _isPlayerBullet;
        this.scale = _scale;
        transform.localScale = new Vector3(scale, scale, 1);
        active = true;
        AudioManager.Instance.PlaySFX(SoundType.PLAYER_ATTACK);
    }
}