using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class Hud : MonoBehaviour
{
    public string m_HealthText;
    public TMP_Text m_HealthTextObject;
    private StringBuilder m_Sb;

    private void Start()
    {
        m_Sb = new System.Text.StringBuilder();
        m_HealthText = "Health: ";
    }

    public void UpdateHealthH()
    {
        m_Sb.Clear();
        m_Sb.Append(m_HealthText);
        m_Sb.Append(PlayerManager.PlayerHealth);
        m_HealthTextObject.text = m_Sb.ToString();
    }
}
