using UnityEngine;

using System.Collections.Generic;

public class LayerChecker : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer = null;
    private CheckCollide m_UpperChecker = null;
    private int m_BaseLayerOrder = 0;
    private List<int> m_AdditionalRenderersBaseLayerOrder = new List<int>();

    [SerializeField]
    private SpriteRenderer[] m_AdditionalRenderers = null;

    public void Awake()
    {
        m_SpriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
        m_BaseLayerOrder = m_SpriteRenderer.sortingOrder;

        m_UpperChecker = transform.FindChild("Upper").GetComponent<CheckCollide>();

        m_UpperChecker.AddCollideEnterAction(LayerUpper);
        m_UpperChecker.AddCollideExitAction(ResetLayerOrder);

        for (int i = 0; i < m_AdditionalRenderers.Length; i++)
        {
            m_AdditionalRenderersBaseLayerOrder.Add(m_AdditionalRenderers[i].sortingOrder);
        }
    }

    private void LayerUpper(JourneyActor p_JourneyActor)
    {
        m_SpriteRenderer.sortingOrder = p_JourneyActor.sortingOrder + 1;

        for (int i = 0; i < m_AdditionalRenderers.Length; i++)
        {
            m_AdditionalRenderers[i].sortingOrder = m_SpriteRenderer.sortingOrder + (i + 1);
        }
    }

    private void ResetLayerOrder(JourneyActor p_JourneyActor)
    {
        m_SpriteRenderer.sortingOrder = m_BaseLayerOrder;

        for (int i = 0; i < m_AdditionalRenderers.Length; i++)
        {
            m_AdditionalRenderers[i].sortingOrder = m_AdditionalRenderersBaseLayerOrder[i];
        }
    }
}
