using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] Collider2D boundary;
    [SerializeField]
    private TextMeshProUGUI powerupText;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private Animator powerupAnimator;
    [SerializeField]
    private GamePlayStatePanel[] gameplayStatePanels;
    private float timer;
    private GameplayState gameplayState = GameplayState.MENU;
    private float timeLasted;
    private float lastTimeScale = 1;
    private bool won = false;
    
    public bool Won
    {
        get { return won; }
    }

    [SerializeField]
    PlayerController player;
    [SerializeField] RoomConfig[] rooms;
    [SerializeField] Transform spawnPoint;
    
    private RoomConfig currentRoom;
    private int currentRoomIndex = 0;
    
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public float TimeLasted
    {
        get { return timeLasted; }
    }

    private List<EnemyController> enemies = new List<EnemyController>();
    private Powerup currentPowerup = Powerup.BIGG;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;    
        }
    }

    void Start()
    {
        player.enabled = false;
        SwitchState(GameplayState.MENU);
    }

    private void Update()
    {
        if (gameplayState == GameplayState.GAMEPLAY)
        {
            timeLasted += Time.deltaTime;
            if (Input.GetButtonUp("Cancel"))
            {
                lastTimeScale = Time.timeScale;
                PauseGame();
                return;
            }
        }
        if (gameplayState == GameplayState.PAUSE && Input.GetButtonUp("Cancel"))
        {
            ResumeGame();
        }
    }
    
    public void BeginGame()
    {
        won = false;
        currentRoomIndex = 0;
        ExitRoom();
        SwitchState(GameplayState.WARMUP);
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        var powerupsCount = Enum.GetNames(typeof(Powerup)).Length;
        
        player.transform.position = spawnPoint.position;
        player.Reset();
        currentPowerup = (Powerup) Random.Range(0, powerupsCount);
        AnnouncePowerup();
        
        while (timer < 3.0f)
        {
            yield return new WaitForSeconds(1.0f);
            timer++;
            timerText.text = string.Format("{0}", 3 - timer);
        }
        timer = 0f;
        SwitchState(GameplayState.GAMEPLAY);

        timerText.text = string.Empty;
        if (currentRoomIndex == 0)
        {
            timeLasted = 0f;
        }
        player.enabled = true;
        EnterRoom(currentRoomIndex);
    }

    private void SwitchState(GameplayState state)
    {
        // There is obviously better ways to do this!
        gameplayState = state;
        Debug.Log("Switching to " + gameplayState);

        switch (gameplayState)
        {
            case GameplayState.MENU:
                ShowPanelsExcept(new[] {GameplayState.MENU});
                break;
            case GameplayState.WARMUP:
                ShowPanelsExcept(new[] {GameplayState.GAMEPLAY});
                break;
            case GameplayState.GAMEPLAY:
                ShowPanelsExcept(new[] {GameplayState.GAMEPLAY});
                break;
            case GameplayState.GAMEOVER:
                ShowPanelsExcept(new[] {GameplayState.GAMEPLAY, GameplayState.GAMEOVER});
                break;
            case GameplayState.PAUSE:
                ShowPanelsExcept(new[] {GameplayState.GAMEPLAY, GameplayState.PAUSE});
                break;
            default:
                Debug.LogWarning("Unknown gameplay state");
                break;
        }
    }
    
    private void PauseGame()
    {
        Time.timeScale = 0f;
        SwitchState(GameplayState.PAUSE);
    }
    
    private void ResumeGame()
    {
        Time.timeScale = lastTimeScale;
        SwitchState(GameplayState.GAMEPLAY);
    }

    private void ShowPanelsExcept(GameplayState[] states)
    {
        Array.ForEach(gameplayStatePanels,
            (panel) =>
            {
                if (states != null && Array.Exists(states, state => state == panel.state))
                {
                    panel.panel.SetActive(true);
                }
                else
                {
                    panel.panel.SetActive(false);
                }
            });
    }

    public void EnterNextRoom()
    {
        ExitRoom();
        currentRoomIndex++;
        if (currentRoomIndex >= rooms.Length)
        {
            Debug.LogWarning("No more rooms!");
            won = true;
            GameOver();
            return;
        }
        StartCoroutine(StartGame());
    }

    private void EnterRoom(int index)
    {
        currentRoom = rooms[index];

        foreach (var enemy in currentRoom.enemies)
        {
            var randomPosition = new Vector3(Random.Range(boundary.bounds.min.x + 1, boundary.bounds.max.x - 1), Random.Range(boundary.bounds.min.y + 1, boundary.bounds.max.y - 1), 0);
            
            var addedEnemy = Instantiate(enemy, randomPosition, Quaternion.identity);
            enemies.Add(addedEnemy);
        }
        
        AquirePowerup();
    }
    
    public bool AreAllEnemiesDead()
    {
        bool allEnemiesDead = true;
        foreach (var enemy in enemies)
        {
            if (enemy != null && !enemy.isDead)
            {
                allEnemiesDead = false;
            }
        }

        return allEnemiesDead;
    }

    private void AnnouncePowerup()
    {
        powerupText.text = string.Format("Curse unlocked: {0}", currentPowerup);
        powerupAnimator.SetTrigger("Show");
        AudioManager.Instance.PlaySFX(SoundType.POWER_UP);
    }

    private void AquirePowerup()
    {
        player.AquirePowerup(currentPowerup);
        if (currentPowerup == Powerup.MAGNET || currentPowerup == Powerup.TURNBASED)
        {
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    if (currentPowerup == Powerup.MAGNET)
                    {
                        enemy.StartChasing();
                    }
                    else
                    {
                        enemy.StartTurnBased();
                    }
                }
            }
        }
    }
    
    private void ExitRoom()
    {
        // Destroy all enemies
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
        enemies.Clear();
        // Disable player
        player.enabled = false;
    }

    public void GameOver(bool win = false)
    {
        player.enabled = false;
        // Kill all enemies
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.Die();
            }
        }
        AudioManager.Instance.PlaySFX(SoundType.GAME_END);
        SwitchState(GameplayState.GAMEOVER);
    }
}

[Serializable]
struct GamePlayStatePanel
{
    public GameplayState state;
    public GameObject panel;
}
