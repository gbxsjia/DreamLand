using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private void Start()
    {
        BattleManager.instance.PlayerOject = gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        StateManager sm = other.GetComponent<StateManager>();
        if (sm && sm.campType== CampType.Enemy)
        {
            AIBrain brain = other.GetComponent<AIBrain>();
            if (brain)
            {
                brain.OnEnemyEnter(gameObject);
            }
        }
    }
}
