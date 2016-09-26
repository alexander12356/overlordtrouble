using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance = null;
    private string m_CurrentSceneName = string.Empty;
    private bool m_IsTesting = true;
    private Stack<string> p_SceneIds = new Stack<string>();

    public bool isTesting
    {
        get { return m_IsTesting;  }
        set { m_IsTesting = value; }
    }

    public static GameManager GetInstance()
    {
        if (m_Instance == null)
        {
            GameObject gameObject = new GameObject();
            gameObject.name = "SceneManager";
            gameObject.AddComponent<GameManager>();

            m_Instance = gameObject.GetComponent<GameManager>();
        }
        return m_Instance;
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
        DontDestroyOnLoad(gameObject);
    }

    public void StartLocation(string p_LocationId)
    {
        p_SceneIds.Clear();
        p_SceneIds.Push(p_LocationId);
        m_CurrentSceneName = p_LocationId;
        SceneManager.LoadScene(p_LocationId);
    }

    public void AddScene(string p_SceneId)
    {
        SetActiveForAllObjects(false);
        p_SceneIds.Push(p_SceneId);
        SceneManager.LoadScene(p_SceneId, LoadSceneMode.Additive);
    }

    public void UnloadScene()
    {
        SceneManager.UnloadScene(p_SceneIds.Pop());
        SetActiveForAllObjects(true);
    }

    private void SetActiveForAllObjects(bool p_Value)
    {
        GameObject[] l_RootGameObjects = SceneManager.GetSceneByName(p_SceneIds.Peek()).GetRootGameObjects();

        for (int i = 0; i < l_RootGameObjects.Length; i++)
        {
            l_RootGameObjects[i].SetActive(p_Value);
        }
    }
}
