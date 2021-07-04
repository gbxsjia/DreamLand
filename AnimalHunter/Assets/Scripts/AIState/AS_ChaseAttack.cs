using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIState/ChaseAttack")]
public class AS_ChaseAttack : AIState_Base
{
    public float BulletThreshold = 0.5f;
    [Range(0,1)]
    public float HealthThreshold=0.4f;
    public override void StateBegin(AIBrain brain)
    {
        base.StateBegin(brain);
        brain.MoveComp.TargetObject = brain.Target;
        brain.MoveComp.MoveType = AIMoveType.Chase;
        brain.characterManager.SetDefaultMoveMode(1);
    }
    public override void UpdateFunction()
    {
        base.UpdateFunction();
        if (owner.Target)
        {
            float distance = Vector3.Distance(owner.Target.transform.position, owner.transform.position);
            if (owner.characterManager.CurrentWeapon && distance < owner.characterManager.CurrentWeapon.AIAttackRange)
            {
                Vector3 attackDirection = owner.Target.transform.position - owner.transform.position;
                owner.characterManager.Attack(attackDirection);
            }
        }
    }
    public override bool StateCondition(AIBrain brain)
    {
        bool result = false;
        result = brain.IsInBattle
            && brain.characterManager.CurrentWeapon.BulletCurrent >= BulletThreshold
            && brain.characterManager.CurrentWeapon
            && brain.stateManager.GetHealthPercent() >= HealthThreshold;
        return result;
    }
}
