using UnityEngine;

using System.Collections;

public delegate void PanelActionHandler();
public class Panel : MonoBehaviour
{
    #region Variables
    private event PanelActionHandler m_PopAction = null;
    private event PanelActionHandler m_PushAction = null;
    private Transform m_Transform;
    private float m_ShowingSpeed = 1400.0f;
    private bool m_Close = false;
    private bool m_Moving = false;
    private bool m_IsShowed = false;
    #endregion

    #region Interface
    public bool isShowed
    {
        get { return m_IsShowed; }
    }

    public bool moving
    {
        get
        {
            return m_Moving;
        }
    }

    public Transform myTransform
    {
        get
        {
            if (m_Transform == null)
            {
                m_Transform = transform;
            }
            return m_Transform;
        }

        set
        {
            m_Transform = value;
        }
    }

    public void AddPopAction(PanelActionHandler p_Action)
    {
        m_PopAction += p_Action;
    }

    public void RemovePopAction(PanelActionHandler p_Action)
    {
        m_PopAction -= p_Action;
    }

    public virtual void PopAction()
    {
        if (m_PopAction != null)
        {
            m_PopAction();
        }
    }

    public void AddPushAction(PanelActionHandler p_Action)
    {
        m_PushAction += p_Action;
    }

    public void RemovePushAction(PanelActionHandler p_Action)
    {
        m_PushAction -= p_Action;
    }

    public virtual void PushAction()
    {
        if (m_PushAction != null)
        {
            m_PushAction();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(Showing());
    }

    public void Hide()
    {
        StartCoroutine(Hiding());
    }

    public void Close()
    {
        Hide();
        m_Close = true;
    }

    public virtual void UpdatePanel()
    {

    }
    #endregion

    #region Private
    private void Awake()
    {
        m_Transform = transform;
    }

    protected IEnumerator Showing()
    {
        m_Moving = true;
        myTransform.localPosition = new Vector3(1500.0f, 0.0f, 0.0f);
        myTransform.localScale = Vector3.one;
        Vector3 l_Position = myTransform.localPosition;

        while (myTransform.localPosition.x >= 0)
        {
            l_Position.x -= m_ShowingSpeed * Time.deltaTime;
            myTransform.localPosition = l_Position;
            yield return new WaitForEndOfFrame();
        }
        myTransform.localPosition = Vector3.zero;
        m_Moving = false;

        EndShowing();
    }

    protected IEnumerator Hiding()
    {
        m_Moving = true;
        Vector3 l_Position = myTransform.localPosition;
        while (myTransform.localPosition.x >= -1180)
        {
            l_Position.x -= m_ShowingSpeed * Time.deltaTime;
            myTransform.localPosition = l_Position;
            yield return new WaitForEndOfFrame();
        }

        m_Moving = false;
        myTransform.localPosition = new Vector3(-1180.0f, 0.0f, 0.0f);

        EndHiding();
    }

    protected void EndShowing()
    {
        m_IsShowed = true;
        PushAction();
    }

    protected void EndHiding()
    {
        m_IsShowed = false;
        gameObject.SetActive(false);
        if (m_Close)
        {
            PopAction();
            Destroy(gameObject);
        }
    }
    #endregion
}