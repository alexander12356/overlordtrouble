using UnityEngine;

public class ChangeObjectPositionAction : MonoBehaviour
{
    [SerializeField]
    private GameObject m_TargetGameObject;

    [SerializeField]
    private Transform m_TargetPosition;

    public void Run()
    {
        m_TargetGameObject.transform.position = m_TargetPosition.position;
    }
}
