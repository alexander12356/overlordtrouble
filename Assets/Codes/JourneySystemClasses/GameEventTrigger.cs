using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
    [SerializeField]
    public ActionStruct m_Action;

    public void OnTriggerEnter2D(Collider2D p_Collision)
    {
        if (p_Collision.tag == "Player")
        {
            m_Action.actionEvent.Invoke();
            Destroy(gameObject);
        }
    }
}
