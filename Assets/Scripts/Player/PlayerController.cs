using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [FormerlySerializedAs("playerConfig")]
    [Header("Configs")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PowerupConfig powerupConfig;

    [Header("Animation")]

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private RenderInfo[] headRender;
    [SerializeField] private RenderInfo[] bodyRender;
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private Collider2D collider2D;
    [SerializeField] private Rigidbody2D rigidbody2D;

    public Health health;
    
    private CharacterAnimator characterAnimatorHead;
    private CharacterAnimator characterAnimatorBody;
    
    public bool isMoving;
    private float bulletScale = 1;
    private float shootDelay = 0.5f;
    private bool isReverseControls;
    private Vector3 lookDirection = Vector3.up;
    private Direction moveDirection = Direction.Down;
    private float shootTime;
    private Camera mainCamera;
    
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string FIRE1 = "Fire1";
    private const string FIRE2 = "Fire2";
    
    private const float LOCAL_SCALE = 2f;
    private const float VELOCITY_RESET_DELAY = 1.5f;
    
    private void Awake()
    {
        characterAnimatorHead = new CharacterAnimator();
        characterAnimatorBody = new CharacterAnimator();
        characterAnimatorHead.Init(headRenderer, (int) playerData.fps);
        characterAnimatorBody.Init(bodyRenderer, (int) playerData.fps);
        mainCamera = Camera.main;
        shootDelay = playerData.shootDelay;
    }

    private void Update()
    {
        TakeInput();
        AnimationUpdate();
    }
    
    private void TakeInput()
    {
        Fire();        
        Move();
    }
    
    private void Fire()
    {
        float shootDirectionX = Input.GetAxis(FIRE1);
        float shootDirectionY = Input.GetAxis(FIRE2);

        if (Input.GetButton(FIRE1) || Input.GetButton(FIRE2))
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
    }

    private void Move()
    {
        var x = Input.GetAxis(HORIZONTAL);
        var y = Input.GetAxis(VERTICAL);
        if (isReverseControls)
        {
            x = -x;
            y = -y;
        }
        transform.Translate(new Vector3(x, y, 0) * (Time.deltaTime * playerData.speed));
        
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, collider2D.bounds.min.x, collider2D.bounds.max.x), 
            Mathf.Clamp(transform.position.y, collider2D.bounds.min.y, collider2D.bounds.max.y), 0);
        
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
        
        lookDirection = new Vector3(Input.GetAxis(FIRE1), Input.GetAxis(FIRE2), 0);
        lookDirection = lookDirection.normalized;
        bullet.direction = lookDirection;
        shootTime = Time.time;
        bullet.Shoot( bulletScale, true);
    }

    public void TakeDamage(float damageValue)
    {
        Debug.Log("Player took damage" + damageValue);
        health.TakeDamage(damageValue);
    }
    
    private void ShakeCamera()
    {
        mainCamera.GetComponent<CameraShake>().Shake();
    }

    public void AquirePowerup(Powerup currentPowerup)
    {
        switch (currentPowerup)
        {
            case Powerup.BIGG:
                transform.localScale = new Vector3(transform.localScale.x * powerupConfig.bigScale, transform.localScale.y * powerupConfig.bigScale, Vector3.one.z);
                break;
            case Powerup.TIME:
                Time.timeScale = powerupConfig.timeScale;
                break;
            case Powerup.DOT:
                bulletScale = powerupConfig.bulletScale;
                shootDelay = powerupConfig.shootDelay;
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
            if (GameManager.Instance.AreAllEnemiesDead())
            {
                AudioManager.Instance.PlaySFX(SoundType.DOOR_OPEN);
                GameManager.Instance.EnterNextRoom();
                ResetPlayer();
            }
        }
    }
    
    private void ResetPlayer()
    {
        transform.localScale = new Vector3(LOCAL_SCALE, LOCAL_SCALE, Vector3.one.z);
        Time.timeScale = 1;
        isReverseControls = false;
        bulletPrefab.transform.localScale = Vector3.one;
        bulletScale = 1;
    }

    public void Knockback(Vector3 transformPosition)
    {
        var direction = transform.position - transformPosition;
        rigidbody2D.AddForce(direction * 10, ForceMode2D.Impulse);
        StartCoroutine(ResetVelocity());
    }
    
    IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(VELOCITY_RESET_DELAY);
        rigidbody2D.velocity = Vector2.zero;
    }

    public void Reset()
    {
        ResetPlayer();
        health.Reset();
    }
}
