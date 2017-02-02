using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAnimation : MonoBehaviour
{
    public void Awake()
    {
        AnimationObject l_AnimationObject = GetComponent<AnimationObject>();

        SaveSystem.GetInstance().AddAnimationObject(l_AnimationObject);
    }	
}
