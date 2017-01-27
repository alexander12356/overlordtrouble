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
    [SerializeField]
    private ScreenFader m_ScreenFader = null;
    AsyncOperation m_AsyncOp;
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
        yield return new WaitForSeconds(1);
        IEnumerable<Type> typeList = ConcatTypes(GetTypeList(typeof(Panel)), GetTypeList(typeof(PanelButton)));
        m_AsyncOp = SceneManager.LoadSceneAsync("MainMenu");
        m_AsyncOp.allowSceneActivation = false;
        while (m_AsyncOp.progress < 0.9f || !m_PrefabsLoadedComplete)
        {
            SetProgressBarValue();
            yield return StartCoroutine(LoadPrefabs(typeList));
        }
        LoadDataBases();
        yield return StartCoroutine(m_ScreenFader.FadeToBlack());
        m_AsyncOp.allowSceneActivation = true;
        while (!m_AsyncOp.isDone)
        {
            SetProgressBarValue(); // TODO: в принципе бесполезно, т.к. к этому времени ScreenFader затемнит сцену, но пусть пока стоит. P.S. весь цикл по идее бесполезен
            yield return null;
        }
        yield return m_AsyncOp;
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
        int i = 1; // счет начинается с одного, чтобы i в конце цикла было равно l_PrefabsCount
        foreach (var type in p_TypeList)
        {
            Debug.Log(type);
            PropertyInfo loadPrefab = type.GetProperty("prefab");
            if (loadPrefab != null)
                loadPrefab.GetValue(type, null);
            m_PercentPrefabsLoaded = ((float)i / l_PrefabsCount);
            SetProgressBarValue();
            i++;
            yield return new WaitForSeconds(0.05f); // искусственная задержка, чтобы загрузка не была слишком быстрой
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
        // op.progress делится не на 2, а на 1.8 потому что максимальное значение op.progress в текущей реализации равно 0.9
        float l_ProgressValue = m_AsyncOp.progress / 1.8f + m_PercentPrefabsLoaded / 2.0f;
        if (l_ProgressValue > 1.0f)
            l_ProgressValue = 1.0f;
        m_ProgressBar.value = l_ProgressValue;
    }
}
