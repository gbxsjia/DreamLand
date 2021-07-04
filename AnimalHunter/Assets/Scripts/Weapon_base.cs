using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_base : MonoBehaviour
{
    public Action_Base AttackAction;
    public Action_Base SkillAction;
    public GameObject AttackBulletPrefab;

    public CharacterManager WeaponOwner;

    public int BulletMax = 3;
    public float BulletCurrent;
    public float BulletReloadSpeed;

    public BulletLaunchInfo[] BulletLaunchInfos;
    public float BulletDamage;
    public float BulletRange;
    public float BulletSpeed;
    public float AIAttackRange;

    private Coroutine AttackCoroutine;

    private void Update()
    {
        Reload(BulletReloadSpeed * Time.deltaTime);
    }
    public void Reload(float amount)
    {
        BulletCurrent += amount;
        BulletCurrent = Mathf.Min(BulletCurrent, BulletMax);
    }
    public void Equip(CharacterManager weaponOwner)
    {
        WeaponOwner = weaponOwner;
    }
    public bool CanAttack()
    {
        return BulletCurrent >= 1;
    }
    public bool CanSkill()
    {
        return true;
    }
    public void Attack(Vector3 direction)
    {
        // 同时发射多发，多个角度，多个波次，出生位置的偏移
        BulletCurrent--;
        AttackCoroutine = StartCoroutine(AttackProcess(direction));
    }
    public void Skill(Vector3 direction, Action_Base action)
    {

    }
    private IEnumerator AttackProcess(Vector3 direction)
    {
        float StartTime = Time.time;
        int counter = 0;
        while (counter < BulletLaunchInfos.Length)
        {
            if (Time.time - StartTime > BulletLaunchInfos[counter].Time)
            {
                Vector3 BulletPosition = WeaponOwner.transform.position + Vector3.up;
                BulletPosition += WeaponOwner.transform.right * BulletLaunchInfos[counter].Offset.x + WeaponOwner.transform.forward * BulletLaunchInfos[counter].Offset.y;
                Quaternion BulletRotation = Quaternion.LookRotation(direction);
                BulletRotation *= Quaternion.Euler(0, BulletLaunchInfos[counter].Angle, 0);

                GameObject bulletObject = Instantiate(AttackBulletPrefab,BulletPosition, BulletRotation);
                Bullet_Base bullet = bulletObject.GetComponent<Bullet_Base>();
                bullet.InitialBullet(WeaponOwner, BulletSpeed, BulletRange, BulletDamage);

                counter++;
            }
            else
            {
                yield return null;
            }            
        }

        yield return null;
    }


    public void Skill(Vector3 direction)
    {

    }
}

[System.Serializable]
public class BulletLaunchInfo
{
    public float Time;
    public float Angle;
    public Vector2 Offset;
}