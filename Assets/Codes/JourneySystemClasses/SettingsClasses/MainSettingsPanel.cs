using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSettingsPanel : SettingsPanel
{
    private static MainSettingsPanel m_Prefab = null;
    private PanelDropdown m_LanguageDropdown = null;
    private string m_OldLanguageId = string.Empty;
    private string m_NewLanguageId = string.Empty;
    private List<string> m_LanguagesIds = new List<string>();

    private PanelDropdown languageDropDown
    {
        get { return m_LanguageDropdown; }
    }

    public static MainSettingsPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<MainSettingsPanel>("Prefabs/Panels/MainSettingsPanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_LanguageDropdown = transform.FindChild("SettingsElements").FindChild("Language").GetComponentInChildren<PanelDropdown>();
    }

    public override void Start()
    {
        base.Start();

        InitLanguagesList();
    }

    public override void InitOldOptions()
    {
        base.InitOldOptions();

        m_OldLanguageId = LocalizationDataBase.GetInstance().currentLanguage;
    }

    private void InitLanguagesList()
    {
        m_LanguagesIds = LocalizationDataBase.GetInstance().GetLanguages();
        List<string> l_LanguagesList = new List<string>();

        for (int i = 0; i < m_LanguagesIds.Count; i++)
        {
            l_LanguagesList.Add(LocalizationDataBase.GetInstance().GetText("Lang:" + m_LanguagesIds[i]));
        }

        languageDropDown.ClearOptions();
        languageDropDown.AddOptions(l_LanguagesList);
        languageDropDown.currentValue = m_LanguagesIds.IndexOf(LocalizationDataBase.GetInstance().currentLanguage);
        languageDropDown.AddCancelAction(DeselectLanguage);

        m_NewLanguageId = m_LanguagesIds[languageDropDown.currentValue];
    }

    public override void InitControlButtons()
    {
        base.InitControlButtons();

        controlButtonList[4].AddAction(SelectLanguage);
        controlButtonList[4].title = LocalizationDataBase.GetInstance().GetText("GUI:SettingsPanel:Language");
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        languageDropDown.UpdateKey();
    }

    #region LANGUAGE
    private void SelectLanguage()
    {
        controlButtonList.isActive = false;
        languageDropDown.isActive = true;
        languageDropDown.Show();
    }

    public void DeselectLanguage()
    {
        controlButtonList.isActive = true;
        languageDropDown.isActive = false;
        languageDropDown.Hide();
        m_NewLanguageId = m_LanguagesIds[languageDropDown.value];
    }
    #endregion

    public override void Accept()
    {
        if (m_OldLanguageId != m_NewLanguageId)
        {
            LocalizationDataBase.GetInstance().ChangeLanguage(m_NewLanguageId);
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            base.Accept();
        }
    }
}
