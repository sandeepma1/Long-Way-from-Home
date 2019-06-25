using System;
using System.Collections;
using UnityEngine;

public static class GEM
{
    #region Tags
    public static string MapItemDroppedTagName = "MapItemDropped";
    #endregion

    #region Layers
    public static string MapItemLayerName = "MapItem";
    #endregion

    #region Sprite sorting layer names
    public static readonly string MapTilesSortingLayer = "L0MapTiles";
    public static readonly string MapItemsSortingLayer = "L1MapItems";
    public static readonly string PlayerSortingLayer = "L2Player";
    #endregion

    #region Save names identifiers
    public static string MapSaveName = "MapSave";
    public static string PlayerInfoSaveName = "PlayerInfoSave";
    public static string AllFurnituresSaveName = "AllFurnituresSave";
    #endregion

    #region  Player Profile
    public static string playerName;
    public static string playerFarmName;
    public static int playerLevel;
    public static int playerXPPoints;
    public static int playerCoins;
    public static int playerGems;
    public static int playerStamina;
    public static string playerStaminaMaxDateTime;
    #endregion

    #region Debug Stuff
    public static bool ShowDebug = false;
    public static bool ShowDebugError = true;
    public static bool ShowDebugWarning = true;
    public static void PrintDebug(string text)
    {
        if (ShowDebug)
        {
            Debug.Log(text);
        }
    }
    public static void PrintDebugError(string text)
    {
        if (ShowDebugError)
        {
            Debug.LogError(text);
        }
    }
    public static void PrintDebugWarning(string text)
    {
        if (ShowDebugWarning)
        {
            Debug.LogWarning(text);
        }
    }
    #endregion

    public static int numberOfRocksInLevel = 0;
    public static int[] screensPositions = new int[] { 0, 7, 24, 41, 58, 75, 92 };
    public static int[] farmLandUpgrade = new int[] { 1, 3, 5, 7, 9, 11, 13, 16 };
    public static bool isObjectDragging = false;

    public delegate void GameEvent();

    private static E_STATES m_gameState = E_STATES.e_game;
    public static void SetState(E_STATES state)
    {
        m_gameState = state;
    }
    public static E_STATES GetState()
    {
        return m_gameState;
    }
    public enum E_STATES
    {
        e_game,
        e_pause,
        e_inventory
    };

    public static System.DateTime dateTime = System.DateTime.Now;
    private static E_MenuState m_menuState = E_MenuState.e_menuDown;
    public static void SetMenuState(E_MenuState state)
    {
        m_menuState = state;
    }
    public static E_MenuState GetMenuState()
    {
        return m_menuState;
    }
    public enum E_MenuState
    {
        e_menuUp,
        e_menuDown
    };
}