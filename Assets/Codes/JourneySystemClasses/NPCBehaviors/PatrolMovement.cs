using UnityEngine;
using System.Collections.Generic;

public class PatrolMovement : BaseMovement
{
    [SerializeField]
    private List<Vector2> m_PatrolPoints = null;

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}