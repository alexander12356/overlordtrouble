using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class EncyclodepiaPanel: Panel {

    private ButtonList m_EnemyList = null;
    private ButtonListScrolling m_EnemyListScrolling = null;
    private Text m_EnemyDescriptionText = null;
    private Image m_AvatarImage = null;

    public override void Awake()
    {
        base.Awake();

        EncyclopediaSystem.GetInstance().ShowPanel(this);

        InitAdditionalInfo();
        InitButtonLists();
        InitMonsterList();      
    }

    public void Start()
    {
        ShowEnemyInfo();
    }

    private void InitAdditionalInfo()
    {
        m_EnemyDescriptionText = transform.FindChild("EnemyDescription").GetComponentInChildren<Text>();
        m_AvatarImage = transform.FindChild("Avatar").GetComponentInChildren<Image>();
    }

    private void InitButtonLists()
    {
        m_EnemyList = transform.FindChild("EnemyList").FindChild("MaskEnemyList").GetComponentInChildren<ButtonList>();
        m_EnemyList.AddKeyArrowAction(ShowEnemyInfo);
        m_EnemyList.isActive = true;

        m_EnemyListScrolling = transform.FindChild("EnemyList").GetComponentInChildren<ButtonListScrolling>();
        m_EnemyListScrolling.Init(60.0f, 14);
        m_EnemyList.AddKeyArrowAction(m_EnemyListScrolling.CheckScrolling);
    }

    private void ShowEnemyInfo()
    {
        PanelButtonEncyclopediaEnemy l_PanelButtonEncyclopediaEnemy = (PanelButtonEncyclopediaEnemy)m_EnemyList.currentButton;
        string l_EnemyDescriptionStr = LocalizationDataBase.GetInstance().GetText("Enemy:" + l_PanelButtonEncyclopediaEnemy.enemyId + ":Description");
        string l_EnemyElementalStr = LocalizationDataBase.GetInstance().GetText("Element") + ": " + LocalizationDataBase.GetInstance().GetText("Elemental:" + EnemyDataBase.GetInstance().GetEnemy(l_PanelButtonEncyclopediaEnemy.enemyId).elemental);
        m_EnemyDescriptionText.text = l_EnemyDescriptionStr + "\n\n" + l_EnemyElementalStr;
        m_AvatarImage.sprite = Resources.Load<Sprite>("Sprites/Creations/" + l_PanelButtonEncyclopediaEnemy.enemyId + "/BattleProfile");
    }

    void InitMonsterList()
    {
        Dictionary<string, EnemyData> l_EnemyBase = EnemyDataBase.GetInstance().GetEnemyBase();
        foreach(string l_Key in l_EnemyBase.Keys)
        {
            PanelButtonEncyclopediaEnemy l_PanelButton = Instantiate(PanelButtonEncyclopediaEnemy.prefab);
            l_PanelButton.enemyId = l_EnemyBase[l_Key].id;
            m_EnemyList.AddButton(l_PanelButton);
        }
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_EnemyList.UpdateKey();
    }
}
