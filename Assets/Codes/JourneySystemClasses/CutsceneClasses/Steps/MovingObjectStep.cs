using UnityEngine;
using System.Collections;

public class MovingObjectStep : BaseStep
{
    private MonoBehaviour[] m_Scripts;

    [SerializeField]
    private Transform m_ObjectTransform = null;

    [SerializeField]
    private Vector3 m_DestPosition = Vector3.zero;

    [SerializeField]
    private float m_Speed = 0.0f;

    public void Awake()
    {
        m_Scripts = m_ObjectTransform.gameObject.GetComponents<MonoBehaviour>();
    }

    public override void StartStep()
    {
        base.StartStep();

        LogicEnable(false);
    }

    public override void UpdateStep()
    {
        base.UpdateStep();

        m_ObjectTransform.localPosition = Vector3.MoveTowards(m_ObjectTransform.localPosition, m_DestPosition, m_Speed * Time.deltaTime);

        if ((m_ObjectTransform.localPosition - m_DestPosition).sqrMagnitude < 0.25f)
        {
            LogicEnable(true);
            EndStep();
        }
    }

    private void LogicEnable(bool p_Value)
    {
        for (int i = 0; i < m_Scripts.Length; i++)
        {
            m_Scripts[i].enabled = p_Value;
        }
    }
}
