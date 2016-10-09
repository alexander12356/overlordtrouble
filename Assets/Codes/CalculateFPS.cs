using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CalculateFPS : MonoBehaviour
{
#if DEVELOPMENT_BUILD || UNITY_EDITOR
    private Text m_Text;
    private int m_FpsCounter;

    public static int currentFps = 0;

    public void Start()
    {
        m_Text = GetComponent<Text>();
        StartCoroutine(FPSRefresher());
    }

    public void Update()
    {
        m_FpsCounter++;
    }

    private IEnumerator FPSRefresher()
    {
        while (true)
        {
            if (m_FpsCounter < 30)
            {
                m_Text.color = Color.red;
            }
            else if (m_FpsCounter < 50)
            {
                m_Text.color = Color.yellow;
            }
            else
            {
                m_Text.color = Color.green;
            }
            currentFps = m_FpsCounter;
            m_Text.text = m_FpsCounter.ToString();
            m_FpsCounter = 0;
            yield return new WaitForSeconds(1);
        }
    }
#endif
}