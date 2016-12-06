using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEffectChecker : MonoBehaviour
{
    // Called from Animation
    public void EndAnimation()
    {
        ResultSystem.GetInstance().NextStep();
    }
}
