using UnityEngine;
using UnityEngine.UI;

public class EnterNameButton : PanelButton
{
    private Animator m_Animator = null;

    public override void Awake()
    {
        m_SelectedImage = transform.FindChild("SelectImage").GetComponent<Image>();
        m_Animator = GetComponent<Animator>();
    }

    public override void RunAction()
    {
        m_Animator.SetTrigger("Bumped");

        base.RunAction();
    }
}
