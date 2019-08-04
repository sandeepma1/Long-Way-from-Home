using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static Action<float> SetPlayerHealth;
    public static Action<float> SetPlayerHunger;
    public static Action<float> SetPlayerThirst;

    private const float healthFrequency = 2;
    private float healthCounter = 0;
    private float playerHealth;
    private float PlayerHealth
    {
        get { return playerHealth; }
        set
        {
            playerHealth = value;
            if (playerHealth <= 0)
            {
                playerHealth = 0;
            }
            SetPlayerHealth?.Invoke(playerHealth);
        }
    }
    private float playerHunger;
    private float PlayerHunger
    {
        get { return playerHunger; }
        set
        {
            playerHunger = value;
            if (playerHunger <= 0)
            {
                playerHunger = 0;
            }
            SetPlayerHunger?.Invoke(playerHunger);
        }
    }
    private float playerThirst;
    private float PlayerThirst
    {
        get { return playerThirst; }
        set
        {
            playerThirst = value;
            if (playerThirst <= 0)
            {
                playerThirst = 0;
            }
            SetPlayerThirst?.Invoke(playerThirst);
        }
    }

    private void Start()
    {
        LoadPlayerStatsData();
        ActionManager.OnPlayerActionDone += OnPlayerActionDone;
        PlayerMovement.OnPlayerMovedPerMeter += OnPlayerMovedPerMeter;
        MasterSave.RequestSaveData += RequestSaveData;
        InvokeRepeating("DecrementHungerIdle", 0, GEM.idlehungerFrequency);
        InvokeRepeating("DecrementThirstIdle", 0, GEM.idlethirstFrequency);
    }

    private void OnDestroy()
    {
        ActionManager.OnPlayerActionDone -= OnPlayerActionDone;
        PlayerMovement.OnPlayerMovedPerMeter -= OnPlayerMovedPerMeter;
        MasterSave.RequestSaveData -= RequestSaveData;
    }

    private void Update()
    {
        if (PlayerHunger <= 0 && PlayerThirst <= 0)
        {
            healthCounter += Time.deltaTime;
            if (healthCounter <= 0)
            {
                healthCounter = healthFrequency;
                if (PlayerHealth >= 0)
                {
                    DecrementHealth();
                }
            }
        }
    }

    #region --Save Load Stuff--
    private void LoadPlayerStatsData()
    {
        PlayerSavePlayerStats playerSavePlayerStats = MasterSave.LoadPlayerStats();
        PlayerHealth = playerSavePlayerStats.health;
        PlayerHunger = playerSavePlayerStats.hunger;
        PlayerThirst = playerSavePlayerStats.thirst;
    }

    private void RequestSaveData()
    {
        MasterSave.SavePlayerStats(new PlayerSavePlayerStats(PlayerHealth, PlayerHunger, PlayerThirst));
    }
    #endregion

    private void OnPlayerActionDone()
    {
        PlayerHunger -= GEM.actionHungerExhaustionLevel;
        PlayerThirst -= GEM.actionThirstExhaustionLevel;
    }

    private void OnPlayerMovedPerMeter()
    {
        PlayerHunger -= GEM.walkingHungerExhaustionLevel;
        PlayerThirst -= GEM.walkingThirstExhaustionLevel;
    }

    private void DecrementHealth()
    {
        PlayerHealth -= GEM.healthExhaustionLevel;
    }

    private void DecrementHungerIdle()
    {
        PlayerHunger -= GEM.idleHungerExhaustionLevel;
    }

    private void DecrementThirstIdle()
    {
        PlayerThirst -= GEM.idleThirstExhaustionLevel;
    }
}