using UnityEngine;
using System.Collections.Generic;

namespace BattleSystemClasses.Bosses.Leshii
{
    public enum LeshiiOrganIds
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

            m_RightHand = transform.FindChild(LeshiiOrganIds.RightHand.ToString()).GetComponent<LeshiiOrgan>();
            m_LeftHand  = transform.FindChild(LeshiiOrganIds.LeftHand.ToString()).GetComponent<LeshiiOrgan>();
            m_Body      = transform.FindChild(LeshiiOrganIds.Body.ToString()).GetComponent<LeshiiOrgan>();

            m_RightHand.Init(LeshiiOrganIds.RightHand, this);
            m_LeftHand.Init(LeshiiOrganIds.LeftHand, this);
            m_Body.Init(LeshiiOrganIds.Body, this);

            m_BodyAnimator = GetComponent<Animator>();
            m_HeadAnimator = transform.FindChild(LeshiiOrganIds.head.ToString()).GetComponent<Animator>();
        }
        
        public List<LeshiiOrgan> GetOrgans()
        {
            List<LeshiiOrgan> l_LeshiiOrgans = new List<LeshiiOrgan>() { m_RightHand, m_Body, m_LeftHand };
            return l_LeshiiOrgans;
        }

        public void Run()
        {

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
            l_TextPanel.SetTalkingAnimator(bodyAnimator, "BlockTalking");
            l_TextPanel.AddButtonAction(CloseDialogBlock);

            DamageSystem.GetInstance().AttackFail();
            DamageSystem.GetInstance().AddTextPanel(l_TextPanel);
        }

        public void CloseDialogBlock()
        {
            bodyAnimator.SetTrigger("BlockStop");
        }
    }
}