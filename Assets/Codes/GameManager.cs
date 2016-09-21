using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance = null;
    private string m_PrevSceneName = string.Empty;
    private string m_CurrentSceneName = string.Empty;

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

    public void LoadScene(string p_SceneId, LoadSceneMode p_LoadSceneMode)
    {
        if (p_LoadSceneMode == LoadSceneMode.Additive)
        {
            m_PrevSceneName = SceneManager.GetActiveScene().name;
            SetActiveForAllObjects(false);
        }

        if (p_SceneId == m_PrevSceneName)
        {
            SceneManager.UnloadScene(m_CurrentSceneName);
            m_CurrentSceneName = p_SceneId;
            m_PrevSceneName = string.Empty;
            SetActiveForAllObjects(true);
        }
        else
        {
            SceneManager.LoadScene(p_SceneId, p_LoadSceneMode);
            m_CurrentSceneName = p_SceneId;
        }        
    }

    private void UnloadScene()
    {
        SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
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
