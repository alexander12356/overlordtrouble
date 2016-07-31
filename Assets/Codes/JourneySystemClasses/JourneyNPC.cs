using UnityEngine;

public class JourneyNPC : JourneyActor
{
    #region Variables
    //private CheckCollide m_DialogCollide = null;
    private bool m_DialogReady = false;

    [SerializeField]
    private string m_DialogId = "dialog for ";

    [SerializeField]
    private string m_NpcId = "TestNPC";
    #endregion

    #region Interface
    public override void Awake()
    {
        base.Awake();

        //m_DialogCollide = GetComponentInChildren<CheckCollide>();
    }

    public override void Update()
    {
        base.Update();
    }

    public void StartDialog()
    {

    }
    #endregion

    #region Private
    private void DialogReady()
    {

    }

    private void DialogNotReady()
    {

    }
    #endregion
}