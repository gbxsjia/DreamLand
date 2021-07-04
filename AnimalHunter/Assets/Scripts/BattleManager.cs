using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    bool oldBattleMode = false;
    public static BattleManager instance;
    public GameObject PlayerOject;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        for (int i = BattleEnemies.Count - 1; i >= 0; i--)
        {
            if (!BattleEnemies[i] || !BattleEnemies[i].IsInBattle)
            {
                BattleEnemies.RemoveAt(i);
            }
        }
        if(BattleEnemies.Count == 0 && oldBattleMode && BattleModeChangeEvent != null)
        {
            oldBattleMode = false;
            BattleModeChangeEvent(false);
        }
    }
    public List<AIBrain> BattleEnemies = new List<AIBrain>();
    public event System.Action<bool> BattleModeChangeEvent;
    public void ReportBattleEnemy(AIBrain brain)
    {
        BattleEnemies.Add(brain);

        if (!oldBattleMode && BattleModeChangeEvent!=null)
        {            
            BattleModeChangeEvent(true);
        }
        oldBattleMode = true;
    }
    public bool IsInBattleMode()
    {
        foreach(AIBrain brain in BattleEnemies)
        {
            if(brain && brain.IsInBattle)
            {
                return true;
            }
        }
        return false;
    }
}
