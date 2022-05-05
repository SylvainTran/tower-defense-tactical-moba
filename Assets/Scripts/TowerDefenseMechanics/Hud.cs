using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class Hud : MonoBehaviour
{
    public string m_HealthText;
    public TMP_Text m_HealthTextObject;

    public string m_TurretText;
    public TMP_Text m_TurretTextObject;

    private StringBuilder m_Sb;

    private void Start()
    {
        m_Sb = new System.Text.StringBuilder();
        m_HealthText = "Health: ";
        m_TurretText = "Heroes: ";
    }

    public void UpdateText(TMP_Text textObject, string text, int value)
    {
        m_Sb.Clear();
        m_Sb.Append(text);
        m_Sb.Append(value);
        textObject.text = m_Sb.ToString();
    }

    public void UpdateHealthH()
    {
        if (m_HealthTextObject == null)
        {
            return;
        }
        UpdateText(m_HealthTextObject, m_HealthText, PlayerManager.PlayerHealth);
    }

    public void UpdateTurretH()
    {
        if (m_TurretTextObject == null)
        {
            return;
        }
        UpdateText(m_TurretTextObject, m_TurretText, PlayerManager.NumberOfTurrets);
    }
}
