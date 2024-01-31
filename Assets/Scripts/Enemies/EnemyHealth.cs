using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        var randomValue = Random.Range(0, 2);
        AudioManager.Instance.PlaySFX(randomValue == 0 ? SoundType.PLAYER_HURT_1 : SoundType.PLAYER_HURT_2);
    }
    protected override void Die()
    {
        // Show a death animation
        headRenderer.enabled = false;
        if (gameObject.TryGetComponent<Collider2D>(out Collider2D coll))
        {
            coll.enabled = false;
        }
        AudioManager.Instance.PlaySFX(SoundType.PLAYER_EXPLODE);
        Destroy(this.gameObject, 3);
    }
}