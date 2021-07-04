using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    public GameObject Target;
    public bool IsInBattle;

    public AI_Move MoveComp;
    public CharacterManager characterManager;
    public StateManager stateManager;

    public AIState_Base[] AllStates;
    public AIState_Base CurrentState;
    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        stateManager = GetComponent<StateManager>();
        MoveComp = GetComponent<AI_Move>();
    }

    private void Start()
    {
        for (int i = 0; i < AllStates.Length; i++)
        {
            AllStates[i] = Instantiate(AllStates[i]);
        }
        if (stateManager.campType == CampType.Player)
        {
            BattleManager.instance.BattleModeChangeEvent += OnBattleModeChange;
        }
    }
    public AIState_Base SearchState()
    {
        AIState_Base result = null;
        int MaxWeight=-1;
        for (int i = 0; i < AllStates.Length; i++)
        {
            if (AllStates[i].StateCondition(this) && AllStates[i].Weight >= MaxWeight)
            {
                result = AllStates[i];
                MaxWeight = AllStates[i].Weight;
            }
        }
        return result;
    }

    private void Update()
    {
        if (CurrentState == null)
        {
            AIState_Base newState = SearchState();
            if (newState)
            {
                CurrentState =newState;
                newState.StateBegin(this);
            }
            else
            {
                Debug.LogError("No State Matches!");
            }
        }
        else
        {
            if (CurrentState.StateCondition(this))
            {
                CurrentState.UpdateFunction();
            }
            else
            {
                CurrentState.StateEnd();
                CurrentState = null;
            }
        }
    }
    public void OnEnemyEnter(GameObject enemy)
    {
        if (!IsInBattle)
        {
            Target = enemy;
            IsInBattle = true;
            BattleManager.instance.ReportBattleEnemy(this);
            Collider[] colliders = Physics.OverlapSphere(transform.position, 6);
            foreach(Collider c in colliders)
            {
                StateManager sm = c.GetComponent<StateManager>();
                if (sm && sm.campType == stateManager.campType)
                {
                    AIBrain brain = c.GetComponent<AIBrain>();
                    if (brain &&!brain.IsInBattle)
                    {
                        brain.OnEnemyEnter(enemy);
                    }
                }
            }
        }
    }
    public void OnBattleModeChange(bool newBattleMode)
    {
        if (newBattleMode && !IsInBattle)
        {
            List<GameObject> enemyList = new List<GameObject>();
            foreach (AIBrain brain in BattleManager.instance.BattleEnemies)
            {
                if(brain && brain.IsInBattle)
                {
                    enemyList.Add(brain.gameObject);
                }
            }
            FindNearestEnemy(enemyList);
            IsInBattle = true;
        }
        else if ( !newBattleMode && IsInBattle)
        {
            IsInBattle = false;
        }
    }
    public void FindNearestEnemy(List<GameObject> enemyList)
    {
        float minDistance = Mathf.Infinity;
        foreach (GameObject obj in enemyList)
        {
            float dis = Vector3.Distance(obj.transform.position, transform.position);
            if (dis < minDistance)
            {
                minDistance = dis;
                Target = obj.gameObject;
            }
        }
    }
}