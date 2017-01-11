using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPosition : MonoBehaviour
{
    [SerializeField]
    private float m_Speed;

    [SerializeField]
    private bool m_IsDelay;

    public float speed
    {
        get { return m_Speed; }
    }
    public bool isDelay
    {
        get { return m_IsDelay; }
    }
    public Vector3 myPosition
    {
        get { return transform.position; }
    }
}
