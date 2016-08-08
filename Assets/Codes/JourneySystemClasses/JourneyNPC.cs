using UnityEngine;
using System;

public class JourneyNPC : JourneyActor
{
    #region Variables
    private CheckCollide   m_DialogCollide     = null;
    private Transform      m_SpeakorTransform  = null;
    private SpriteRenderer[] m_SpriteRenderers = null;

    [SerializeField]
    private string m_DialogId = "dialog for ";

    [SerializeField]
    private string m_NpcId = "TestNPC";

    [SerializeField]
    private BaseMovement m_BaseMovement = null;
    #endregion

    #region Interface
    public override void Awake()
    {
        base.Awake();

        m_DialogCollide = GetComponentInChildren<CheckCollide>();
        m_DialogCollide.SetCollideEnterAction(DialogReady);
        m_DialogCollide.SetCollideExitAction(DialogNotReady);

        m_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);

        if (!m_BaseMovement)
        {
            Debug.LogWarning("Movement logic is null");
        }
    }

    public override void Update()
    {
        base.Update();

        if (m_BaseMovement)
        {
            m_BaseMovement.LogicUpdate();
        }
    }

    public void StartDialog()
    {
        DialogManager.StartDialog(m_DialogId);
        ApplyTo(m_SpeakorTransform.position);
    }
    #endregion

    #region Private
    private void DialogReady(JourneyPlayer p_JourneyPlayer)
    {
        p_JourneyPlayer.AddActiveButtonAction(StartDialog);

        m_SpeakorTransform = p_JourneyPlayer.myTransform;

        m_SpriteRenderers[1].gameObject.SetActive(true);
    }

    private void DialogNotReady(JourneyPlayer p_JourneyPlayer)
    {
        p_JourneyPlayer.RemoveActiveButtonAction(StartDialog);

        m_SpriteRenderers[1].gameObject.SetActive(false);
    }

    private void ApplyTo(Vector3 p_Target)
    {
        Vector2 l_Position = myTransform.position;
        Vector2 l_SpeakerPosition = m_SpeakorTransform.position;
        double l_Angle = Math.Atan2(l_Position.y - l_SpeakerPosition.y, l_Position.x - l_SpeakerPosition.x) / Math.PI * 180;
        l_Angle = (l_Angle < 0) ? l_Angle + 360 : l_Angle;

        if ((l_Angle > 315.0f && l_Angle < 360.0f) || (l_Angle > 0.0f && l_Angle < 45.0f))
        {
            m_Animator.SetTrigger("Left");
        }
        else if (l_Angle > 45.0f && l_Angle < 135.0f)
        {
            m_Animator.SetTrigger("Down");
        }
        else if (l_Angle > 135.0f && l_Angle < 225.0f)
        {
            m_Animator.SetTrigger("Right");
        }
        else if (l_Angle > 225.0f && l_Angle < 315.0f)
        {
            m_Animator.SetTrigger("Up");
        }
    }
    #endregion
}