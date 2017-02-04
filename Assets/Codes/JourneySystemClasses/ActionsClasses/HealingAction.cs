using UnityEngine;

public class HealingAction : MonoBehaviour
{
	public void Run()
    {
        PlayerData.GetInstance().health = PlayerData.GetInstance().GetStats()["HealthPoints"];
        PlayerData.GetInstance().specialPoints = PlayerData.GetInstance().GetStats()["MonstylePoints"];
    }
}
