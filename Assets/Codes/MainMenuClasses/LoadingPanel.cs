using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : Panel
{
    public static LoadingPanel m_Prefab = null;
    private ButtonList m_SavesList = null;
    private ButtonListScrolling m_SavesListScrolling = null;

    private event PanelActionHandler m_CancelAction;

    // test data
    private List<SaveData> m_TestSaveData = new List<SaveData>();

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

        SaveDataBase.GetInstance().Parse();

        InitButtonList();
    }

    private void InitButtonList()
    {
        foreach (SaveData l_SaveData in SaveDataBase.GetInstance().GetSaves().Values)
        {
            string l_LevelText = LocalizationDataBase.GetInstance().GetText("GUI:Profile:Level");
            string l_DurationText = LocalizationDataBase.GetInstance().GetText("GUI:LoadingPanel:GameDuration");

            LoadButton l_LoadButton = Instantiate(LoadButton.prefab);
            l_LoadButton.title = l_SaveData.userName + " " + l_SaveData.level + " " + l_LevelText + ". " + l_SaveData.saveDate.ToString() + "\n" + l_SaveData.gameDuration + " " + l_DurationText;
            l_LoadButton.AddAction(LoadGame);
            l_LoadButton.Init(l_SaveData);
            savesList.AddButton(l_LoadButton);
        }
        savesList.AddKeyArrowAction(savesListScrolling.CheckScrolling);
        savesList.AddCancelAction(Cancel);
        savesList.isActive = true;

        savesListScrolling.Init(244.0f, 1);
        savesListScrolling.scrollRect.verticalNormalizedPosition = 1.0f;
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
        {
            return;
        }

        savesList.UpdateKey();

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

        SaveDataBase.GetInstance().DeleteSave(l_LoadButton.saveData.userName);
    }

    private void LoadGame()
    {
        YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
        l_YesNoPanel.AddYesAction(StartLoadedGame);
        l_YesNoPanel.SetText(LocalizationDataBase.GetInstance().GetText("GUI:LoadingPanel:Shure"));
        MainMenuSystem.GetInstance().ShowPanel(l_YesNoPanel, true);
    }

    private void StartLoadedGame()
    {
        GameManager.GetInstance().isTesting = false;

        SaveData l_SaveData = (m_SavesList.currentButton as LoadButton).saveData;

        PlayerData.GetInstance().LoadData(l_SaveData.playerData);
        SaveSystem.GetInstance().LoadFromFile(l_SaveData.worldState);
        SaveSystem.GetInstance().StartDuration();
        PlayerInventory.GetInstance().LoadData(l_SaveData.inventoryItems, l_SaveData.equipmentSlotData);

        string l_SenderLocation = l_SaveData.location["SenderLocation"].str;
        string l_TargetRoom = l_SaveData.location["TargetRoom"].str;
        string l_Scene = l_SaveData.location["Scene"].str;

        SaveSystem.GetInstance().Init(l_Scene);
        PlayerPrefs.SetString("SenderLocation", l_SenderLocation);
        PlayerPrefs.SetString("TargetRoomId", l_TargetRoom);
        MainMenuSystem.GetInstance().StartLocation(l_Scene);
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
}
