using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResourceLoader : MonoBehaviour {

    [SerializeField]
    private Slider m_ProgressBar = null;
    AsyncOperation op;
    float m_PercentPrefabsLoaded;
    bool m_PrefabsLoadedComplete;
    void Start()
    {
        m_PercentPrefabsLoaded = 0.0f;
        m_PrefabsLoadedComplete = false;
        StartCoroutine(LoadingResources());
    }

    IEnumerator LoadingResources()
    {
        yield return null;
        op = SceneManager.LoadSceneAsync("MainMenu");
        op.allowSceneActivation = false;
        IEnumerable<Type> typeList = ConcatTypes(GetTypeList(typeof(Panel)), GetTypeList(typeof(PanelButton)));
        while (op.progress < 0.9f || !m_PrefabsLoadedComplete)
        {
            SetProgressBarValue();
            yield return StartCoroutine(LoadPrefabs(typeList));
        }
        op.allowSceneActivation = true;
        while (!op.isDone)
        {
            SetProgressBarValue();
            LoadDataBases();
            yield return null;
        }
        yield return op;
    }

    private void LoadDataBases()
    {
        PlayerData.GetInstance();
        PlayerInventory.GetInstance();
        DataLoader.GetInstance();
    }

    IEnumerator LoadPrefabs(Type p_Type)
    {
        IEnumerable<Type> typeList = Assembly.GetAssembly(p_Type).GetTypes().Where(type => type.IsSubclassOf(p_Type));
        foreach (var type in typeList)
        {
            Debug.Log(type);
            PropertyInfo loadPrefab = type.GetProperty("prefab");
            if(loadPrefab != null)
                loadPrefab.GetValue(type, null);
            yield return null;
        }
    }

    IEnumerator LoadPrefabs(IEnumerable<Type> p_TypeList)
    {
        int l_PrefabsCount = p_TypeList.Count();
        int i = 0;
        foreach (var type in p_TypeList)
        {
            Debug.Log(type);
            PropertyInfo loadPrefab = type.GetProperty("prefab");
            if (loadPrefab != null)
                loadPrefab.GetValue(type, null);
            m_PercentPrefabsLoaded = ((float)i / l_PrefabsCount);
            SetProgressBarValue();
            i++;
            yield return new WaitForSeconds(0.05f);
        }
        m_PrefabsLoadedComplete = true;
    }

    private IEnumerable<Type> GetTypeList(Type p_Type)
    {
        return Assembly.GetAssembly(p_Type).GetTypes().Where(type => type.IsSubclassOf(p_Type));
    }

    private IEnumerable<Type> ConcatTypes(IEnumerable<Type> p_ListA, IEnumerable<Type> p_ListB)
    {
        return p_ListA.Concat(p_ListB);
    }

    private void SetProgressBarValue()
    {
        float l_ProgressValue = op.progress / 2.0f + m_PercentPrefabsLoaded / 2.0f;
        if (l_ProgressValue > 1.0f)
            l_ProgressValue = 1.0f;
        m_ProgressBar.value = l_ProgressValue;
    }
}
