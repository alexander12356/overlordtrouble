public class StunPossibility : StunEffect
{
    private int m_StunChance = 0;

    public StunPossibility(Special p_Special, int p_StunChance) : base(p_Special)
    {
        id = "Stun";
        m_StunChance = p_StunChance;
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        System.Random l_Random = new System.Random();
        int l_CurrentStunChance = l_Random.Next(0, 100);

        if (l_CurrentStunChance < m_StunChance)
        {
            m_Player = (BattlePlayer)p_Target;

            if (m_Player.HasSpecial(m_Special.id))
            {
                m_Player.StackEffect(m_Special.id, this);
            }
            else
            {
                m_Player.monstyleCapacity = 3;
                m_Player.AddEffect(m_Special.id, this);
                m_Player.AddStatusEffectIcon(id);
            }

            DamageSystem.GetInstance().AddEffectSpecial(m_Player, m_Special);

            UnityEngine.Debug.Log("Stun for Player");
        }
    }
}
