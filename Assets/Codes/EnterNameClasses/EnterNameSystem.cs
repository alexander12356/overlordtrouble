using System.Collections.Generic;

using UnityEngine;

public class EnterNameSystem : MonoBehaviour
{
    private static EnterNameSystem m_Instance = null;
    private MovieTexture m_MovieTexture = null;
    private int m_PrevVSyncCount = 0;
    private bool m_VideoIsOver = false;

    [SerializeField]
    private MeshRenderer m_MeshRenderer = null;

    [SerializeField]
    private PanelManager m_PanelManager = null;

    public static EnterNameSystem GetInstance()
    {
        return m_Instance;
    }

    public void Awake()
    {
        m_Instance = this;
        m_MovieTexture = m_MeshRenderer.material.mainTexture as MovieTexture;
    }

	public void Start ()
    {
        m_PrevVSyncCount = QualitySettings.vSyncCount;
        QualitySettings.vSyncCount = 0;
        m_MovieTexture.Play();
    }

    public void Update()
    {
        if (!m_MovieTexture.isPlaying && !m_VideoIsOver)
        {
            QualitySettings.vSyncCount = m_PrevVSyncCount;
            m_VideoIsOver = true;
            m_MeshRenderer.gameObject.SetActive(false);

            EnterNameTextPanel l_TextPanel = Instantiate(EnterNameTextPanel.prefab);
            l_TextPanel.AddButtonAction(l_TextPanel.Close);
            l_TextPanel.AddButtonAction(ShowEnterNamePanel);
            l_TextPanel.SetText(new List<string>() { LocalizationDataBase.GetInstance().GetText("GUI:EnterName:EnterName") });
            ShowPanel(l_TextPanel);
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            m_MovieTexture.Stop();
        }
    }

    public void ShowPanel(Panel p_Panel, bool p_WithOverlay = false)
    {
        m_PanelManager.ShowPanel(p_Panel, p_WithOverlay);
    }

    public void StartGame()
    {
        m_PanelManager.StartLocation("Town");
    }

    private void ShowEnterNamePanel()
    {
        EnterNamePanel l_EnterNamePanel = Instantiate(EnterNamePanel.prefab);
        ShowPanel(l_EnterNamePanel);
    }
}
