using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI powerupText;
    [SerializeField]
    private Animator powerupAnimator;

    [SerializeField]
    PlayerController player;
    [SerializeField] RoomConfig[] rooms;
    
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
        EnterRoom(0);
    }
    
    public void EnterNextRoom()
    {
        ExitRoom();
        currentRoomIndex++;
        EnterRoom(currentRoomIndex);
    }

    private void EnterRoom(int index)
    {
        currentRoom = rooms[index];
        var powerupsCount = Enum.GetNames(typeof(Powerup)).Length;

        currentPowerup = (Powerup) Random.Range(0, powerupsCount);

        currentPowerup = Powerup.TURNBASED;

        foreach (var enemy in currentRoom.enemies)
        {
            var randomPosition = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
            
            var addedEnemy = Instantiate(enemy, randomPosition, Quaternion.identity);
            enemies.Add(addedEnemy);
        }
        
        AnnouncePowerup();
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
    }
}
