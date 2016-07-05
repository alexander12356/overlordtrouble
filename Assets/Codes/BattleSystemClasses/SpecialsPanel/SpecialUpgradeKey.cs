using UnityEngine;
using UnityEngine.UI;

public class SpecialUpgradeKey : MonoBehaviour
{
    private KeyCode m_Key= KeyCode.UpArrow;

    private Image m_Image = null;

    private bool m_Upgraded = false;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
    }

    public void SetKey(KeyCode p_Key)
    {
        m_Key = p_Key;

        string l_Path = "Sprites/BattleSystem/UI/";
        switch (p_Key)
        {
            case KeyCode.UpArrow:
                l_Path += "UpArrow";
                break;
            case KeyCode.RightArrow:
                l_Path += "RightArrow";
                break;
            case KeyCode.LeftArrow:
                l_Path += "LeftArrow";
                break;
            case KeyCode.DownArrow:
                l_Path += "DownArrow";
                break;
        }
        m_Image.sprite = Resources.Load<Sprite>(l_Path);
    }

    public bool Upgraded
    {
        get
        {
            return m_Upgraded;
        }

        set
        {
            m_Upgraded = value;

            if (m_Upgraded)
            {
                m_Image.color = Color.blue;
            }
            else
            {
                m_Image.color = Color.white;
            }
        }
    }
}
