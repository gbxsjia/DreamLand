using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action/Jump")]
public class Action_Jump : Action_Base
{
    public float Height;
    public float MaxDistance;

    private Vector3 EndPoint;
    private Vector3 StartPoint;
    private float WholeDuration;
    private float A;
    private float B;

    public override void IntializeAction(CharacterManager character, Vector3 direction)
    {
        base.IntializeAction(character, direction);
        StartPoint = character.transform.position;
        EndPoint = StartPoint + direction;
        if (direction.sqrMagnitude > MaxDistance * MaxDistance)
        {
            EndPoint = StartPoint + direction.normalized * MaxDistance;
        }
        WholeDuration = LifeTimer;
        A = -4 * Height / WholeDuration / WholeDuration;
        B = 4 * Height / WholeDuration;
    }
    public override void UpdateAction(float deltaTime)
    {
        base.UpdateAction(deltaTime);
        Vector3 position = Vector3.Lerp(StartPoint, EndPoint, 1 - LifeTimer / WholeDuration);
        position.y = A * LifeTimer * LifeTimer + B * LifeTimer;
        Owner.transform.position = position;       
    }
    public override void ActionEnd()
    {
        base.ActionEnd();
        Owner.transform.position = EndPoint;
    }
}
