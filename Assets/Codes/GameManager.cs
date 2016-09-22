using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance = null;
    private string m_PrevSceneName = string.Empty;
    private string m_CurrentSceneName = string.Empty;
    private JourneyNPC m_JourneyNpc = null;

    public static GameManager GetInstance()
    {
        if (m_Instance == null)
        {
            GameObject gameObject = new GameObject();
            gameObject.name = "GameManager";
            gameObject.AddComponent<GameManager>();

            m_Instance = gameObject.GetComponent<GameManager>();
        }
        return m_Instance;
    }

    public void Awake()
    {
        m_Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartLocation(string p_LocationId)
    {
        SceneManager.LoadScene(p_LocationId);
    }

    public void StartBattle()
    {
        SetActiveForAllObjects(false);
        SceneManager.LoadScene("BattleSystem", LoadSceneMode.Additive);
    }

    public void EndBattle()
    {
        SceneManager.UnloadScene("BattleSystem");
        SetActiveForAllObjects(true);
    }

    private void SetActiveForAllObjects(bool p_Value)
    {
        GameObject[] l_RootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        for (int i = 0; i < l_RootGameObjects.Length; i++)
        {
            l_RootGameObjects[i].SetActive(p_Value);
        }
    }
}
