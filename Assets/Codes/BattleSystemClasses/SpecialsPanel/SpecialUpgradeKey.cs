using UnityEngine;
using UnityEngine.UI;

public class SpecialUpgradeKey : MonoBehaviour
{
    private KeyCode m_Key;

    private Image m_Image;

    public void SetKey(KeyCode p_Key)
    {
        string l_Path = "Sprites/BattleSystem/UI";
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

    public void Upgrade()
    {
        m_Image.color = Color.blue;
    }
}
