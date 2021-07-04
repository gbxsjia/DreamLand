using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIState/KeepDistance")]
public class AS_KeepDistance : AIState_Base
{
    public bool CanAttack;
    public float MaxBullet = 3;
    public float MinBullet = 0.5f;
    [Range(0, 1)]
    public float HealthThreshold = 0.4f;
    public override void StateBegin(AIBrain brain)
    {
        base.StateBegin(brain);
        brain.MoveComp.TargetObject = brain.Target;
        brain.MoveComp.MoveType = AIMoveType.KeepDistance;
        brain.characterManager.SetDefaultMoveMode(0);
    }
    public override void UpdateFunction()
    {
        base.UpdateFunction();
        if (owner.Target && owner.characterManager.CurrentWeapon && CanAttack)
        {
            Vector3 attackDirection = owner.Target.transform.position - owner.transform.position;
            owner.characterManager.Attack(attackDirection);
        }
    }
    public override bool StateCondition(AIBrain brain)
    {
        bool result = false;
        result = brain.IsInBattle
            && brain.characterManager.CurrentWeapon.BulletCurrent >= MinBullet
            && brain.characterManager.CurrentWeapon.BulletCurrent <= MaxBullet
            && brain.characterManager.CurrentWeapon
            && brain.stateManager.GetHealthPercent() >= HealthThreshold;
        return result;
    }
}
