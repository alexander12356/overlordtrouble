using UnityEngine;

public class JourneyChest : JourneyActor
{
    public override void ChangeInteractionBehavior(string p_BehaviorId)
    {
        base.ChangeInteractionBehavior(p_BehaviorId);

        if (p_BehaviorId == "Empty")
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Chest/chestOpen");
        }
    }
}
