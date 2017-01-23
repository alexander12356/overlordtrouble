using UnityEngine;

public class EnemyHuntBehavior : BaseMovement
{
    private enum HuntState
    {
        Wait,
        Hunt,
        ReturnToHome
    }

    private Vector3 m_StartPosition = Vector3.zero;
    private HuntState m_HuntState = HuntState.Wait;
    private JourneyPlayer m_JourneyPlayer = null;
    private CheckCollide m_BattleTrigger = null;
    private CheckCollide m_HuntTrigger = null;

    [SerializeField]
    private float m_MaxHuntDistance = 3.0f;

    [SerializeField]
    private float m_Speed = 4.0f;

    public override void Awake()
    {
        base.Awake();

        m_HuntTrigger = transform.FindChild("HuntTrigger").GetComponent<CheckCollide>();
        m_BattleTrigger = transform.FindChild("BattleTrigger").GetComponent<CheckCollide>();
    }

    public override void Start()
    {
        base.Start();

        m_StartPosition = journeyActor.myTransform.position;
        m_HuntTrigger.AddCollideEnterAction(StartHunt);
        m_BattleTrigger.AddCollideEnterAction(StartBattle);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        switch (m_HuntState)
        {
            case HuntState.Wait:
                journeyActor.myAnimator.SetBool("IsWalking", false);
                break;
            case HuntState.Hunt:
                if ((m_StartPosition - m_JourneyPlayer.myTransform.position).sqrMagnitude < (m_MaxHuntDistance * m_MaxHuntDistance) && m_JourneyPlayer.enabled)
                {
                    journeyActor.myAnimator.SetBool("IsWalking", true);
                    journeyActor.GoTo(m_JourneyPlayer.myTransform.position, m_Speed * Time.deltaTime);
                }
                else
                {
                    m_JourneyPlayer = null;
                    m_HuntState = HuntState.ReturnToHome;
                }
                break;
            case HuntState.ReturnToHome:
                journeyActor.GoTo(m_StartPosition, m_Speed * Time.deltaTime);
                if (journeyActor.myTransform.position == m_StartPosition)
                {
                    m_HuntState = HuntState.Wait;
                }
                break;
        }
    }

    public override void LogicStart()
    {
        m_HuntTrigger.AddCollideEnterAction(StartHunt);
        m_BattleTrigger.AddCollideEnterAction(StartBattle);
    }

    public override void LogicStop()
    {
        m_HuntTrigger.RemoveCollideEnterAction(StartHunt);
        m_BattleTrigger.RemoveCollideEnterAction(StartBattle);
    }

    private void StartHunt(JourneyActor m_JourneyActor)
    {
        m_HuntState = HuntState.Hunt;
        m_JourneyPlayer = m_JourneyActor as JourneyPlayer;
    }

    private void StartBattle(JourneyActor m_JourneyActor)
    {
        journeyActor.myAnimator.SetBool("IsWalking", false);
        journeyActor.Interact(m_JourneyActor);
    }
}
