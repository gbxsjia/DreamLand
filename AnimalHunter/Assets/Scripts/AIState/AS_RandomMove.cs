using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomMove",menuName = "AIState/RandomMove")]
public class AS_RandomMove : AIState_Base
{
    public float MoveDuration;
    public float RestDuration;

    private float timer;
    private bool isMoving;
    public override void StateBegin(AIBrain brain)
    {
        base.StateBegin(brain);
        brain.MoveComp.TargetObject = null;
        brain.MoveComp.MoveType = AIMoveType.RandomMove;
        isMoving = true;
        brain.characterManager.SetDefaultMoveMode(0);
    }
    public override void UpdateFunction()
    {
        base.UpdateFunction();
        timer += Time.deltaTime;
        if (isMoving)
        {
            if (timer >= MoveDuration)
            {
                owner.MoveComp.MoveType = AIMoveType.None;
                timer = 0;
                isMoving = false;
            }
        }
        else
        {
            if (timer >= RestDuration)
            {
                owner.MoveComp.MoveType = AIMoveType.RandomMove;
                timer = 0;
                isMoving = true;
            }
        }
    }
    public override bool StateCondition(AIBrain brain)
    {
        return !brain.IsInBattle;
    }
}
