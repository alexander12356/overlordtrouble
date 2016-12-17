using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : Panel {
    public static SettingsPanel m_Prefab = null;
    private ButtonList m_ControlButtonList = null;
    private PanelSlider m_VolumeSlider = null;
    private PanelDropdown m_LanguageDropdown = null;
    private PanelCheckbox m_SubtitlesCheckbox = null;
    private PanelValueSelector m_ResolutionSelector = null;
    private PanelCheckbox m_WindowedModeCheckbox = null;

    private event PanelActionHandler m_CancelAction;

    #region PROPERTIES

    public static SettingsPanel prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<SettingsPanel>("Prefabs/Panels/SettingsPanel");
            }
            return m_Prefab;
        }
    }

    public ButtonList controlButtonList
    {
        get
        {
            if(m_ControlButtonList == null)
            {
                m_ControlButtonList = GetComponent<ButtonList>();
            }
            return m_ControlButtonList;
        }
    }

    public PanelSlider volumeSlider
    {
        get
        {
            if(m_VolumeSlider == null)
            {
                m_VolumeSlider = transform.FindChild("SettingsElements").FindChild("Volume").GetComponentInChildren<PanelSlider>();
            }
            return m_VolumeSlider;
        }
    }

    public PanelDropdown languageDropDown
    {
        get
        {
            if(m_LanguageDropdown == null)
            {
                m_LanguageDropdown = transform.FindChild("SettingsElements").FindChild("Language").GetComponentInChildren<PanelDropdown>();
            }
            return m_LanguageDropdown;
        }
    }

    public PanelCheckbox subtitlesCheckbox
    {
        get
        {
            if (m_SubtitlesCheckbox == null)
            {
                m_SubtitlesCheckbox = transform.FindChild("SettingsElements").FindChild("Subtitles").GetComponentInChildren<PanelCheckbox>();
            }
            return m_SubtitlesCheckbox;
        }
    }

    public PanelValueSelector resolutionSelector
    {
        get
        {
            if(m_ResolutionSelector == null)
            {
                m_ResolutionSelector = transform.FindChild("SettingsElements").FindChild("Resolution").GetComponentInChildren<PanelValueSelector>();
            }
            return m_ResolutionSelector;
        }
    }

    public PanelCheckbox windowedModeCheckbox
    {
        get
        {
            if(m_WindowedModeCheckbox == null)
            {
                m_WindowedModeCheckbox = transform.FindChild("SettingsElements").FindChild("WindowedMode").GetComponentInChildren<PanelCheckbox>();
            }
            return m_WindowedModeCheckbox;
        }
    }

    #endregion

    public override void Awake()
    {
        base.Awake();

        InitControlButtons();
        InitVolume();
        InitLanguagesList();
        InitSubtitlesCheckbox();
        InitResolution();
        InitWindowedMode();
    }

    #region INITS

    private void InitControlButtons()
    {
        controlButtonList[0].AddAction(SelectVolume);
        controlButtonList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:SettingsPanel:Volume");
        controlButtonList[1].AddAction(SelectLanguage);
        controlButtonList[1].title = LocalizationDataBase.GetInstance().GetText("GUI:SettingsPanel:Language");
        controlButtonList[2].AddAction(SelectSubtitles);
        controlButtonList[2].title = LocalizationDataBase.GetInstance().GetText("GUI:SettingsPanel:Subtitles");
        controlButtonList[3].AddAction(SelectResolution);
        controlButtonList[3].title = LocalizationDataBase.GetInstance().GetText("GUI:SettingsPanel:Resolution");
        controlButtonList[4].AddAction(SelectWindowedMode);
        controlButtonList[4].title = LocalizationDataBase.GetInstance().GetText("GUI:SettingsPanel:WindowedMode");
        controlButtonList[5].AddAction(Accept);
        controlButtonList[5].title = LocalizationDataBase.GetInstance().GetText("GUI:SettingsPanel:Accept");
        controlButtonList[6].AddAction(Cancel);
        controlButtonList[6].title = LocalizationDataBase.GetInstance().GetText("GUI:SettingsPanel:Cancel");
        controlButtonList.isActive = true;
    }

    private void InitVolume()
    {
        volumeSlider.AddCancelAction(DeselectVolume);
        volumeSlider.currentValue = 0.5f; // начальное значение, надо откуда то загружать
    }

    private void InitLanguagesList()
    {
        List<string> languagesList = new List<string>() { "Английский", "Русский", "Французский", "Литовский" };
        languageDropDown.ClearOptions();
        languageDropDown.AddOptions(languagesList);
        languageDropDown.AddCancelAction(DeselectLanguage);
    }

    private void InitSubtitlesCheckbox()
    {
        subtitlesCheckbox.AddCancelAction(DeselectSubtitles);
        subtitlesCheckbox.currentValue = false;
    }

    private void InitResolution()
    {
        // test data
        Dictionary<int, string> l_Resolutions = new Dictionary<int, string>();
        l_Resolutions.Add(0, "800х600");
        l_Resolutions.Add(1, "1024х780");
        l_Resolutions.Add(2, "1600х800");
        l_Resolutions.Add(3, "1920х1080");

        resolutionSelector.values = l_Resolutions;
        resolutionSelector.AddCancelAction(DeselectResolution);
    }

    private void InitWindowedMode()
    {
        windowedModeCheckbox.AddCancelAction(DeselectWindowedMode);
        windowedModeCheckbox.currentValue = false;
    }

    #endregion

    #region VOLUME

    private void SelectVolume()
    {
        volumeSlider.isActive = true;
        controlButtonList.isActive = false;
    }

    private void DeselectVolume()
    {
        volumeSlider.isActive = false;
        controlButtonList.isActive = true;
    }

    #endregion

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
        languageDropDown.value = languageDropDown.currentValue;
    }

    #endregion

    #region SUBTITLES

    private void SelectSubtitles()
    {
        controlButtonList.isActive = false;
        subtitlesCheckbox.isActive = true;
    }

    private void DeselectSubtitles()
    {
        subtitlesCheckbox.isOn = subtitlesCheckbox.currentValue;
        controlButtonList.isActive = true;
        subtitlesCheckbox.isActive = false;
    }

    #endregion

    #region RESOLUTION

    private void SelectResolution()
    {
        controlButtonList.isActive = false;
        resolutionSelector.isActive = true;
    }

    private void DeselectResolution()
    {
        controlButtonList.isActive = true;
        resolutionSelector.isActive = false;
    }

    #endregion

    #region WINDOWED MODE

    private void SelectWindowedMode()
    {
        controlButtonList.isActive = false;
        windowedModeCheckbox.isActive = true;
    }

    private void DeselectWindowedMode()
    {
        controlButtonList.isActive = true;
        windowedModeCheckbox.isActive = false;
    }

    #endregion

    private void Accept()
    {
        Cancel();
    }

    private void Cancel()
    {
        controlButtonList.isActive = false;
        if(m_CancelAction != null)
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

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        controlButtonList.UpdateKey();
        volumeSlider.UpdateKey();
        languageDropDown.UpdateKey();
        subtitlesCheckbox.UpdateKey();
        resolutionSelector.UpdateKey();
        windowedModeCheckbox.UpdateKey();
    }
}
