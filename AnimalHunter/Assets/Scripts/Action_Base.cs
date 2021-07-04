using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Action/Normal")]
public class Action_Base : ScriptableObject
{
    public AnimationClip AnimClip;
    public bool UseFixedDuration;
    public float Duration;    

    public Vector3 Direction;
    public CharacterManager Owner;
    protected float LifeTimer;
    public virtual void IntializeAction(CharacterManager character, Vector3 direction)
    {
        Owner = character;
        Direction = direction;
        if (AnimClip != null && !UseFixedDuration)
        {
            LifeTimer = AnimClip.length;
        }
        else
        {
            LifeTimer = Duration;
        }
        character.SetMoveMode(0);
    }

    public virtual void UpdateAction(float deltaTime)
    {
        LifeTimer -= deltaTime;
        if (LifeTimer <= 0)
        {
            ActionEnd();
        }
    }

    public virtual void ActionEnd()
    {
        Owner.EndAction(this);
        Owner.ResetMoveMode();
    }
}
