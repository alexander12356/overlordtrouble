using UnityEngine;

using System.Collections.Generic;

public class CutsceneSystem : MonoBehaviour
{
    private static CutsceneSystem m_Instance = null;
    private int m_CurrentStep = -1;
    private List<BaseStep> m_Steps = new List<BaseStep>();

    public static CutsceneSystem GetInstance()
    {
        return m_Instance;
    }

    public void Awake()
    {
        m_Instance = this;
        enabled    = false;
        LoadCutscene();
    }

    public void StartCutscene()
    {
        enabled = true;
        NextStep();
    }

    public void EndCutscene()
    {
        enabled = false;
        JourneySystem.GetInstance().SetControl(ControlType.Player);
    }

    public void NextStep()
    {
        m_CurrentStep++;

        if (m_CurrentStep > m_Steps.Count - 1)
        {
            EndCutscene();
            return;
        }

        m_Steps[m_CurrentStep].StartStep();
    }

    public void Update()
    {
        m_Steps[m_CurrentStep].UpdateStep();
    }

    private void LoadCutscene()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            m_Steps.Add(transform.GetChild(i).GetComponent<BaseStep>());
        }
    }
}
