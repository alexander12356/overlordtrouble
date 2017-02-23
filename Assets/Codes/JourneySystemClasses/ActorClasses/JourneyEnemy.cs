using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class JourneyEnemy : JourneyNPC
{
    private Dictionary<string, ResultBehavior> m_WinActions = new Dictionary<string, ResultBehavior>();
    private Dictionary<string, ResultBehavior> m_LoseActions = new Dictionary<string, ResultBehavior>();
    private string[] m_CustomBattleIds = null;

    [SerializeField]
    private float m_StartLogicCooldown = 2.0f;

    [SerializeField]
    private string m_WinBehaviorId = "";

    [SerializeField]
    private string m_LoseBehaviorId = "";

    public string winBehaviorId
    {
        get { return m_WinBehaviorId; }
    }
    public string loseBehaviorId
    {
        get { return m_LoseBehaviorId; }
    }

    public string[] customBattleIds
    {
        get
        {
            return m_CustomBattleIds;
        }
        set
        {
            m_CustomBattleIds = value;
        }
    }

    public override void Awake()
    {
        base.Awake();

        Transform l_WinBehaviorTransform = myTransform.FindChild("WinBehavior");
        if (l_WinBehaviorTransform != null)
        {
            for (int i = 0; i < l_WinBehaviorTransform.childCount; i++)
            {
                ResultBehavior l_ActionStruct = l_WinBehaviorTransform.GetChild(i).GetComponent<ResultBehavior>();
                m_WinActions.Add(l_ActionStruct.id, l_ActionStruct);
            }
        }

        Transform l_LoseBehaviorTransform = myTransform.FindChild("LoseBehavior");
        if (l_LoseBehaviorTransform != null)
        {
            for (int i = 0; i < l_LoseBehaviorTransform.childCount; i++)
            {
                ResultBehavior l_ActionStruct = l_LoseBehaviorTransform.GetChild(i).GetComponent<ResultBehavior>();
                m_LoseActions.Add(l_ActionStruct.id, l_ActionStruct);
            }
        }
    }

    public void ChangeWinBehavior(string p_Id)
    {
        m_WinBehaviorId = p_Id;
    }

    public void ChangeLoseBehavior(string p_Id)
    {
        m_LoseBehaviorId = p_Id;
    }

    public void Win()
    {
        if (m_WinBehaviorId != "")
        {
            m_WinActions[m_WinBehaviorId].actionEvent.Invoke();
        }
    }

    public void Lose()
    {
        if (m_LoseBehaviorId != "")
        {
            m_LoseActions[m_LoseBehaviorId].actionEvent.Invoke();
        }
    }

    public void PlayerRetreated()
    {
        GameManager.GetInstance().StartCoroutine(WaitingLogicStart(m_StartLogicCooldown));
    }

    private IEnumerator WaitingLogicStart(float m_Time)
    {
        yield return new WaitForSeconds(m_Time);

        StartLogic();
    }
}
