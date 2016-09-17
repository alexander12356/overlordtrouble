using UnityEngine;

public class MovingActorStep : BaseStep
{
    [SerializeField]
    protected JourneyActor m_JourneyActor;

    [SerializeField]
    private Vector3 m_DestPosition = Vector3.zero;

    [SerializeField]
    private string m_AnimationName = string.Empty;

    [SerializeField]
    private float m_Speed = 0.0f;

    public override void UpdateStep()
    {
        base.UpdateStep();

        m_JourneyActor.myTransform.localPosition = Vector3.MoveTowards(m_JourneyActor.myTransform.localPosition, m_DestPosition, m_Speed * Time.deltaTime);
        m_JourneyActor.myAnimator.SetBool(m_AnimationName, true);

        if ((m_JourneyActor.myTransform.localPosition - m_DestPosition).sqrMagnitude < 0.25f)
        {
            EndStep();
            m_JourneyActor.myAnimator.SetBool(m_AnimationName, false);
        }
    }
}
