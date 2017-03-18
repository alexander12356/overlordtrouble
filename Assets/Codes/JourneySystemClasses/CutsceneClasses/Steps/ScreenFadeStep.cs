using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFadeStep : BaseStep
{
    [SerializeField]
    private ScreenFader m_ScreenFader = null;

    [SerializeField]
    private bool m_ToBlack = false;

    [SerializeField]
    private Color m_ScreenColor = Color.black;

    public override void StartStep()
    {
        base.StartStep();

        StartCoroutine(FadeScreen());
    }

    private IEnumerator FadeScreen()
    {
        if (m_ToBlack)
        {
            yield return m_ScreenFader.FadeToBlack();
        }
        else
        {
            yield return m_ScreenFader.FadeToClear();
        }

        EndStep();
    }
}
