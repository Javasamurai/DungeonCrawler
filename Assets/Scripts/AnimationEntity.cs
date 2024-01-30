using UnityEngine;

public interface AnimationEntity
{
    public void Init(SpriteRenderer spriteRenderer, int fps);
    public void Update(RenderInfo[] renderInfo, Direction direction, bool isMoving);
}
