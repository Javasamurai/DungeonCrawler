using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSprites : MonoBehaviour
{
    public Sprite[] sprites;
    public float framesPerSecond = 10f;
    public bool loop = true;
    public SpriteRenderer spriteRenderer;
    private float spriteTimer;
    private int spriteIndex;

    void Update()
    {
        if (spriteIndex == sprites.Length - 1 && !loop)
        {
            return;
        }
        NextSprite();
    }

    private void NextSprite()
    {
        spriteIndex = (int)(Time.time * framesPerSecond);
        spriteIndex = spriteIndex % sprites.Length;
        spriteRenderer.sprite = sprites[spriteIndex];
    }
}