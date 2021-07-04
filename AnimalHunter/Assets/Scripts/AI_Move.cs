using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Move : MonoBehaviour
{
    public CharacterManager characterManager;
    public GameObject TargetObject;

    public Vector3 HomePosition;

    public int RayAmount;
    public float RayLength;
    public float CheckInterval = 0.2f;

    public MoveRayResult[] RayResults;
    private Vector3 MoveDirection = Vector3.zero;
    protected Vector3 LastMoveDirection;

    public AIMoveType MoveType;
    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
    }
    private void Start()
    {
        HomePosition = transform.position;
        StartCoroutine(MoveDecision());
    }

    private IEnumerator MoveDecision()
    {
        while (true)
        {
            RayTrace();
            MoveDirection = Vector3.zero;
            switch (MoveType)
            {
                case AIMoveType.None:                   
                    break;
                case AIMoveType.Chase:
                    if (TargetObject != null)
                    {
                        MD_ChaseMove();
                    }    
                    break;
                case AIMoveType.RunAway:
                    if (TargetObject != null)
                    {
                        MD_RunAway();
                    }
                    break;
                case AIMoveType.KeepDistance:
                    if (TargetObject != null)
                    {
                        MD_KeepDistance(3.5f, 0.5f);
                    }
                    break;
                case AIMoveType.RandomMove:
                    MD_RandomMove();
                    break;
            }
            MoveFunction();
            yield return new WaitForSeconds(CheckInterval);
        }
    }
    private void Update()
    {
        characterManager.Move(MoveDirection);
    }

    private void RayTrace()
    {
        RayResults = new MoveRayResult[RayAmount];
        Vector3 Direction = Vector3.forward;
        for (int i = 0; i < RayAmount; i++)
        {
            MoveRayResult result = new MoveRayResult();
            RayResults[i] = result;

            result.Direction = Direction;
            RaycastHit hit;
            Physics.SphereCast(transform.position + Vector3.up, 0.5f, Direction, out hit, RayLength);
            result.HitResult = hit;

            Direction = Quaternion.Euler(0, 360 / RayAmount, 0) * Direction;            
        }
    }
    public void MD_ChaseMove()
    {
        if (TargetObject != null)
        {
            for (int i = 0; i < RayAmount; i++)
            {
                if (RayResults[i].HitResult.collider != null)
                {
                    RayResults[i].Weight = 0;
                    Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + RayResults[i].Direction * 2f, Color.red);
                }
                else
                {
                    Vector3 TargetDirection = TargetObject.transform.position - transform.position;
                    float dot = Vector3.Dot(TargetDirection, RayResults[i].Direction);

                    dot = (dot + 1) / 2;

                    float PersistDot = Vector3.Dot(LastMoveDirection, RayResults[i].Direction);
                    PersistDot = (PersistDot + 1) / 4;
                    RayResults[i].Weight = dot + PersistDot;
                    Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + RayResults[i].Direction * RayResults[i].Weight, Color.green);
                }
            }
        }
        else
        {
            MoveDirection = Vector3.zero;
        }
    }
    public void MD_RunAway()
    {
        for (int i = 0; i < RayAmount; i++)
        {
            if (RayResults[i].HitResult.collider != null)
            {
                RayResults[i].Weight = 0;
                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + RayResults[i].Direction * 2f, Color.red);
            }
            else
            {
                Vector3 TargetDirection = transform.position - TargetObject.transform.position;
                float dot = Vector3.Dot(TargetDirection, RayResults[i].Direction);

                dot = (dot + 1) / 2;

                float PersistDot = Vector3.Dot(LastMoveDirection, RayResults[i].Direction);
                PersistDot = (PersistDot + 1) / 4;
                RayResults[i].Weight = dot + PersistDot;
                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + RayResults[i].Direction * RayResults[i].Weight, Color.green);
            }
        }
    }
    public void MD_KeepDistance(float targetDistance, float circleRange)
    {
        for (int i = 0; i < RayAmount; i++)
        {
            if (RayResults[i].HitResult.collider != null)
            {
                RayResults[i].Weight = 0;
                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + RayResults[i].Direction * 2f, Color.red);
            }
            else
            {
                Vector3 TargetDirection = (TargetObject.transform.position - transform.position).normalized;
                float currentDistance = Vector3.Distance(TargetObject.transform.position, transform.position);
                float DotWeight = Vector3.Dot(TargetDirection, RayResults[i].Direction);
                if (currentDistance > targetDistance + circleRange)
                {
                    DotWeight = Mathf.Clamp01(1 - Mathf.Abs(DotWeight - 0.732f));
                }
                else if (currentDistance < targetDistance - circleRange)
                {
                    DotWeight = Mathf.Clamp01(1 - Mathf.Abs(DotWeight + 0.732f));
                }
                else
                {
                    DotWeight = 1 - Mathf.Abs(DotWeight);
                }

                float PersistDot = Vector3.Dot(LastMoveDirection, RayResults[i].Direction);
                PersistDot = (PersistDot + 1) / 4;
                RayResults[i].Weight = DotWeight + PersistDot;
                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + RayResults[i].Direction * RayResults[i].Weight, Color.green);
            }
        }
    }
    public void MD_RandomMove()
    {
        for (int i = 0; i < RayAmount; i++)
        {
            if (RayResults[i].HitResult.collider != null)
            {
                RayResults[i].Weight = 0;
                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + RayResults[i].Direction * 2f, Color.red);
            }
            else
            {
                Vector3 HomeDirection = (HomePosition - transform.position).normalized;
                float HomeDistance = Vector3.Distance(HomePosition, transform.position);
                float HomeDot = Vector3.Dot(HomeDirection, RayResults[i].Direction);
                float HomeWeight = HomeDistance * (HomeDot + 1) / 16;

                Vector3 TargetDirection = Quaternion.Euler(0, Random.Range(-15, 15), 0) * LastMoveDirection;
                float DirectionDot = Vector3.Dot(TargetDirection, RayResults[i].Direction);
                float DirectionWeight = (DirectionDot + 1) / 2;

                RayResults[i].Weight = DirectionWeight + HomeWeight;
                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + RayResults[i].Direction * RayResults[i].Weight, Color.green);
            }
        }
    }

    public void MoveFunction()
    {
        float maxWeight = -1;
        int index=0;
        for (int i = 0; i < RayAmount; i++)
        {
            if (RayResults[i].Weight > maxWeight && RayResults[i].Weight > 0)
            {
                index = i;
                MoveDirection = RayResults[i].Direction;
                LastMoveDirection = MoveDirection;
                maxWeight = RayResults[i].Weight;
            }
        }
        Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + RayResults[index].Weight * MoveDirection, Color.blue);
    }
}

public class MoveRayResult
{
    public Vector3 Direction;
    public RaycastHit HitResult;
    public float Weight;
}
public enum AIMoveType
{
    None,
    Chase,
    KeepDistance,
    RunAway,
    RandomMove
}