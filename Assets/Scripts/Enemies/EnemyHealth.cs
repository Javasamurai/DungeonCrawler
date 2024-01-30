using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;
    protected override void Die()
    {
        // Show a death animation
        headRenderer.enabled = false;
        if (gameObject.TryGetComponent<Collider2D>(out Collider2D coll))
        {
            coll.enabled = false;
        }
        Destroy(this.gameObject, 3);
    }
}