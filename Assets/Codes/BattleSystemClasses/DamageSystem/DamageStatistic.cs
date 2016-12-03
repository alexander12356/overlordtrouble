using System.Collections.Generic;

public struct DamageStatistic
{
    public BattleActor target;
    public List<string> resultText;

    public DamageStatistic(BattleActor p_Target, List<string> p_ResultText)
    {
        target = p_Target;
        resultText = p_ResultText;
    }
}
