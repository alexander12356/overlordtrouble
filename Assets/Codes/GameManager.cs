using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Prefab;
    private static GameManager m_Instance = null;
    private string m_CurrentSceneName = string.Empty;
    private bool m_IsTesting = true;
    private Stack<string> m_SceneIds = new Stack<string>();

    public bool isTesting
    {
        get { return m_IsTesting;  }
        set { m_IsTesting = value; }
    }
    public static GameManager prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<GameManager>("Prefabs/GameManager");
            }
            return m_Prefab;
        }
    }
    public static GameManager GetInstance()
    {
        if (m_Instance == null)
        {
            m_Instance = Instantiate(prefab);
        }
        return m_Instance;
    }
    public string currentSceneName
    {
        get { return m_CurrentSceneName; }
    }

    public static bool IsInstance()
    {
        if (m_Instance == null)
        {
            return false;
        }
        return true;
    }

    public void Awake()
    {
        m_Instance = this;
        m_CurrentSceneName = SceneManager.GetActiveScene().name;

        DontDestroyOnLoad(gameObject);
    }

    public void StartLocation(string p_LocationId)
    {
        m_SceneIds.Clear();
        m_SceneIds.Push(p_LocationId);
        m_CurrentSceneName = p_LocationId;
        SceneManager.LoadScene(p_LocationId);
    }

    public void AddScene(string p_SceneId)
    {
        if (m_SceneIds.Count == 0)
        {
            if (isTesting)
            {
                m_SceneIds.Push(SceneManager.GetActiveScene().name);
            }
            else
            {
                Debug.LogError("Oppanki");
            }
        }
        SetActiveForAllObjects(false);
        m_SceneIds.Push(p_SceneId);
        SceneManager.LoadScene(p_SceneId, LoadSceneMode.Additive);
    }

    public void UnloadScene()
    {
        if (m_SceneIds.Count == 0)
        {
            if (isTesting)
            {
                SceneManager.LoadScene("DemoMainScene");
                return;
            }
            else
            {
                Debug.LogError("Oppanki");
            }
        }

        SceneManager.UnloadSceneAsync(m_SceneIds.Pop());
        SetActiveForAllObjects(true);
    }

    private void SetActiveForAllObjects(bool p_Value)
    {
        GameObject[] l_RootGameObjects = SceneManager.GetSceneByName(m_SceneIds.Peek()).GetRootGameObjects();
        for (int i = 0; i < l_RootGameObjects.Length; i++)
        {
            l_RootGameObjects[i].SetActive(p_Value);
        }
    }
}
