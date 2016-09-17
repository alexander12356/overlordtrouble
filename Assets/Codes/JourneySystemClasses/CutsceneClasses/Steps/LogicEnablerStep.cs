using UnityEngine;
using System.Collections;

public class LogicEnablerStep : BaseStep
{
    [SerializeField]
    private MonoBehaviour m_Logic = null;

    [SerializeField]
    private bool m_Enable = true;

    public override void StartStep()
    {
        base.StartStep();

        m_Logic.enabled = m_Enable;

        EndStep();
    }
}
