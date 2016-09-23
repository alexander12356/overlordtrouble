using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance = null;
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

    public void StartLocation(string p_LocationId)
    {
        m_CurrentSceneName = p_LocationId;
        SceneManager.LoadScene(p_LocationId);
    }

    public void AddScene(string p_SceneId)
    {
        m_CurrentSceneName = p_SceneId;
        SetActiveForAllObjects(false);
        SceneManager.LoadScene(p_SceneId, LoadSceneMode.Additive);
    }

    public void UnloadScene()
    {
        SceneManager.UnloadScene(m_CurrentSceneName);
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
