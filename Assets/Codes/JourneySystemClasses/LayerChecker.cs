using UnityEngine;

public class LayerChecker : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer = null;
    private CheckCollide m_UpperChecker = null;
    private int m_BaseLayerOrder = 0;

    [SerializeField]
    private SpriteRenderer[] m_AdditionalRenderers = null;

    public void Awake()
    {
        m_SpriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
        m_BaseLayerOrder = m_SpriteRenderer.sortingOrder;

        m_UpperChecker = transform.FindChild("Upper").GetComponent<CheckCollide>();

        m_UpperChecker.AddCollideEnterAction(LayerUpper);
        m_UpperChecker.AddCollideExitAction(ResetLayerOrder);
    }

    private void LayerUpper(JourneyPlayer m_JourneyPlayer)
    {
        m_SpriteRenderer.sortingOrder = m_JourneyPlayer.sortingOrder + 1;

        for (int i = 0; i < m_AdditionalRenderers.Length; i++)
        {
            m_AdditionalRenderers[i].sortingOrder = m_JourneyPlayer.sortingOrder + 1;
        }
    }

    private void ResetLayerOrder(JourneyPlayer m_JourneyPlayer)
    {
        m_SpriteRenderer.sortingOrder = m_BaseLayerOrder;

        for (int i = 0; i < m_AdditionalRenderers.Length; i++)
        {
            m_AdditionalRenderers[i].sortingOrder = m_BaseLayerOrder;
        }
    }
}
