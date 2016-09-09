using UnityEngine;

public class LayerChecker : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer = null;
    private CheckCollide m_UpperChecker = null;
    private int m_BaseLayerOrder = 0;

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
    }

    private void ResetLayerOrder(JourneyPlayer m_JourneyPlayer)
    {
        m_SpriteRenderer.sortingOrder = m_BaseLayerOrder;
    }
}
