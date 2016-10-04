using UnityEngine;
using UnityEngine.UI;

public class LocalizationGUIText : MonoBehaviour
{
    [SerializeField]
    private string m_TextId = "";

    public void Awake()
    {
        Text l_Text = GetComponent<Text>();
        l_Text.text = LocalizationDataBase.GetInstance().GetText(m_TextId);
    }
}