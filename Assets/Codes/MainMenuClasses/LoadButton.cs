using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : PanelButton {

    private static LoadButton m_Prefab = null;

    public static LoadButton prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<LoadButton>("Prefabs/Button/LoadButton");
            }
            return m_Prefab;
        }
    }

    public new Image selectedImage
    {
        get
        {
            if(m_SelectedImage == null)
            {
                m_SelectedImage = transform.FindChild("BackgroundSelected").GetComponent<Image>();
            }
            return m_SelectedImage;
        }
    }
}
