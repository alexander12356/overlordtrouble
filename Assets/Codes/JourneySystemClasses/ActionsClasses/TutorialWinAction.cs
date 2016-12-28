using UnityEngine;

public class TutorialWinAction : MonoBehaviour
{
    public void Win()
    {
        CutsceneSystem.GetInstance().StartCutscene("TutorialWin");
    }
}
