using UnityEngine;

public class StayMovement : BaseMovement
{
    #region Variables

    private float m_WaitTime    = 0.0f;
    private float m_ElapsedTime = 0.0f;

    [SerializeField]
    private ActorDirection m_PrevDirection = 0;
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
            ActorDirection l_NewDirection = GetNewDirection();

            while (l_NewDirection == m_PrevDirection)
            {
                l_NewDirection = GetNewDirection();
            }

            ChangeDirection(l_NewDirection);
        }
    }
    #endregion

    #region Private
    private void ChangeDirection(ActorDirection p_NewDirection)
    {
        m_PrevDirection = p_NewDirection;
        journeyActor.SetDirection(p_NewDirection);
    }

    private ActorDirection GetNewDirection()
    {
        return (ActorDirection)Random.Range((int)ActorDirection.Right, (int)ActorDirection.Down);
    }

    private float GetNewWaitTime()
    {
        return Random.Range(1.0f, 2.5f);
    }
    #endregion
}
