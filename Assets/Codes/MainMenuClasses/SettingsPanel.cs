using System;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionCompare : IComparer<Resolution>
{
    public int Compare(Resolution x, Resolution y)
    {
        if (x.width > y.width)
        {
            return -1;
        }
        else if (x.width < y.width)
        {
            return 1;
        }
        else
        {
            if (x.height > y.height)
            {
                return -1;
            }
            else if (x.height < y.height)
            {
                return 1;
            }
        }
        return 0;
    }
}

public class SettingsPanel : Panel
{
    private static SettingsPanel m_Prefab = null;
    private ButtonList m_ControlButtonList = null;
    private ButtonList m_WindowButtonList = null;
    private PanelSlider m_EffectVolumeSlider = null;
    private PanelSlider m_MusicSlider = null;
    private PanelValueSelector m_ResolutionSelector = null;
    private PanelCheckbox m_WindowedModeCheckbox = null;
    private List<Resolution> m_SupportedResolutions = new List<Resolution>();
    private event PanelActionHandler m_CancelAction;
    private Resolution m_OldResolution;
    private bool m_OldIsFullScreen = false;

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
        get { return m_ControlButtonList; }
    }
    private ButtonList windowButtonList
    {
        get { return m_WindowButtonList; }
    }
    private PanelSlider musicVolumeSlider
    {
        get { return m_MusicSlider; }
    }
    private PanelSlider effectVolumeSlider
    {
        get { return m_EffectVolumeSlider; }
    }
    private PanelValueSelector resolutionSelector
    {
        get { return m_ResolutionSelector; }
    }
    private PanelCheckbox windowedModeCheckbox
    {
        get { return m_WindowedModeCheckbox; }
    }

    #endregion

    public override void Awake()
    {
        base.Awake();

        m_ControlButtonList = transform.FindChild("SettingsElements").GetComponent<ButtonList>();
        m_WindowButtonList = transform.FindChild("ControlButtons").GetComponent<ButtonList>();
        m_MusicSlider = transform.FindChild("SettingsElements").FindChild("VolumeMusic").GetComponentInChildren<PanelSlider>();
        m_EffectVolumeSlider = transform.FindChild("SettingsElements").FindChild("VolumeEffects").GetComponentInChildren<PanelSlider>();
        m_ResolutionSelector = transform.FindChild("SettingsElements").FindChild("Resolution").GetComponentInChildren<PanelValueSelector>();
        m_WindowedModeCheckbox = transform.FindChild("SettingsElements").FindChild("WindowedMode").GetComponentInChildren<PanelCheckbox>();
    }

    public virtual void Start()
    {
        InitOldOptions();
        InitControlButtons();
        InitVolume();
        InitResolution();
        InitWindowedMode();
    }

    #region INITS

    public virtual void InitOldOptions()
    {
        m_OldResolution = Screen.currentResolution;
        m_OldIsFullScreen = Screen.fullScreen;
    }

    public virtual void InitControlButtons()
    {
        controlButtonList[0].AddAction(SelectResolution);
        controlButtonList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:SettingsPanel:Resolution");
        controlButtonList[1].AddAction(ChangeWindowedMode);
        controlButtonList[1].title = LocalizationDataBase.GetInstance().GetText("GUI:SettingsPanel:WindowedMode");
        controlButtonList[2].AddAction(SelectEffectVolume);
        controlButtonList[2].title = LocalizationDataBase.GetInstance().GetText("GUI:SettingsPanel:EffectVolume");
        controlButtonList[3].AddAction(SelectMusicVolume);
        controlButtonList[3].title = LocalizationDataBase.GetInstance().GetText("GUI:SettingsPanel:MusicVolume");
        controlButtonList.isActive = true;

        windowButtonList[0].AddAction(Accept);
        windowButtonList[1].AddAction(Cancel);
        windowButtonList.isActive = false;
    }

    private void InitVolume()
    {
        musicVolumeSlider.AddCancelAction(DeselectMusicVolume);
        musicVolumeSlider.currentValue = AudioSystem.GetInstance().musicVolume;

        effectVolumeSlider.AddCancelAction(DeselectEffectVolume);
        effectVolumeSlider.currentValue = AudioSystem.GetInstance().soundVolume;
    }

    

    private void InitResolution()
    {
        m_SupportedResolutions = new List<Resolution>();

        foreach (Resolution l_Resolution in Screen.resolutions)
        {
            if (!m_SupportedResolutions.Contains(l_Resolution))
            {
                m_SupportedResolutions.Add(l_Resolution);
            }
        }
        ResolutionCompare l_ResolutionCompare = new ResolutionCompare();
        m_SupportedResolutions.Sort(l_ResolutionCompare);

        Dictionary<int, string> l_ResolutionList = new Dictionary<int, string>();
        for (int i = 0; i < m_SupportedResolutions.Count; i++)
        {
            l_ResolutionList.Add(i, m_SupportedResolutions[i].ToString());
        }

        resolutionSelector.values = l_ResolutionList;
        resolutionSelector.AddCancelAction(DeselectResolution);

        int l_CurrentResolutionIndex = m_SupportedResolutions.IndexOf(Screen.currentResolution);
        resolutionSelector.currentIndex = l_CurrentResolutionIndex;
    }

    private void InitWindowedMode()
    {
        windowedModeCheckbox.currentValue = false;
        windowedModeCheckbox.isOn = !Screen.fullScreen;
    }

    #endregion

    #region MUSIC_VOLUME

    public void ChangeMusicValue()
    {
        AudioSystem.GetInstance().ChangeMusicVolume(musicVolumeSlider.currentValue);
    }

    private void SelectMusicVolume()
    {
        musicVolumeSlider.isActive = true;
        controlButtonList.isActive = false;
    }

    private void DeselectMusicVolume()
    {
        musicVolumeSlider.isActive = false;
        controlButtonList.isActive = true;

        AudioSystem.GetInstance().ChangeMusicVolume(musicVolumeSlider.currentValue);
    }
    #endregion

    #region EFFECT_VOLUME

    private void SelectEffectVolume()
    {
        effectVolumeSlider.isActive = true;
        controlButtonList.isActive = false;
    }

    private void DeselectEffectVolume()
    {
        effectVolumeSlider.isActive = false;
        controlButtonList.isActive = true;
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

        Resolution l_NewResolution = m_SupportedResolutions[resolutionSelector.currentIndex];
        Screen.SetResolution(l_NewResolution.width, l_NewResolution.height, Screen.fullScreen);

        Debug.Log("CurrentResolution: " + Display.main.renderingWidth + " " + Display.main.renderingHeight);
    }

    #endregion

    #region WINDOWED MODE

    private void ChangeWindowedMode()
    {
        windowedModeCheckbox.Toggle();

        Screen.fullScreen = !windowedModeCheckbox.isOn;
    }

    #endregion

    public virtual void Accept()
    {
        Close();
    }

    private void Cancel()
    {
        Screen.SetResolution(m_OldResolution.width, m_OldResolution.height, m_OldIsFullScreen);

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

        if (windowButtonList.isActive && Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_ControlButtonList.isActive = true;
            m_WindowButtonList.isActive = false;
        }
        else if (controlButtonList.isActive && controlButtonList.currentButtonId == controlButtonList.count - 1 && Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_ControlButtonList.isActive = false;
            m_WindowButtonList.isActive = true;
        }
        else
        {
            controlButtonList.UpdateKey();
            windowButtonList.UpdateKey();
            musicVolumeSlider.UpdateKey();
            effectVolumeSlider.UpdateKey();
            resolutionSelector.UpdateKey();
        }
    }
}
