using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Warp : MonoBehaviour
{
    private Dictionary<string, RunActionBehavior> m_WarpBehaviors = new Dictionary<string, RunActionBehavior>();

    [SerializeField]
    private string m_Id = "";

    [SerializeField]
    private string m_TargetRoomId = string.Empty;
    
    [SerializeField]
    private Transform warpTarget = null;

    [SerializeField]
    private string m_CurrentBehaviorId = "";

    public string id
    {
        get { return m_Id; }
    }
    public string currentBehavior
    {
        get { return m_CurrentBehaviorId; }
    }
    
    public void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            RunActionBehavior l_Action = transform.GetChild(i).GetComponent<RunActionBehavior>();
            m_WarpBehaviors.Add(l_Action.name, l_Action);
        }
    }

    IEnumerator OnTriggerEnter2D(Collider2D otherCollider)
    {
        Transform collTransform = otherCollider.gameObject.transform.parent;
        if (collTransform.tag == "Player" && enabled)
        {
            ScreenFader l_ScreenFader = JourneySystem.GetInstance().panelManager.screenFader;

            collTransform.GetComponent<JourneyPlayer>().SetActive(false);

            yield return StartCoroutine(l_ScreenFader.FadeToBlack());

            if (m_CurrentBehaviorId != "")
            {
                m_WarpBehaviors[m_CurrentBehaviorId].RunAction(null);
            }

            RoomSystem.GetInstance().ChangeRoom(m_TargetRoomId);
            collTransform.position = warpTarget.position;
            Camera.main.transform.position = warpTarget.position;

            collTransform.GetComponent<JourneyPlayer>().SetActive(true);

            yield return StartCoroutine(l_ScreenFader.FadeToClear());
        }
        else yield break;
    }

    public void ChangeBehavior(string p_BehaviorId)
    {
        m_CurrentBehaviorId = p_BehaviorId;
    }
}
