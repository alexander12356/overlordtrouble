using UnityEngine;
using System;

public class JourneyNPC : JourneyActor
{
    #region Variables
    private CheckCollide     m_CheckCollide    = null;
    private BaseCollideBehaviors m_BaseCollideBehaviors = null;
    private BaseMovement m_BaseMovement = null;

    [SerializeField]
    private string m_NpcId = "TestNPC";
    #endregion

    #region Interface
    public override void Awake()
    {
        base.Awake();

        m_BaseCollideBehaviors = GetComponent<BaseCollideBehaviors>();
        m_CheckCollide = GetComponentInChildren<CheckCollide>();

        m_CheckCollide.AddCollideEnterAction(m_BaseCollideBehaviors.EnterAction);
        m_CheckCollide.AddCollideExitAction(m_BaseCollideBehaviors.ExitAction);

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

#if UNITY_EDITOR
    [ContextMenu("GenerateStayNPC)")]
    public void GenerateStayNPC()
    {
        gameObject.AddComponent<StayMovement>();
        gameObject.AddComponent<DialogCollideBehaviors>();
        GenerateBaseObjects();
    }

    [ContextMenu("GenerateMovingNPC)")]
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

        GameObject l_ActiveKey = new GameObject();
        l_ActiveKey.name = "ActiveKey";
        l_ActiveKey.AddComponent<SpriteRenderer>();
        l_ActiveKey.transform.SetParent(transform);
        l_ActiveKey.transform.localPosition = Vector3.zero;
    }
#endif
#endregion
}