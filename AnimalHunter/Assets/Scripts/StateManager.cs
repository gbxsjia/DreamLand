using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public CharacterManager characterManager;

    public float HealthMax;
    public float HealthCurrent;

    public float HealthReginDelay = 3;
    public float HealthReginSpeed = 1;
    public float HealthReginAmount = 0.2f;
    private float HealthReginTimer;
    private float LastReginTime;

    public float EnergyCurrent;

    public CampType campType;
    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        characterManager.stateManager = this;
        InitializeState();
    }
    private void Update()
    {
        if(Time.time- HealthReginTimer >= HealthReginDelay)
        {
            if(Time.time- LastReginTime>= HealthReginSpeed)
            {
                Heal(HealthMax * HealthReginAmount);
                LastReginTime = Time.time;
            }
        }
    }
    public void InitializeState()
    {
        HealthCurrent = HealthMax;
    }
    public void Heal(float amount)
    {
        HealthCurrent += amount;
        HealthCurrent = Mathf.Min(HealthMax, HealthCurrent);
    }
    public void TakeDamage(float damageAmount,GameObject damageCauser,CharacterManager damageInstigator)
    {
        HealthCurrent -= damageAmount;
        if (HealthCurrent <= 0)
        {
            Death(damageCauser,damageInstigator);
        }
        HealthReginTimer = Time.time;
    }
    public void ChangeEnergy(float amount)
    {
        EnergyCurrent += amount;
        EnergyCurrent = Mathf.Clamp01(EnergyCurrent);
    }
    public void Death(GameObject damageCauser, CharacterManager damageInstigator)
    {
        characterManager.Death(damageCauser, damageInstigator);
    }

    public float GetHealthPercent()
    {
        return HealthCurrent/HealthMax;
    }
}
public enum CampType
{
    Player,
    Enemy,
    Netural
}