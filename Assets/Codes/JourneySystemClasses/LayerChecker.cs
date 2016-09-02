using UnityEngine;

public class LayerChecker : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer = null;
    private CheckCollide m_UpperChecker = null;
    private CheckCollide m_DownerChecker = null;
    private int m_BaseLayerOrder = 0;

    public void Awake()
    {
        m_SpriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
        m_BaseLayerOrder = m_SpriteRenderer.sortingOrder;

        for (int i = 0; i < transform.childCount; i++)
        {
            switch(transform.GetChild(i).name)
            {
                case "Upper":
                    m_UpperChecker = transform.GetChild(i).GetComponent<CheckCollide>();
                    break;
                case "Downer":
                    m_DownerChecker = transform.GetChild(i).GetComponent<CheckCollide>();
                    break;
            }
        }

        m_DownerChecker.AddCollideEnterAction(LayerDowner);
        m_DownerChecker.AddCollideExitAction(ResetLayerOrder);

        m_UpperChecker.AddCollideEnterAction(LayerUpper);
        m_UpperChecker.AddCollideExitAction(ResetLayerOrder);
    }

    private void LayerUpper(JourneyPlayer m_JourneyPlayer)
    {
        m_SpriteRenderer.sortingOrder = m_BaseLayerOrder + 1;
    }

    private void LayerDowner(JourneyPlayer m_JourneyPlayer)
    {
        m_SpriteRenderer.sortingOrder = m_BaseLayerOrder - 1;
    }

    private void ResetLayerOrder(JourneyPlayer m_JourneyPlayer)
    {
        m_SpriteRenderer.sortingOrder = m_BaseLayerOrder;
    }
}
