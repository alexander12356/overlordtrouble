using UnityEngine;

using System.Collections;

public class LocationWarp : MonoBehaviour
{
    [SerializeField]
    private string m_TargetLocationId;

    [SerializeField]
    private string m_TargetRoomId;

    [SerializeField]
    private string m_SenderLocationId;

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Transform collTransform = otherCollider.gameObject.transform.parent;
        if (collTransform.tag == "Player" && enabled)
        {
            AudioSystem.GetInstance().PlaySound("ChangeLocation");

            PlayerPrefs.SetString("SenderLocation", m_SenderLocationId);
            PlayerPrefs.SetString("TargetRoomId", m_TargetRoomId);
            JourneySystem.GetInstance().StartLocation(m_TargetLocationId);
        }
    }
}
