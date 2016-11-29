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
        head
    }

    public class Leshii : MonoBehaviour
    {
        private LeshiiOrgan m_RightHand = null;
        private LeshiiOrgan m_LeftHand  = null;
        private LeshiiOrgan m_Body      = null;
        private Animator m_BodyAnimator = null;
        private Animator m_HeadAnimator = null;
        private Vector2 m_HandsLive = Vector2.zero;

        public Animator bodyAnimator
        {
            get { return m_BodyAnimator; }
        }
        public Animator headAnimator
        {
            get { return m_HeadAnimator; }
        }

        public void Init()
        {
            int l_ChildCount = transform.childCount;

            m_RightHand = transform.FindChild(OrganIds.RightHand.ToString()).GetComponent<LeshiiOrgan>();
            m_LeftHand  = transform.FindChild(OrganIds.LeftHand.ToString()).GetComponent<LeshiiOrgan>();
            m_Body      = transform.FindChild(OrganIds.Body.ToString()).GetComponent<LeshiiOrgan>();

            m_RightHand.Init(OrganIds.RightHand, this);
            m_LeftHand.Init(OrganIds.LeftHand, this);
            m_Body.Init(OrganIds.Body, this);

            m_BodyAnimator = GetComponent<Animator>();
            m_HeadAnimator = transform.FindChild(OrganIds.head.ToString()).GetComponent<Animator>();

            CalculateIdle(m_HandsLive);
        }
        
        public List<LeshiiOrgan> GetOrgans()
        {
            List<LeshiiOrgan> l_LeshiiOrgans = new List<LeshiiOrgan>() { m_RightHand, m_Body, m_LeftHand };
            return l_LeshiiOrgans;
        }

        public void Run()
        {
            if (IsAllHandsDied())
            {
                return;
            }

            if (m_LeftHand.isDead)
            {
                m_BodyAnimator.SetTrigger("AttackNoLeft");
            }
            else if (m_RightHand.isDead)
            {
                m_BodyAnimator.SetTrigger("AttackNoRight");
            }
            else
            {
                m_BodyAnimator.SetTrigger("Attack");
            }
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
            bodyAnimator.SetTrigger("BlockStart");

            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);

            l_TextPanel.SetText(new List<string>() { "Лол блок" });
            l_TextPanel.SetTalkingAnimator(headAnimator, "Talking");
            l_TextPanel.AddButtonAction(CloseDialogBlock);

            DamageSystem.GetInstance().AttackFail();
            DamageSystem.GetInstance().AddTextPanel(l_TextPanel);
        }

        public void CloseDialogBlock()
        {
            bodyAnimator.SetTrigger("BlockStop");
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
    }
}