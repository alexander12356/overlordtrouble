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
    private ButtonList m_SavesList = null;
    private ButtonListScrolling m_SavesListScrolling = null;

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

    private ButtonList savesList
    {
        get
        {
            if(m_SavesList == null)
            {
                m_SavesList = transform.FindChild("SavesList").GetComponentInChildren<ButtonList>();
            }
            return m_SavesList;
        }
    }

    private ButtonListScrolling savesListScrolling
    {
        get
        {
            if(m_SavesListScrolling == null)
            {
                m_SavesListScrolling = transform.FindChild("SavesList").GetComponentInChildren<ButtonListScrolling>();
            }
            return m_SavesListScrolling;
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
            savesList.AddButton(loadButton);
        }
        savesList.AddKeyArrowAction(savesListScrolling.CheckScrolling);
        savesList.isActive = true;

        savesListScrolling.Init(244.0f, 1);
        savesListScrolling.scrollRect.verticalNormalizedPosition = 1.0f;
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        savesList.UpdateKey();

        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Backspace))
        {
            Cancel();
            Input.ResetInputAxes();
        }

        if(Input.GetKeyDown(KeyCode.Delete))
        {
            TryDeleteSave();
        }
    }

    private void TryDeleteSave()
    {
        YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
        l_YesNoPanel.AddYesAction(DeleteSave);
        l_YesNoPanel.SetText(LocalizationDataBase.GetInstance().GetText("GUI:Profile:QuestionImproveStats"));
        MainMenuSystem.GetInstance().ShowPanel(l_YesNoPanel, true);
    }

    private void DeleteSave()
    {
        LoadButton l_LoadButton = (LoadButton)savesList.currentButton;
        string removedButtonId = l_LoadButton.id;
        savesList.RemoveButton(savesList.currentButtonId);
    }

    private void RunNewGame()
    {

    }

    private void Cancel()
    {
        m_SavesList.isActive = false;
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
        l_TestSaveData = new TestSaveData("Flea", 12, DateTime.Now, DateTime.Now);
        m_TestSaveData.Add(l_TestSaveData);
    }
}
