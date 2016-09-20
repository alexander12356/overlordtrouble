using UnityEngine;

using System.Collections.Generic;

public class CutsceneSystem : MonoBehaviour
{
    private static CutsceneSystem m_Instance = null;
    private string m_CurrentCutscene = string.Empty;
    private int m_CurrentStep = -1;
    private Dictionary<string, List<BaseStep>> m_Cutscenes = new Dictionary<string, List<BaseStep>>();

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

    public void StartCutscene(string p_Id)
    {
        JourneySystem.GetInstance().SetControl(ControlType.Cutscene);

        m_CurrentCutscene = p_Id;
        m_CurrentStep = -1;
        
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

        if (m_CurrentStep > m_Cutscenes[m_CurrentCutscene].Count - 1)
        {
            EndCutscene();
            return;
        }

        m_Cutscenes[m_CurrentCutscene][m_CurrentStep].StartStep();
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            Skip();
        }

        m_Cutscenes[m_CurrentCutscene][m_CurrentStep].UpdateStep();
    }

    private void LoadCutscene()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform l_Cutscene = transform.GetChild(i).transform;
            List<BaseStep> l_Steps = new List<BaseStep>();
            for (int j = 0; j < l_Cutscene.childCount; j++)
            {
                l_Steps.Add(l_Cutscene.GetChild(j).GetComponent<BaseStep>());
            }
            m_Cutscenes.Add(l_Cutscene.gameObject.name, l_Steps);
        }
    }

    public void Skip()
    {
        EndCutscene();
        StartCutscene("Skip");
    }
}
