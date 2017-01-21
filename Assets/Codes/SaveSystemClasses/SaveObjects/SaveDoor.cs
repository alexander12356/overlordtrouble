using UnityEngine;

public class SaveDoor : MonoBehaviour
{
    public void Awake()
    {
        FrontDoor l_FrontDoor = GetComponent<FrontDoor>();
        SaveSystem.GetInstance().AddDoor(l_FrontDoor);
    }
}
