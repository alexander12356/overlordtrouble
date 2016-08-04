using UnityEngine;
using System.Collections;
[System.Serializable]
public class Monster : ScriptableObject {
	[ReadOnlyAttribute]public int ID = MonsterID.monsterID;
	public string name = "";
	public string description = "";
	public float attackMin = 0f;
	public float attackMax = 0f;
	public float attack = 0f;
	public int hp = 0;
	public int Defence = 0;

	public bool Elemental = false;
	public bool Dark = false; 
	public bool Light = false;
	public bool Fire = false;
	public bool Water = false;
	public bool Earth = false;
	public bool Air = false;


	public Animator anim;

	public Sprite monsterSprite;

	[Range(0.0f,100.0f)]public float EpicPercent = .0f;
	[Range(0.0f,100.0f)]public float RarePercent = .0f;
	[Range(0.0f,100.0f)]public float SimplePercent = .0f;


	public float GoldMin = 0f;
	public float GoldMax = 0f;
	[ReadOnlyAttribute]public int GoldDrop;
}
