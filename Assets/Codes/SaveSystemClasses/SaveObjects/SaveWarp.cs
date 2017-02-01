using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveWarp : MonoBehaviour
{
    public void Awake()
    {
        Warp l_Warp = GetComponent<Warp>();

        SaveSystem.GetInstance().AddWarp(l_Warp);
    }
}
