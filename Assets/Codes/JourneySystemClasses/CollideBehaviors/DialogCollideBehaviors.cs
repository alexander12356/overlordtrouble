using UnityEngine;
using System.Collections;

public class DialogCollideBehaviors : BaseCollideBehaviors
{
    [SerializeField]
    private string m_DialogId = string.Empty;

    [SerializeField]
    private SpriteRenderer m_ActiveButtonImage = null;
    private Vector3 m_PlayerPosition;

    public override void Awake()
    {
        base.Awake();
    }

    public override void EnterAction(JourneyPlayer p_JourneyPlayer)
    {
        base.EnterAction(p_JourneyPlayer);

        m_PlayerPosition = p_JourneyPlayer.myTransform.position;
        p_JourneyPlayer.AddActiveButtonAction(StartDialog);
        p_JourneyPlayer.AddDisactiveButtonAction(EndDialog);

        m_ActiveButtonImage.enabled = true;
    }

    public override void ExitAction(JourneyPlayer p_JourneyPlayer)
    {
        base.ExitAction(p_JourneyPlayer);

        p_JourneyPlayer.RemoveActiveButtonAction(StartDialog);
        p_JourneyPlayer.RemoveDisactiveButtonAction(EndDialog);

        m_ActiveButtonImage.enabled = false;
    }

    private void StartDialog()
    {
        JourneySystem.GetInstance().StartDialog(m_DialogId);
        m_JourneyActor.ApplyTo(m_PlayerPosition);
        m_JourneyActor.StopLogic();
    }

    private void EndDialog()
    {
        m_JourneyActor.StartLogic();
    }
}
