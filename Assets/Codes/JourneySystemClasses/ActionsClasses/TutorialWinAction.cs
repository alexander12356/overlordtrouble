using UnityEngine;

public class TutorialWinAction : MonoBehaviour
{
    [SerializeField]
    private JourneyEnemy m_JourneyEnemy = null;

    [SerializeField]
    private ActionStruct m_NewLoseAction;

    public void Win()
    {
        CutsceneSystem.GetInstance().StartCutscene("TutorialWin");

        m_JourneyEnemy.loseAction = m_NewLoseAction;
    }
}
