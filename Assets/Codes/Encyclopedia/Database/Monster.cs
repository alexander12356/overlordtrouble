using UnityEngine;
using System.Collections;
[System.Serializable]
public class Monster : ScriptableObject {

	public string name = "";
	public string description = "";
	public int attack = 0;
	public int hp = 0;

	public Sprite monsterSprite;

	[ReadOnlyAttribute]public int ID = MonsterID.monsterID;


	[Range(0.0f,100.0f)]public float EpicPercent = .0f;
	[Range(0.0f,100.0f)]public float RarePercent = .0f;
	[Range(0.0f,100.0f)]public float SimplePercent = .0f;


	public float GoldMin = 0f;
	public float GoldMax = 0f;
	[ReadOnlyAttribute]public int GoldDrop;
}
