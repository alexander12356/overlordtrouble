using UnityEngine;
using System.Collections;

public class RendererStep : BaseStep
{
    [SerializeField]
    private SpriteRenderer m_SpriteRenderer = null;

    [SerializeField]
    private bool m_Visisble = true;

    public override void StartStep()
    {
        base.StartStep();

        m_SpriteRenderer.enabled = m_Visisble;

        EndStep();
    }
}
