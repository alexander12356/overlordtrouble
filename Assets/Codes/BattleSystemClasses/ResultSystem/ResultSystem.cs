using System.Collections.Generic;

public class ResultSystem : Singleton<ResultSystem>
{
    private Queue<BattleBaseStep> m_StepQueue = new Queue<BattleBaseStep>();

    public void ShowResult()
    {
        BattleSystem.GetInstance().CloseMainPanel();
        NextStep();
    }

    public void AddStep(BattleBaseStep p_Step)
    {
        m_StepQueue.Enqueue(p_Step);
    }

    public void NextStep()
    {
        if (m_StepQueue.Count == 0)
        {
            EndResult();
            return;
        }
        m_StepQueue.Dequeue().RunStep();
    }

    private void EndResult()
    {
        m_StepQueue.Clear();
        TurnSystem.GetInstance().NextActorRun();
    }
}
