using UnityEngine;

public class BaseStep : MonoBehaviour
{
    [SerializeField]
    protected string m_StepId;

    public virtual void UpdateStep()
    {

    }

    public virtual void StartStep()
    {

    }

    public virtual void EndStep()
    {
        CutsceneSystem.GetInstance().NextStep();
    }
}
