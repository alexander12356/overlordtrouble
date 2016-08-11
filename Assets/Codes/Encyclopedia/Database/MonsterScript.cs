using UnityEngine;
using System.Collections;

public class MonsterScript : MonoBehaviour {
	public int ID;
	public string name = "";
	public string description = "";
	public float attackMin = 0f;
	public float attackMax = 0f;
	public float attack = 0f;
	public int hp = 0;
	public int Defence = 0;

	public string Elements;

	public bool Elemental = false;
	public bool Dark = false; 
	public bool Light = false;
	public bool Fire = false;
	public bool Water = false;
	public bool Earth = false;
	public bool Air = false;


	public Sprite monsterSprite;
	public Animator anim;

	[SerializeField][Range(0.0f,100.0f)]public float EpicPercent = .0f;
	[SerializeField][Range(0.0f,100.0f)]public float RarePercent = .0f;
	[SerializeField][Range(0.0f,100.0f)]public float SimplePercent = .0f;


	[SerializeField]public float GoldMin = 0f;
	[SerializeField]public float GoldMax = 0f;
	public int GoldDrop;

	public bool Visability = false;


	void Start () {
	}

	void Update () {
		
	}
}

