using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class SavePoint : MonoBehaviour
{
    private ActionStruct m_HealingAction;
    private ActionStruct m_SaveAction;

    [SerializeField]
    private string m_Id = "";

    public void Awake()
    {
        UnityEvent l_HealingEvent = new UnityEvent();
        l_HealingEvent.AddListener(Healing);

        UnityEvent l_SaveEvent = new UnityEvent();
        l_SaveEvent.AddListener(Save);

        m_HealingAction = new ActionStruct();
        m_HealingAction.id = "Talk";
        m_HealingAction.actionEvent = l_HealingEvent;

        m_SaveAction = new ActionStruct();
        m_SaveAction.id = "Yes";
        m_SaveAction.actionEvent = l_SaveEvent;
    }

    public void OnTriggerEnter2D(Collider2D p_OtherCollider)
    {
        Transform collTransform = p_OtherCollider.gameObject.transform.parent;
        if (collTransform.tag == "Player" && enabled)
        {
            DialogManager.GetInstance().StartDialog("Save", new List<ActionStruct>() { m_HealingAction, m_SaveAction });
            JourneySystem.GetInstance().SetControl(ControlType.Panel);
        }
    }

    private void Healing()
    {
        PlayerData.GetInstance().health = PlayerData.GetInstance().GetStats()["HealthPoints"];
        PlayerData.GetInstance().specialPoints = PlayerData.GetInstance().GetStats()["MonstylePoints"];
    }

    private void Save()
    {
        SaveSystem.GetInstance().SaveToFile(m_Id);
    }
}
