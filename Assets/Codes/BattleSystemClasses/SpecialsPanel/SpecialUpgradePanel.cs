using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class SpecialUpgradePanel : MonoBehaviour
{
    [SerializeField]
    private SpecialIcon m_SpecialIconPrefab = null;

    [SerializeField]
    private SpecialUpgradeKey m_SpecialUpgradeKeyPrefab = null;

    [SerializeField]
    private Transform m_SpecialIconsConteiner = null;

    private static SpecialUpgradePanel m_Instance;

    private List<KeyCode> m_SpecialKeys = new List<KeyCode>();

    private List<Special> m_SpecialList;

    private List<SpecialIcon> m_SpecialIcons = new List<SpecialIcon>();

    private List<SpecialUpgradeKey> m_SpecialUpgradeKeyList = new List<SpecialUpgradeKey>();

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
        CreateIcons(m_SpecialList);

        RunTimer();
    }

    public void RunTimer()
    {
        RandomizeSpecialKeys();



        Debug.Log("Upgrade Time START");
    }

    private void RandomizeSpecialKeys()
    {
        m_SpecialKeys.Clear();
        m_SpecialUpgradeKeyList.Clear();
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
            l_SpecialUpgradeKey.transform.localScale = Vector3.one;
            l_SpecialUpgradeKey.transform.localPosition = new Vector3(0.0f, 70.0f, 0.0f);

            m_SpecialUpgradeKeyList.Add(l_SpecialUpgradeKey);
        }
    }

    private int specialCount = 0;
    private float m_UpgradeTime = 0.0f;
    private bool m_Start = false;
    private void Update()
    {
        if (!m_Start)
        {
            return;
        }
        
        if (m_UpgradeTime < 3.0f)
        {
            m_UpgradeTime += Time.deltaTime;
        }
        else
        {
            m_Start = false;
            Debug.Log("Upgrade Time END");
            //PanelManager.GetInstance().Show(PanelEnum.Main);
            Player.GetInstance().SpecialAttack(m_SpecialList);
            return;
        }
        

        if (Input.GetKeyDown(m_SpecialKeys[specialCount]))
        {
            m_SpecialUpgradeKeyList[specialCount].Upgraded = true;
            m_SpecialList[specialCount].level++;

            specialCount++;

            if (specialCount >= m_SpecialKeys.Count)
            {
                specialCount = 0;

                for (int i = 0; i < m_SpecialUpgradeKeyList.Count; i++)
                {
                    m_SpecialUpgradeKeyList[i].Upgraded = false;
                    Destroy(m_SpecialUpgradeKeyList[i].gameObject);
                }
                RandomizeSpecialKeys();
            }
        }
        else
        {
            
        }
    }

    private void CreateIcons(List<Special> p_SpecialList)
    {
        for (int i = 0; i < p_SpecialList.Count; i++)
        {
            SpecialIcon l_NewSpecialIcon = Instantiate(m_SpecialIconPrefab);
            l_NewSpecialIcon.transform.SetParent(m_SpecialIconsConteiner);
            l_NewSpecialIcon.id = p_SpecialList[i].title;
            l_NewSpecialIcon.transform.localScale = Vector3.one;

            m_SpecialIcons.Add(l_NewSpecialIcon);
        }
    }

    private void ClearUpgradeKey()
    {

    }
}
