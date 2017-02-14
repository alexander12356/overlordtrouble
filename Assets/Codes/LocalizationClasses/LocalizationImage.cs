using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationImage : MonoBehaviour
{
    [System.Serializable]
    private struct ImageStruct
    {
        public string languageId;
        public Sprite sprite;
    }

    private SpriteRenderer m_SpriteRenderer = null;
    private Dictionary<string, Sprite> m_LanguageImageList = new Dictionary<string, Sprite>();

    [SerializeField]
    private List<ImageStruct> m_ImageList = null;

    public void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < m_ImageList.Count; i++)
        {
            m_LanguageImageList.Add(m_ImageList[i].languageId, m_ImageList[i].sprite);
        }
    }

    public void Start()
    {
        m_SpriteRenderer.sprite = m_LanguageImageList[LocalizationDataBase.GetInstance().currentLanguage];
    }
}
