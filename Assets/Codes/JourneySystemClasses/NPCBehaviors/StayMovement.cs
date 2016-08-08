using UnityEngine;

public class StayMovement : BaseMovement
{
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private JourneyActor m_JourneyActor;
    private float m_WaitTime    = 0.0f;
    private float m_ElapsedTime = 0.0f;

    [SerializeField]
    private Direction m_PrevDirection = 0;

    public void Awake()
    {
        m_JourneyActor = GetComponent<JourneyActor>();
        m_WaitTime = GetNewWaitTime();

        ChangeDirection(m_PrevDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        m_ElapsedTime += Time.deltaTime;

        if (m_ElapsedTime >= m_WaitTime)
        {
            m_WaitTime = GetNewWaitTime();
            m_ElapsedTime = 0.0f;
            Direction l_NewDirection = GetNewDirection();

            while (l_NewDirection == m_PrevDirection)
            {
                l_NewDirection = GetNewDirection();
            }

            ChangeDirection(l_NewDirection);
        }
    }

    private void ChangeDirection(Direction p_NewDirection)
    {
        m_PrevDirection = p_NewDirection;
        m_JourneyActor.myAnimator.SetTrigger(p_NewDirection.ToString());
    }

    private Direction GetNewDirection()
    {
        return (Direction)Random.Range((int)Direction.Left, (int)Direction.Down);
    }

    private float GetNewWaitTime()
    {
        return Random.Range(2.0f, 5.0f);
    }
}
