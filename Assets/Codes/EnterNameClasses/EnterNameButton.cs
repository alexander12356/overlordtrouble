using UnityEngine.UI;

public class EnterNameButton : PanelButton
{
    public override void Awake()
    {
        m_SelectedImage = transform.FindChild("SelectImage").GetComponent<Image>();
    }
}
