using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class SpecialUpgradePanel : MonoBehaviour
{
    [SerializeField]
    private SpecialIcon m_SpecialIconPrefab;

    [SerializeField]
    private SpecialUpgradeKey m_SpecialUpgradeKeyPrefab;

    [SerializeField]
    private Transform m_SpecialIconsConteiner;

    private static SpecialUpgradePanel m_Instance;

    private List<KeyCode> m_SpecialKeys = new List<KeyCode>();

    private List<Special> m_SpecialList;

    private List<SpecialIcon> m_SpecialIcons;

    public static SpecialUpgradePanel GetInstance()
    {
        return m_Instance;
    }

    private void Awake()
    {
        m_Instance = this;
    }

    public void SetSpecials(List<Special> p_SpecialList)
    {
        m_SpecialList = p_SpecialList;

        RunTimer();
    }

    private void RandomizeSpecialKeys()
    {
        m_SpecialKeys.Clear();
        int l_KeyCode = 0;
        for (int i = 0; i < m_SpecialList.Count; i++)
        {
            l_KeyCode = Random.Range(0, 4);
            KeyCode l_Key = KeyCode.UpArrow;

            switch (l_KeyCode)
            {
                case 0:
                    l_Key = KeyCode.UpArrow;
                    break;
                case 1:
                    l_Key = KeyCode.DownArrow;
                    break;
                case 2:
                    l_Key = KeyCode.LeftArrow;
                    break;
                case 3:
                    l_Key = KeyCode.RightArrow;
                    break;
            }
            m_SpecialKeys.Add(l_Key);

            SpecialUpgradeKey l_SpecialUpgradeKey = Instantiate(m_SpecialUpgradeKeyPrefab);
            l_SpecialUpgradeKey.SetKey(l_Key);
            l_SpecialUpgradeKey.transform.SetParent(m_SpecialIcons[i].transform);
            l_SpecialUpgradeKey.transform.localPosition = Vector3.zero;
        }
    }

    public void RunTimer()
    {
        RandomizeSpecialKeys();
        StartCoroutine(UpgradeTimer());
    }

    float m_UpgradeTime = 0.0f;
    private IEnumerator UpgradeTimer()
    {
        m_UpgradeTime = 0.0f;
        while (m_UpgradeTime < 3.0f)
        {
            m_UpgradeTime += Time.deltaTime;
            UpgradeKeyCatch();
            yield return new WaitForEndOfFrame();
        }
    }

    private int levelCount = 1;
    private int specialCount = 0;
    private void UpgradeKeyCatch()
    {
        if (Input.GetKeyDown(m_SpecialKeys[specialCount]))
        {
            specialCount++;

            if (specialCount >= m_SpecialKeys.Count)
            {
                specialCount = 0;
                levelCount++;
            }
        }
        else
        {
            //m_SpecialList[ m_SpecialKeys[specialCount];
        }
    }

    private void CreateIcons(List<Special> p_SpecialList)
    {
        for (int i = 0; i < p_SpecialList.Count; i++)
        {
            SpecialIcon l_NewSpecialIcon = Instantiate(m_SpecialIconPrefab);
            l_NewSpecialIcon.transform.SetParent(m_SpecialIconsConteiner);
            l_NewSpecialIcon.SetSpecial(p_SpecialList[i].title);
            l_NewSpecialIcon.transform.localScale = Vector3.one;

            m_SpecialIcons.Add(l_NewSpecialIcon);
        }
    }
}
