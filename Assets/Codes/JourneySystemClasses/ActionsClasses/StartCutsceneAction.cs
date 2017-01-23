using UnityEngine;

public class StartCutsceneAction : MonoBehaviour
{
    [SerializeField]
    private string p_CutsceneId = string.Empty;

    public void StartCutscene()
    {
        CutsceneSystem.GetInstance().StartCutscene(p_CutsceneId);
    }
}
