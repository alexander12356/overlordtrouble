using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLoseAction : MonoBehaviour
{
    public void Lose()
    {
        CutsceneSystem.GetInstance().StartCutscene("TutorialLose");
    }
}
