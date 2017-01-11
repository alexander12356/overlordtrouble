using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    #region Variables
    private JourneyActor m_JournetActor = null;
    #endregion

    #region Interface
    public JourneyActor journeyActor
    {
        get { return m_JournetActor; }
        set { m_JournetActor = value; }
    }

    public virtual void Awake()
    {
    }

    public virtual void Start()
    {
    }

    public virtual void LogicStart()
    {

    }

	public virtual void LogicUpdate ()
    {
	
	}

    public virtual void LogicStop()
    {

    }
    #endregion
}
