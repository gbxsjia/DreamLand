using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementSetting
{
    public int MoveTypeIndex;
    public float MoveSpeed;
    public float Acceleration;
    public float TurnSpeed;
}

public enum MoveMode
{
    BattleWalk,
    BattleRun,
    BattleSprint,
    NormalWalk,
    NormalRun,
    NormalSprint
}