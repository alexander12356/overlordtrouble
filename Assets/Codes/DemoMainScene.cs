using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoMainScene : MonoBehaviour
{
    private ButtonList m_ButtonList = null;

    private void Awake()
    {
        m_ButtonList = GetComponent<ButtonList>();

        m_ButtonList[0].AddAction(StartBattleSystem);
        m_ButtonList[1].AddAction(StartMap);
        m_ButtonList[2].AddAction(StartEncyclopedia);
    }

    private void Update()
    {
        m_ButtonList.UpdateKey();
    }

    private void StartBattleSystem()
    {
        SceneManager.LoadScene("BattleSystem");
    }

    private void StartMap()
    {
        SceneManager.LoadScene("Town");
    }

    private void StartEncyclopedia()
    {
        SceneManager.LoadScene("Encyclopedia");
    }
}
