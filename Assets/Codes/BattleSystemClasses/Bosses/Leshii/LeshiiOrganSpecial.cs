namespace BattleSystemClasses.Bosses.Leshii
{
    public class LeshiiOrganSpecial : LeshiiOrgan
    {
        public override bool IsCanDamage(float p_Damage)
        {
            LeshiiSpecial l_LeshiiSpecial = m_Leshii as LeshiiSpecial;

            if (l_LeshiiSpecial.isChargeMode)
            {
                if (m_OrganType == OrganType.Body)
                {
                    if (l_LeshiiSpecial.isLeftHandDied)
                    {
                        return true;
                    }
                    if (!isAoeAttack)
                    {
                        l_LeshiiSpecial.Block();
                    }
                    else
                    {
                        isAoeAttack = false;
                    }
                    return false;
                }
            }
            return true;
        }

        public override void CheckPrevAttack()
        {
            LeshiiSpecial l_LeshiiSpecial = m_Leshii as LeshiiSpecial;

            if (m_OrganType == OrganType.Body)
            {
                if (l_LeshiiSpecial.isChargeMode)
                {
                    if (!l_LeshiiSpecial.isLeftHandDied)
                    {
                        l_LeshiiSpecial.StartBlock();
                    }
                }
            }
        }
    }
}
