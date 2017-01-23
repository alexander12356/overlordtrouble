using UnityEngine;

public class MovingActorStep : BaseStep
{
    [SerializeField]
    protected JourneyActor m_JourneyActor;

    [SerializeField]
    private string m_AnimationName = string.Empty;

    [SerializeField]
    private float m_Speed = 0.0f;

    public override void UpdateStep()
    {
        base.UpdateStep();
        
        m_JourneyActor.myAnimator.SetBool(m_AnimationName, true);
        m_JourneyActor.GoTo(transform.position, m_Speed * Time.deltaTime);

        if ((m_JourneyActor.myTransform.position - transform.position).sqrMagnitude < 0.05f)
        {
            EndStep();
            m_JourneyActor.myAnimator.SetBool(m_AnimationName, false);
        }
    }
}
