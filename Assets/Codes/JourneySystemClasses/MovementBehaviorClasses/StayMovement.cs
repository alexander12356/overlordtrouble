using UnityEngine;

public class StayMovement : BaseMovement
{
    #region Variables
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private float m_WaitTime    = 0.0f;
    private float m_ElapsedTime = 0.0f;

    [SerializeField]
    private Direction m_PrevDirection = 0;
    #endregion

    #region Interface
    public override void Awake()
    {
        base.Awake();
        
    }

    public override void Start()
    {
        ChangeDirection(m_PrevDirection);
        m_WaitTime = GetNewWaitTime();
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
    #endregion

    #region Private
    private void ChangeDirection(Direction p_NewDirection)
    {
        m_PrevDirection = p_NewDirection;
        journeyActor.myAnimator.SetTrigger(p_NewDirection.ToString());
    }

    private Direction GetNewDirection()
    {
        return (Direction)Random.Range((int)Direction.Left, (int)Direction.Down);
    }

    private float GetNewWaitTime()
    {
        return Random.Range(1.0f, 2.5f);
    }
    #endregion
}
