using System.Collections.Generic;
using UnityEngine;

public class VisualEffectChecker : MonoBehaviour
{
    private Queue<VisualEffect> l_AttackEffectQueue = new Queue<VisualEffect>();

    public void AddAttackEffect(VisualEffect p_AttackEffect)
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
