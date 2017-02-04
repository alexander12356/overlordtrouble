using UnityEngine;

public class SwapItemAction : MonoBehaviour
{
    [SerializeField]
    private string m_NeedItemId ="";

    [SerializeField]
    private int m_NeedItemCount = 0;

    [SerializeField]
    private ActionStruct m_SuccessAction;

    [SerializeField]
    private ActionStruct m_FailAction;

    public void Run()
    {
        if (m_NeedItemId == "Monett" && PlayerInventory.GetInstance().coins >= m_NeedItemCount)
        {
            m_SuccessAction.actionEvent.Invoke();
        }
        else if (PlayerInventory.GetInstance().GetItemCount(m_NeedItemId) >= m_NeedItemCount)
        {
            m_SuccessAction.actionEvent.Invoke();
        }
        else
        {
            m_FailAction.actionEvent.Invoke();
        }
    }


}
