using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float shootDelay = 0.5f;
    public float fps = 10.0f;
    
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private RenderInfo[] headRender;
    [SerializeField] private RenderInfo[] bodyRender;
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private Collider2D collider2D;
    public Health health;
    
    private CharacterAnimator characterAnimatorHead;
    private CharacterAnimator characterAnimatorBody;
    
    public bool isMoving;
    private bool isReverseControls;
    private Rigidbody2D rigidbody2D;
    private Vector3 lookDirection = Vector3.up;
    private Direction moveDirection = Direction.Down;
    private float shootTime;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        characterAnimatorHead = new CharacterAnimator();
        characterAnimatorBody = new CharacterAnimator();
        characterAnimatorHead.Init(headRenderer, (int) fps);
        characterAnimatorBody.Init(bodyRenderer, (int) fps);
    }

    private void Update()
    {
        TakeInput();
        AnimationUpdate();
    }
    
    private void TakeInput()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        if (isReverseControls)
        {
            x = -x;
            y = -y;
        }
        
        var shootDirectionX = Input.GetAxis("Fire1");
        var shootDirectionY = Input.GetAxis("Fire2");
        
        transform.Translate(new Vector3(x, y, 0) * (Time.deltaTime * speed));
        
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, collider2D.bounds.min.x, collider2D.bounds.max.x), 
            Mathf.Clamp(transform.position.y, collider2D.bounds.min.y, collider2D.bounds.max.y), 0);
        if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
        {
            if (Time.time - shootTime > shootDelay)
            {
                if (Mathf.Abs(shootDirectionX) > Mathf.Abs(shootDirectionY))
                {
                    moveDirection = shootDirectionX > 0 ? Direction.Right : Direction.Left;
                }
                else
                {
                    moveDirection = shootDirectionY > 0 ? Direction.Up : Direction.Down;
                }
                Shoot();
            }
        }
        isMoving = x != 0 || y != 0;
        if (isMoving)
        {
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                moveDirection = x > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                moveDirection = y > 0 ? Direction.Up : Direction.Down;
            }
        }
    }

    private void AnimationUpdate()
    {
        characterAnimatorHead.Update(headRender, moveDirection, isMoving);
        characterAnimatorBody.Update(bodyRender, moveDirection, isMoving);
    }

    private void Shoot()
    {
        var bullet = Instantiate<Bullet>(bulletPrefab, transform.position, Quaternion.identity);
        
        lookDirection = new Vector3(Input.GetAxis("Fire1"), Input.GetAxis("Fire2"), 0);
        lookDirection = lookDirection.normalized;
        bullet.direction = lookDirection;
        shootTime = Time.time;
        bullet.Shoot(true);
    }

    public void TakeDamage(float damageValue)
    {
        Debug.Log("Player took damage" + damageValue);
        health.TakeDamage(damageValue);
    }
    
    private void ShakeCamera()
    {
        
    }

    public void AquirePowerup(Powerup currentPowerup)
    {
        switch (currentPowerup)
        {
            case Powerup.BIGG:
                transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y * 2, 1);
                break;
            case Powerup.TIME:
                Time.timeScale = 1.5f;
                break;
            case Powerup.DOT:
                bulletPrefab.transform.localScale = new Vector3(.5f, .5f, 1);
                break;
            case Powerup.MISDIRECT:
                isReverseControls = true;
                break;
            default:
                Debug.Log("No powerup found");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            if (GameManager.Instance.IsAllEnemiesDead())
            {
                GameManager.Instance.EnterNextRoom();
                ResetPlayer();
            }
        }
    }
    
    public void ResetPlayer()
    {
        transform.localScale = new Vector3(2, 2, 1);
        Time.timeScale = 1;
        isReverseControls = false;
        bulletPrefab.transform.localScale = Vector3.one;
    }

    public void Knockback(Vector3 transformPosition)
    {
        var direction = transform.position - transformPosition;
        rigidbody2D.AddForce(direction * 10, ForceMode2D.Impulse);
        StartCoroutine(ResetVelocity());
    }
    
    IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(0.5f);
        rigidbody2D.velocity = Vector2.zero;
    }
}
