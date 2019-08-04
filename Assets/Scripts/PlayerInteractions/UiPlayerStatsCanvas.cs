using TMPro;
using UnityEngine;

public class UiPlayerStatsCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI hungerText;
    [SerializeField] private TextMeshProUGUI thirstText;

    private void Awake()
    {
        PlayerStats.SetPlayerHealth += SetPlayerHealth;
        PlayerStats.SetPlayerHunger += SetPlayerHunger;
        PlayerStats.SetPlayerThirst += SetPlayerThirst;
    }

    private void OnDestroy()
    {
        PlayerStats.SetPlayerHealth -= SetPlayerHealth;
        PlayerStats.SetPlayerHunger -= SetPlayerHunger;
        PlayerStats.SetPlayerThirst -= SetPlayerThirst;
    }

    private void SetPlayerHealth(float health)
    {
        healthText.text = health.ToString("F0");
    }

    private void SetPlayerHunger(float hunger)
    {
        hungerText.text = hunger.ToString("F0");
    }

    private void SetPlayerThirst(float thirst)
    {
        thirstText.text = thirst.ToString("F0");
    }
}