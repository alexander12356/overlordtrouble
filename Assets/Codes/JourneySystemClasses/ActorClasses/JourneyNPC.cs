using UnityEngine;
using System;

public class JourneyNPC : JourneyActor
{
    #region Variables
    private BaseMovement m_BaseMovement = null;

    [SerializeField]
    private string m_NpcId = "TestNPC";
    #endregion

    #region Interface
    public string npcId
    {
        get { return m_NpcId; }
    }
    public BaseMovement baseMovement
    {
        get { return m_BaseMovement; }
        set { m_BaseMovement = value; }
    }


    public override void Awake()
    {
        base.Awake();

        m_BaseMovement = GetComponent<BaseMovement>();
        if (!m_BaseMovement)
        {
            Debug.LogWarning("Movement logic is null");
        }
    }

    public override void Update()
    {
        base.Update();

        if (m_BaseMovement)
        {
            m_BaseMovement.LogicUpdate();
        }
    }

    public override void StartLogic()
    {
        base.StartLogic();

        m_BaseMovement.LogicStart();
    }

    public override void StopLogic()
    {
        base.StopLogic();

        m_BaseMovement.LogicStop();
    }

    public override void GoTo(Vector3 p_Target, float p_Delay)
    {
        base.GoTo(p_Target, p_Delay);

        UpdateSortingLayer();
        myTransform.localPosition = Vector3.MoveTowards(myTransform.localPosition, p_Target, p_Delay);

        float l_DeltaX = myTransform.localPosition.x - p_Target.x;
        float l_DeltaY = myTransform.localPosition.y - p_Target.y;
        if (Mathf.Abs(l_DeltaX) > Mathf.Abs(l_DeltaY))
        {
            if (l_DeltaX > 0)
            {
                myAnimator.SetFloat("Input_X", -1);
                myAnimator.SetFloat("Input_Y", 0);
            }
            else
            {
                myAnimator.SetFloat("Input_X", 1);
                myAnimator.SetFloat("Input_Y", 0);
            }
        }
        else
        {
            if (l_DeltaY > 0)
            {
                myAnimator.SetFloat("Input_X", 0);
                myAnimator.SetFloat("Input_Y", -1);
            }
            else
            {
                myAnimator.SetFloat("Input_X", 0);
                myAnimator.SetFloat("Input_Y", 1);
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("GenerateStayNPC")]
    public void GenerateStayNPC()
    {
        gameObject.AddComponent<StayMovement>();
        gameObject.AddComponent<DialogCollideBehaviors>();
        GenerateBaseObjects();
    }

    [ContextMenu("GenerateMovingNPC")]
    public void GenerateMovingNPC()
    {
        gameObject.AddComponent<PatrolMovement>();
        gameObject.AddComponent<DialogCollideBehaviors>();
        GenerateBaseObjects();
    }

    private void GenerateBaseObjects()
    {
        GameObject l_RendererObject = new GameObject();
        l_RendererObject.name = "Renderer";
        l_RendererObject.AddComponent<SpriteRenderer>();
        l_RendererObject.AddComponent<Animator>();
        l_RendererObject.transform.SetParent(transform);
        l_RendererObject.transform.localPosition = Vector3.zero;

        GameObject l_ColliderObject = new GameObject();
        l_ColliderObject.name = "Collider";
        l_ColliderObject.AddComponent<BoxCollider2D>();
        l_ColliderObject.transform.SetParent(transform);
        l_ColliderObject.transform.localPosition = Vector3.zero;

        GameObject l_CheckCollider = new GameObject();
        l_CheckCollider.name = "CheckCollider";
        l_CheckCollider.AddComponent<CheckCollide>();
        l_CheckCollider.AddComponent<CircleCollider2D>();
        l_CheckCollider.GetComponent<CircleCollider2D>().isTrigger = true;
        l_CheckCollider.transform.SetParent(transform);
        l_CheckCollider.transform.localPosition = Vector3.zero;
    }
#endif
#endregion
}