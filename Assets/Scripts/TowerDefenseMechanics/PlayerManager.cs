using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class PlayerManager : MonoBehaviour
{
    public static int PlayerHealth = 500;
    public delegate void PlayerHealthIsZero();
    public static event PlayerHealthIsZero OnPlayerHealthIsZero;

    public static int NumberOfTurrets = 0;

    public static Hud m_Hud;

    private void Start()
    {
        m_Hud = GetComponent<Hud>();
    }

    private void OnEnable()
    {
        PathObject.OnReachedPlayerEvent += RemoveHealth;
        GridManager.OnTurretPlacedEvent += IncreaseTurretsCount;
    }

    private void OnDisable()
    {
        PathObject.OnReachedPlayerEvent -= RemoveHealth;
        GridManager.OnTurretPlacedEvent -= IncreaseTurretsCount;
    }

    public void RemoveHealth()
    {
        if (PlayerHealth > 1)
        {
            UpdateHealth(-1);
            // Update HUD
            m_Hud.UpdateHealthH();
        }
        else
        {
            // Check if game over state, if so lounge or game over screen?        
            OnPlayerHealthIsZero();

            // Socialize, review and correct results post-feedback, complete homeworks,
            // and drink energy drinks to renew health
            // before next round
            // Send data over network to backend analytics (clinical provider)
        }
    }

    public void UpdateHealth(int value)
    {
        PlayerHealth += value;
    }

    public void UpdateTurrets(int value)
    {
        NumberOfTurrets += value;
    }

    public void IncreaseTurretsCount()
    {
        if (NumberOfTurrets < 3)
        {
            UpdateTurrets(1);
            m_Hud.UpdateTurretH();
        }
    }
}
