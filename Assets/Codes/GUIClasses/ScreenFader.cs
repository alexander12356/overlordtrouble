﻿using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour {

    Animator anim;
    bool isFading = false;
	
    // Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}

    public IEnumerator FadeToClear()
    {
        isFading = true;
        anim.SetTrigger("FadeIn");

        while (isFading)
            yield return null;
    }

    public IEnumerator FadeToBlack()
    {
        isFading = true;
        anim.SetTrigger("FadeOut");

        while (isFading)
            yield return null;
    }

    // Called by Animation
    void AnimationComplete()
    {
        Debug.Log("Animation Complete");
        isFading = false;
    }
}