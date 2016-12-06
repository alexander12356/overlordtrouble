using System.Collections.Generic;
using UnityEngine;

public class EndEffectChecker : MonoBehaviour
{
    private Queue<AttackEffect> l_AttackEffectQueue = new Queue<AttackEffect>();

    public void AddAttackEffect(AttackEffect p_AttackEffect)
    {
        l_AttackEffectQueue.Enqueue(p_AttackEffect);
    }

    // Called from Animation
    public void EndAnimation()
    {
        Destroy(l_AttackEffectQueue.Dequeue().gameObject);
        ResultSystem.GetInstance().NextStep();
    }
}
