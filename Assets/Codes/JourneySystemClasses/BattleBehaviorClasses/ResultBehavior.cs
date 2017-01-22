using UnityEngine;
using UnityEngine.Events;

public class ResultBehavior : MonoBehaviour
{
    [SerializeField]
    private ActionStruct m_ResultBehavior;
	
    public string id
    {
        get { return m_ResultBehavior.id; }
    }

    public UnityEvent actionEvent
    {
        get { return m_ResultBehavior.actionEvent; }
    }
}
