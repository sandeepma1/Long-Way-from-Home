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

    #region Player Stats values
    public static float actionHungerExhaustionLevel = 0.15f;
    public static float actionThirstExhaustionLevel = 0.3f;
    public static float walkingHungerExhaustionLevel = 0.0025f;
    public static float walkingThirstExhaustionLevel = 0.005f;
    public static float idleHungerExhaustionLevel = 0.025f;
    public static float idleThirstExhaustionLevel = 0.05f;
    public static float idlehungerFrequency = 20;
    public static float idlethirstFrequency = 20;
    public static float healthExhaustionLevel = 5f;
    #endregion

    #region Save names identifiers
    public static string MapSaveName = "MapSave";
    public static string PlayerInfoSaveName = "PlayerInfoSave";
    public static string PlayerStatsSaveName = "PlayerStatsSave";
    public static string AllFurnituresSaveName = "AllFurnituresSave";
    #endregion

    #region Debug Stuff
    public static bool ShowDebug = true;
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