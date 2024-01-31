
using System;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
[Serializable]
public struct RenderInfo
{
    public Sprite[] animationFrames;
    public Direction direction;
    public bool flipX;
}
public class CharacterAnimator : AnimationEntity
{
    private SpriteRenderer spriteRenderer;
    private int fps;
    public void Init(SpriteRenderer _spriteRenderer, int _fps)
    {
        spriteRenderer = _spriteRenderer;
        fps = _fps;
    }

    public void Update(RenderInfo[] renderInfos, Direction direction, bool isMoving)
    {
        foreach (var renderInfo in renderInfos)
        {
            if (renderInfo.direction == direction)
            {
                if (isMoving)
                {
                    spriteRenderer.sprite = renderInfo.animationFrames[(int) (Time.time * fps) % renderInfo.animationFrames.Length];
                }
                else
                {
                    spriteRenderer.sprite = renderInfo.animationFrames[0];
                }
                spriteRenderer.flipX = renderInfo.flipX;
            }
        }
    }
}