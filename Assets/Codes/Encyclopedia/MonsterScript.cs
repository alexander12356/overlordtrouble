using UnityEngine;
using System.Collections;

public class MonsterScript : MonoBehaviour {
	public int ID;
	public string name = "";
	public string description = "";

	public bool Elemental = false;
	public bool Dark = false; 
	public bool Light = false;
	public bool Fire = false;
	public bool Water = false;
	public bool Earth = false;
	public bool Air = false;


	public Sprite monsterSprite;

	public bool Visability = false;
}

