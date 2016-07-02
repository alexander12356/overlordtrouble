using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : MonoBehaviour
{
    private static LosePanel m_Instance;

    static public LosePanel GetInstance()
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
