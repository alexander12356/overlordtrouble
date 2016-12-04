using UnityEngine;
using System.Collections.Generic;

namespace BattleSystemClasses.Bosses.Leshii
{
    public enum OrganIds
    {
        NONE = -1,
        RightHand,
        LeftHand,
        Body,
        Headmain
    }

    public class Leshii : BattleEnemy
    {
        private Animator m_BodyAnimator = null;
        private Animator m_HeadAnimator = null;
        private Vector2 m_HandsLive = Vector2.zero;
        private int m_SummonHandsCounter = 0;
        private int m_SummonHandsCount = 3;

        [SerializeField]
        private LeshiiOrgan m_RightHand = null;

        [SerializeField]
        private LeshiiOrgan m_LeftHand = null;

        [SerializeField]
        private LeshiiOrgan m_Body = null;

        public Animator bodyAnimator
        {
            get { return m_BodyAnimator; }
        }
        public Animator headAnimator
        {
            get { return m_HeadAnimator; }
        }

        public override void InitStats()
        {
            actorName = LocalizationDataBase.GetInstance().GetText("Boss:Leshii");

            m_RightHand.Init(OrganIds.RightHand, this);
            m_LeftHand.Init(OrganIds.LeftHand, this);
            m_Body.Init(OrganIds.Body, this);

            m_BodyAnimator = GetComponent<Animator>();
            m_HeadAnimator = transform.FindChild(OrganIds.Headmain.ToString()).GetComponentInChildren<Animator>();

            CalculateIdle(m_HandsLive);
        }
        
        public List<LeshiiOrgan> GetOrgans()
        {
            List<LeshiiOrgan> l_LeshiiOrgans = new List<LeshiiOrgan>() { m_RightHand, m_Body, m_LeftHand };
            return l_LeshiiOrgans;
        }

        public override void RunTurn()
        {
            if (IsAllHandsDied())
            {
                if (m_SummonHandsCounter < m_SummonHandsCount)
                {
                    List<string> l_Text = new List<string>();
                    string l_SummonHandsText = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:SummonHands", new string[] { (m_SummonHandsCount - m_SummonHandsCounter).ToString() });
                    l_Text.Add(l_SummonHandsText);

                    TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
                    l_TextPanel.SetTalkingAnimator(m_HeadAnimator, "Talking");
                    l_TextPanel.SetText(l_Text);
                    l_TextPanel.AddButtonAction(l_TextPanel.Close);

                    ResultSystem.GetInstance().AddTextPanel(l_TextPanel);

                    m_SummonHandsCounter++;
                }
                else
                {
                    m_SummonHandsCounter = 0;
                    m_BodyAnimator.SetTrigger("SummonHands");
                    m_HandsLive = Vector2.zero;
                    m_LeftHand.Recovery();
                    m_RightHand.Recovery();
                    CalculateIdle(m_HandsLive);

                    List<string> l_Text = new List<string>();
                    l_Text.Add("Хаха мои руки снова в строю! А что такое ходы? О_о");

                    TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
                    l_TextPanel.SetTalkingAnimator(m_HeadAnimator, "Talking");
                    l_TextPanel.SetText(l_Text);
                    l_TextPanel.AddButtonAction(l_TextPanel.Close);

                    ResultSystem.GetInstance().AddTextPanel(l_TextPanel);
                }

                ResultSystem.GetInstance().ShowResult();
                return;
            }

            AttackWithHands();
            DamageSystem.GetInstance().Attack(this, BattlePlayer.GetInstance(), 1.0f);
            ResultSystem.GetInstance().ShowResult();
        }
        
        public bool IsAllHandsDied()
        {
            if (m_LeftHand.isDead && m_RightHand.isDead)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Block()
        {
            DamageSystem.GetInstance().AttackFail();

            bodyAnimator.SetTrigger("BlockStart");
            
            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            l_TextPanel.SetText(new List<string>() { "Лол блок" });
            l_TextPanel.SetTalkingAnimator(headAnimator, "Talking");
            l_TextPanel.AddButtonAction(CloseDialogBlock);
            l_TextPanel.AddButtonAction(l_TextPanel.Close);
            ResultSystem.GetInstance().AddTextPanel(l_TextPanel);
        }

        public void OrganDie(OrganIds p_OrganIds)
        {
            switch (p_OrganIds)
            {
                case OrganIds.LeftHand:
                    m_BodyAnimator.SetTrigger("LeftHandDestroy");
                    m_HandsLive.x = 1.0f;
                    break;
                case OrganIds.RightHand:
                    m_BodyAnimator.SetTrigger("RightHandDestroy");
                    m_HandsLive.y = 1.0f;
                    break;
                case OrganIds.Body:
                    break;
            }

            CalculateIdle(m_HandsLive);
        }

        private void CalculateIdle(Vector2 p_HandsLive)
        {
            m_BodyAnimator.SetFloat("LeftHand", p_HandsLive.x);
            m_BodyAnimator.SetFloat("RightHand", p_HandsLive.y);
        }

        private void CloseDialogBlock()
        {
            bodyAnimator.SetTrigger("BlockStop");
        }

        private void AttackWithHands()
        {
            if (!m_LeftHand.isDead)
            {
                LeshiiAttackEffect l_LeftAttackEffect = Instantiate(Resources.Load<LeshiiAttackEffect>("Prefabs/Bosses/Leshii/LeshiiAttackEffect"));
                l_LeftAttackEffect.AddPlayAction(AttackLeftHand);
                AttackEffectsSystem.GetInstance().AddEffect(l_LeftAttackEffect);
            }

            if (!m_RightHand.isDead)
            {
                LeshiiAttackEffect l_RightAttackEffect = Instantiate(Resources.Load<LeshiiAttackEffect>("Prefabs/Bosses/Leshii/LeshiiAttackEffect"));
                l_RightAttackEffect.AddPlayAction(AttackRightHand);
                AttackEffectsSystem.GetInstance().AddEffect(l_RightAttackEffect);
            }
            
            AttackEffectsSystem.GetInstance().PlayEffects();
        }

        private void AttackRightHand()
        {
            if (m_LeftHand.isDead)
            {
                bodyAnimator.SetTrigger("AttackNoLeft");
            }
            else
            {
                bodyAnimator.SetTrigger("AttackRight");
            }            
        }

        private void AttackLeftHand()
        {
            if (m_RightHand.isDead)
            {
                bodyAnimator.SetTrigger("AttackNoRight");
            }
            else
            {
                bodyAnimator.SetTrigger("AttackLeft");
            }
        }
    }
}