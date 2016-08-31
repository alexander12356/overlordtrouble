using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;

public class ButtonListScrolling : MonoBehaviour
{
    private ButtonList m_ButtonList = null;
    private ScrollRect m_ScrollRect = null;
    private Coroutine m_ScrollingCoroutine = null;
    private int m_RowCountInPage = 1;
    private float m_RowVerticalSize = 30.0f;

    [SerializeField]
    private float m_ScrollSpeed = 3.0f;

    public ButtonList buttonList
    {
        get
        {
            if (m_ButtonList == null)
            {
                m_ButtonList = GetComponentInChildren<ButtonList>();
            }
            return m_ButtonList;
        }
    }
    public ScrollRect scrollRect
    {
        get
        {
            if (m_ScrollRect == null)
            {
                m_ScrollRect = GetComponent<ScrollRect>();
            }
            return m_ScrollRect;
        }
    }

    public void Awake()
    {
        
        
    }

    public void Init(float p_RowVerticalSize, int p_RowCountInPage)
    {
        m_RowVerticalSize = p_RowVerticalSize;
        m_RowCountInPage = p_RowCountInPage;

        RescaleBounds();
    }

    public void CheckScrolling()
    {
        StartScrolling(CalculateScrollVerticalNormalizedPostition());
    }

    public void StartScrolling(float p_DestNormilizedPosition)
    {
        if (m_ScrollingCoroutine != null)
        {
            StopCoroutine(m_ScrollingCoroutine);
        }
        m_ScrollingCoroutine = StartCoroutine(Scrolling(p_DestNormilizedPosition));
    }

#if UNITY_EDITOR
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log("Calculated Vertical Value: " + CalculateScrollVerticalNormalizedPostition() + ", ScrollRect Verical Value: " + scrollRect.verticalNormalizedPosition);
        }
    }
#endif

    private void RescaleBounds()
    {
        if (m_RowVerticalSize * buttonList.count > buttonList.rectTransform.sizeDelta.y)
        {
            int l_SpecialCount = GetBoundSpecialCount();

            Vector2 l_SizeDelta = buttonList.rectTransform.sizeDelta;
            l_SizeDelta.y = m_RowVerticalSize * l_SpecialCount;
            buttonList.rectTransform.sizeDelta = l_SizeDelta;
        }
    }

    private float CalculateScrollVerticalNormalizedPostition()
    {
        int l_CurrentPage = buttonList.currentButtonId / m_RowCountInPage;
        int l_PageCount = GetBoundSpecialCount() / m_RowCountInPage;

        if (l_PageCount == 1)
        {
            return 1.0f;
        }

        float l_NormalizedPosition = 1.0f - ((float)l_CurrentPage / ((float)l_PageCount - 1));

        if (l_NormalizedPosition < 0)
        {
            l_NormalizedPosition = 0.0f;
        }
        return l_NormalizedPosition;
    }

    private int GetBoundSpecialCount()
    {
        if (buttonList.count % m_RowCountInPage != 0)
        {
            return buttonList.count + (m_RowCountInPage - buttonList.count % m_RowCountInPage);
        }
        return buttonList.count;
    }

    private IEnumerator Scrolling(float p_DestNormilizedPosition)
    {
        while (Math.Abs(scrollRect.verticalNormalizedPosition - p_DestNormilizedPosition) > 0.005f)
        {
            scrollRect.verticalNormalizedPosition = Mathf.MoveTowards(scrollRect.verticalNormalizedPosition, p_DestNormilizedPosition, m_ScrollSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
