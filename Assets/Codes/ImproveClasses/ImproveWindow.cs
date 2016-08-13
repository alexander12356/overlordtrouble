using UnityEngine;
using System.Collections;

public class ImproveWindow : MonoBehaviour
{
    private ButtonList m_ImproveButtonList = null;

    public void Awake()
    {
        m_ImproveButtonList = GetComponent<ButtonList>();
    }

    public void Update()
    {
        m_ImproveButtonList.UpdateKey();
    }
}
