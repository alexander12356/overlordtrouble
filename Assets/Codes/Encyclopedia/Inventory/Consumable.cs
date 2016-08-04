#if UNITY_EDITOR
using UnityEngine;
[System.Serializable]
public class Consumable : Item {
	public int Charges = 0;
}
#endif