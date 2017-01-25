using UnityEngine;

public class JourneyNPC : JourneyActor, IPausable
{
    #region Interface

    public override void Start()
    {
        base.Start();

        JourneySystem.GetInstance().AddOnPauseListener(OnPause);
        JourneySystem.GetInstance().AddOnResumeListener(OnResume);
    }

    public override void GoTo(Vector3 p_Target, float p_Delay)
    {
        base.GoTo(p_Target, p_Delay);

        UpdateSortingLayer();
        myTransform.position = Vector3.MoveTowards(myTransform.position, p_Target, p_Delay);

        float l_DeltaX = myTransform.position.x - p_Target.x;
        float l_DeltaY = myTransform.position.y - p_Target.y;
        if (Mathf.Abs(l_DeltaX) > Mathf.Abs(l_DeltaY))
        {
            if (l_DeltaX > 0)
            {
                myAnimator.SetFloat("Input_X", -1);
                myAnimator.SetFloat("Input_Y", 0);
            }
            else
            {
                myAnimator.SetFloat("Input_X", 1);
                myAnimator.SetFloat("Input_Y", 0);
            }
        }
        else
        {
            if (l_DeltaY > 0)
            {
                myAnimator.SetFloat("Input_X", 0);
                myAnimator.SetFloat("Input_Y", -1);
            }
            else
            {
                myAnimator.SetFloat("Input_X", 0);
                myAnimator.SetFloat("Input_Y", 1);
            }
        }
    }

    public void OnPause()
    {
        StopLogic();
        m_Animator.enabled = false;
    }

    public void OnResume()
    {
        m_Animator.enabled = true;
        StartLogic();
    }

    public void OnDestroy()
    {
        JourneySystem.GetInstance().RemoveOnPauseListener(OnPause);
        JourneySystem.GetInstance().RemoveOnResumeListener(OnResume);
    }
    #endregion
}