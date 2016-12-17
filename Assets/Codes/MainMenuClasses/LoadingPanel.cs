using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSaveData
{
    public string username;
    public int level;
    public DateTime saveDate;
    public DateTime gameDuration;

    public TestSaveData(string p_Username, int p_Level, DateTime p_DateTime, DateTime p_GameDuration)
    {
        username = p_Username;
        level = p_Level;
        saveDate = p_DateTime;
        gameDuration = p_GameDuration;
    }
}

public class LoadingPanel : Panel
{
    public static LoadingPanel m_Prefab = null;
    [SerializeField]
    private ButtonList m_SavesButtonList = null;

    private event PanelActionHandler m_CancelAction;

    // test data
    private List<TestSaveData> m_TestSaveData = null;

    public static LoadingPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<LoadingPanel>("Prefabs/Panels/LoadingPanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();
        // test fill
        FillSaveData();
        InitButtonList();
    }

    private void InitButtonList()
    {
        // test data 
        foreach (var testData in m_TestSaveData)
        {
            LoadButton loadButton = Instantiate(LoadButton.prefab);
            loadButton.title = String.Format("{0} {1} уровень. {2} \nвремя игры {3}", testData.username, testData.level, testData.saveDate, testData.gameDuration.ToShortTimeString());
            m_SavesButtonList.AddButton(loadButton);
        }
        m_SavesButtonList.isActive = true;
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Backspace))
        {
            Cancel();
            Input.ResetInputAxes();
        }
    }

    private void RunNewGame()
    {

    }

    private void Cancel()
    {
        m_SavesButtonList.isActive = false;
        if (m_CancelAction != null)
        {
            m_CancelAction();
        }
        Close();
    }

    public void AddCancelAction(PanelActionHandler p_Action)
    {
        m_CancelAction += p_Action;
    }

    public void RemoveCancelAction(PanelActionHandler p_Action)
    {
        m_CancelAction -= p_Action;
    }

    // test 
    private void FillSaveData()
    {
        m_TestSaveData = new List<TestSaveData>();
        TestSaveData l_TestSaveData;
        l_TestSaveData = new TestSaveData("Кеша", 1, DateTime.Now, DateTime.Now);
        m_TestSaveData.Add(l_TestSaveData);
        l_TestSaveData = new TestSaveData("Юрген", 10, DateTime.Now, DateTime.Now);
        m_TestSaveData.Add(l_TestSaveData);
        l_TestSaveData = new TestSaveData("Володя", 99, DateTime.Now, DateTime.Now);
        m_TestSaveData.Add(l_TestSaveData);
    }
}
