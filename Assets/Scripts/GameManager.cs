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
    
    private float timer = 0f;

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
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        var powerupsCount = Enum.GetNames(typeof(Powerup)).Length;
        currentPowerup = (Powerup) Random.Range(0, powerupsCount);
        AnnouncePowerup();
        player.transform.position = spawnPoint.position;

        while (timer < 3.0f)
        {
            yield return new WaitForSeconds(1.0f);
            timer++;
            timerText.text = string.Format("{0}", 3 - timer);
        }
        
        timer = 0f;
        timerText.text = string.Empty;
        
        player.enabled = true;
        EnterRoom(currentRoomIndex);
    }

    public void EnterNextRoom()
    {
        ExitRoom();
        currentRoomIndex++;
        StartCoroutine(StartGame());
    }

    private void EnterRoom(int index)
    {
        currentRoom = rooms[index];

        foreach (var enemy in currentRoom.enemies)
        {
            var randomPosition = new Vector3(Random.Range(boundary.bounds.min.x, boundary.bounds.max.x), Random.Range(boundary.bounds.min.y, boundary.bounds.max.y), 0);
            
            var addedEnemy = Instantiate(enemy, randomPosition, Quaternion.identity);
            enemies.Add(addedEnemy);
        }
        
        AquirePowerup();
    }
    
    public bool IsAllEnemiesDead()
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
    }

    private void AquirePowerup()
    {
        player.AquirePowerup(currentPowerup);
        if (currentPowerup == Powerup.MAGNET)
        {
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.StartChasing();
                }
            }
        }
        
        if (currentPowerup == Powerup.TURNBASED)
        {
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.StartTurnBased();
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
}
