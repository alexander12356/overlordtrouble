using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    private static WinPanel m_Instance;

    static public WinPanel GetInstance()
    {
        return m_Instance;
    }

    private void Awake()
    {
        m_Instance = this;
    }

    public void ButtonDepress(int p_keyId)
    {
        switch (p_keyId)
        {
            case 1:
                SceneManager.LoadScene("BattleSystem");
                break;
        }
    }
}
