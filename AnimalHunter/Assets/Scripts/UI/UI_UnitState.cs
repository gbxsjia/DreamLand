using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitState : MonoBehaviour
{
    public Transform ownerTransform;
    public StateManager stateManager;
    public CharacterManager characterManager;
    public Image HealthBar;
    public Image BulletBar;

    public void InitialUnitState(GameObject owner)
    {
        ownerTransform = owner.transform;
        stateManager = owner.GetComponentInParent<StateManager>();
        characterManager = owner.GetComponentInParent<CharacterManager>();
    }
  
    public void Update()
    {
        if (stateManager != null)
        {
            HealthBar.fillAmount = stateManager.HealthCurrent / stateManager.HealthMax;
            if (characterManager.CurrentWeapon != null)
            {
                BulletBar.fillAmount = characterManager.CurrentWeapon.BulletCurrent / characterManager.CurrentWeapon.BulletMax;
            }           
        }
        if (ownerTransform != null)
        {
            transform.position = ownerTransform.position;
        }
    }
}
