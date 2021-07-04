using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AIState/None")]
public class AIState_Base : ScriptableObject
{
    public AIBrain owner;
    public int Weight;
    public virtual void StateBegin(AIBrain brain)
    {
        owner = brain;
    }
    public virtual void StateEnd()
    {

    }
    public virtual bool StateCondition(AIBrain brain)
    {
        return true;
    }
    public virtual void UpdateFunction()
    {

    }
}
