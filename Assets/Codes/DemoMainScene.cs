using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoMainScene : MonoBehaviour
{
    private ButtonList m_ButtonList = null;

    private void Awake()
    {
        DataLoader.GetInstance();

        m_ButtonList = GetComponent<ButtonList>();

        m_ButtonList[0].AddAction(StartBattleSystem);
        m_ButtonList[1].AddAction(StartMap);
        m_ButtonList[2].AddAction(StartEncyclopedia);
        m_ButtonList[3].AddAction(StartProfile);
        m_ButtonList[4].AddAction(StartImprove);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

        m_ButtonList.UpdateKey();
    }

    private void StartBattleSystem()
    {
        BattleStarter.GetInstance().InitBattle(null, "TestBattle");
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

    private void StartProfile()
    {
        SceneManager.LoadScene("Profile");
    }

    private void StartImprove()
    {
        SceneManager.LoadScene("Improve");
    }
}
