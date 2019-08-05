using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static Action<float> SetPlayerHealthOnUi;
    public static Action<float> SetPlayerHungerOnUi;
    public static Action<float> SetPlayerThirstOnUi;
    public static Action<float> OnIncrementPlayerHealth;
    public static Action<float> OnIncrementPlayerHunger;
    public static Action<float> OnIncrementPlayerThirst;

    private const float healthFrequency = 2;
    public static float healthCounter = 0;
    private static float playerHealth;
    public static float PlayerHealth
    {
        get { return playerHealth; }
        private set
        {
            playerHealth = value;
            if (playerHealth <= 0)
            {
                playerHealth = 0;
            }
            SetPlayerHealthOnUi?.Invoke(playerHealth);
        }
    }
    private static float playerHunger;
    public static float PlayerHunger
    {
        get { return playerHunger; }
        private set
        {
            playerHunger = value;
            if (playerHunger <= 0)
            {
                playerHunger = 0;
            }
            SetPlayerHungerOnUi?.Invoke(playerHunger);
        }
    }
    private static float playerThirst;
    public static float PlayerThirst
    {
        get { return playerThirst; }
        private set
        {
            playerThirst = value;
            if (playerThirst <= 0)
            {
                playerThirst = 0;
            }
            SetPlayerThirstOnUi?.Invoke(playerThirst);
        }
    }

    private void Start()
    {
        LoadPlayerStatsData();
        ActionManager.OnPlayerActionDone += OnPlayerActionDone;
        PlayerMovement.OnPlayerMovedPerMeter += OnPlayerMovedPerMeter;
        OnIncrementPlayerHealth += OnIncrementPlayerHealthEventHandler;
        OnIncrementPlayerHunger += OnIncrementPlayerHungerEventHandler;
        OnIncrementPlayerThirst += OnIncrementPlayerThirstEventHandler;
        MasterSave.RequestSaveData += RequestSaveData;
        InvokeRepeating("DecrementHungerIdle", 0, GEM.idlehungerFrequency);
        InvokeRepeating("DecrementThirstIdle", 0, GEM.idlethirstFrequency);
    }

    private void OnDestroy()
    {
        ActionManager.OnPlayerActionDone -= OnPlayerActionDone;
        PlayerMovement.OnPlayerMovedPerMeter -= OnPlayerMovedPerMeter;
        MasterSave.RequestSaveData -= RequestSaveData;
        OnIncrementPlayerHealth -= OnIncrementPlayerHealthEventHandler;
        OnIncrementPlayerHunger -= OnIncrementPlayerHungerEventHandler;
        OnIncrementPlayerThirst -= OnIncrementPlayerThirstEventHandler;
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

    private void OnIncrementPlayerHealthEventHandler(float health)
    {
        if (PlayerHealth + health > GEM.maxPlayerHealth)
        {
            PlayerHealth = GEM.maxPlayerHealth;
        }
        else
        {
            PlayerHealth += health;
        }
    }

    private void OnIncrementPlayerHungerEventHandler(float hunger)
    {
        if (PlayerHunger + hunger > GEM.maxPlayerHunger)
        {
            PlayerHunger = GEM.maxPlayerHunger;
        }
        else
        {
            PlayerHunger += hunger;
        }
    }

    private void OnIncrementPlayerThirstEventHandler(float thirst)
    {
        if (PlayerThirst + thirst > GEM.maxPlayerThirst)
        {
            PlayerThirst = GEM.maxPlayerThirst;
        }
        else
        {
            PlayerThirst += thirst;
        }
    }
}