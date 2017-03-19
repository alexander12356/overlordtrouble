using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAction : MonoBehaviour
{
    public void Play(string p_Id)
    {
        AudioSystem.GetInstance().PlaySound(p_Id);
    }
}
