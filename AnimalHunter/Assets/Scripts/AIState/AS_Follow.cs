using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIState/Follow")]
public class AS_Follow : AIState_Base
{
    public bool CanAttack;
    public bool NeedBattleState;
    public float BulletThreshold = 0.5f;
    [Range(0, 1)]
    public float HealthThreshold = 0.4f;
    public float StopDistance;

    private bool isMoving = false;
    public GameObject FollowTarget;
    public override void StateBegin(AIBrain brain)
    {
        base.StateBegin(brain);

        FollowTarget = BattleManager.instance.PlayerOject;

        brain.MoveComp.TargetObject = FollowTarget;
        brain.MoveComp.MoveType = AIMoveType.Chase;
        isMoving = true;
        brain.characterManager.SetDefaultMoveMode(1);
    }
    public override void UpdateFunction()
    {
        base.UpdateFunction();
        if (owner.Target && owner.characterManager.CurrentWeapon && CanAttack)
        {
            Vector3 attackDirection = owner.Target.transform.position - owner.transform.position;
            owner.characterManager.Attack(attackDirection);
        }

        if (FollowTarget)
        {
            if (Vector3.Distance(owner.transform.position, FollowTarget.transform.position) <= StopDistance)
            {
                if (isMoving)
                {
                    owner.MoveComp.MoveType = AIMoveType.None;
                    isMoving = false;
                }
            }
            else
            {
                if (!isMoving)
                {
                    owner.MoveComp.MoveType = AIMoveType.Chase;
                    isMoving = true;
                }
            }
        }
        else if (isMoving)
        {
            owner.MoveComp.MoveType = AIMoveType.None;
            isMoving = false;

        }
    }
    public override bool StateCondition(AIBrain brain)
    {
        bool result = false;
        result = (brain.IsInBattle == NeedBattleState)
            && brain.characterManager.CurrentWeapon.BulletCurrent >= BulletThreshold
            && brain.characterManager.CurrentWeapon
            && brain.stateManager.GetHealthPercent() >= HealthThreshold;
        return result;
    }
}
