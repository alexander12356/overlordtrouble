using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoMainScene : MonoBehaviour
{
    private ButtonList m_ButtonList = null;

    private void Awake()
    {
        DataLoader.GetInstance();

        m_ButtonList = GetComponent<ButtonList>();

        m_ButtonList[0].AddAction(StartTestBattle);
        m_ButtonList[1].AddAction(StartMap);
        m_ButtonList[2].AddAction(StartEncyclopedia);
        m_ButtonList[3].AddAction(StartProfile);
        m_ButtonList[4].AddAction(StartImprove);
        m_ButtonList[5].AddAction(StartBoss);
        m_ButtonList[6].AddAction(StartBossSpecial);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

        m_ButtonList.UpdateKey();
    }

    private void StartTestBattle()
    {
        BattleStarter.GetInstance().InitBattle(null, "TestBattleSkwatwolf");
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

    private void StartBoss()
    {
        SceneManager.LoadScene("BossBattleSystem");
    }

    private void StartBossSpecial()
    {
        SceneManager.LoadScene("BossSpecialBattleSystem");
    }

    private void StartRoad()
    {
        SceneManager.LoadScene("Road");
    }
}
