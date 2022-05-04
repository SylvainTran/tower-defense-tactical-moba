using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float delay = 0;
    public float time = 60;
    public float ticksPerSecond = 1;
    private float frameCount = 0;
    private float totalFrameCount = 0;

    public TMP_Text m_TimerTextObject;
    private StringBuilder m_Sb = new StringBuilder();
    private string m_TimeText = "Time: ";

    public delegate void TenSecondsReached();
    public static TenSecondsReached OnTenSecondsReached;

    public delegate void ZeroSecondsReached();
    public static ZeroSecondsReached OnZeroSecondsReached;

    void Update()
    {
        ++frameCount;
        ++totalFrameCount;
        if (frameCount >= 1.0f/Time.deltaTime)
        {
            time -= ticksPerSecond;
            m_Sb.Clear();
            m_Sb.Append(m_TimeText);
            m_Sb.Append(time);
            m_TimerTextObject.text = m_Sb.ToString();
            frameCount = 0;
        }
        if(totalFrameCount % 10 == 0)
        {
            OnTenSecondsReached();
        }
        if(time == 0)
        {
            OnZeroSecondsReached();
        }
    }
}
