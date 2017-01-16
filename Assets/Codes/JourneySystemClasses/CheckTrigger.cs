using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTrigger : MonoBehaviour
{
    private Collider2D m_Collider;

    [SerializeField]
    private JourneyPlayer m_JourneyPlayer;

    public BoxCollider2D checkCollider
    {
        get { return m_Collider as BoxCollider2D; }
    }

    public void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        m_JourneyPlayer.SetInteractActor(collision.GetComponentInParent<JourneyActor>());
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        m_JourneyPlayer.RemoveInteractActor(collision.GetComponentInParent<JourneyActor>());
    }
}
