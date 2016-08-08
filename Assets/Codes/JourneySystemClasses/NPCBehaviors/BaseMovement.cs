using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    #region Variables
    private JourneyActor m_JourneyActor = null;
    #endregion

    #region Interface
    public JourneyActor journeyActor
    {
        get
        {
            if (m_JourneyActor)
            {
                m_JourneyActor = GetComponent<JourneyActor>();
            }
            return m_JourneyActor;
        }
    }

    public virtual void Awake()
    {
        m_JourneyActor = GetComponent<JourneyActor>();
    }

	public virtual void LogicUpdate ()
    {
	
	}
    #endregion
}
