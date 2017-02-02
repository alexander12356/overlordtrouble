using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGenerateAction : MonoBehaviour
{
    [SerializeField]
    private RoomEnemyGenerator m_Generator = null;

    [SerializeField]
    private bool m_GenerateEnable = true;

	public void Run()
    {
        m_Generator.generateEnable = m_GenerateEnable;
    }
}
