using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Base : MonoBehaviour
{
    private Rigidbody rb;

    private CharacterManager Owner;
    public float BulletSpeed;
    public float BulletRange;
    public float BulletDamage;
    public float BulletGainEnergy;
    public bool HitWallDestroy;
    public bool HitEnemyDestroy;

    private float MovedDistance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void InitialBullet(BulletInitVariables variables)
    {
        Owner = variables.owner;
        BulletSpeed = variables.bulletSpeed;
        BulletRange = variables.bulletRange;
        BulletDamage = variables.bulletDamage;
        BulletGainEnergy = variables.bulletEnergy;
    }
    void Update()
    {
        float distance = BulletSpeed * Time.deltaTime;

        transform.Translate(0, 0, distance);
        MovedDistance += distance;

        if (MovedDistance >= BulletRange)
        {
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        StateManager stateManager = other.GetComponent<StateManager>();
        if (stateManager != null)
        {
            if (!other.isTrigger && GameMode_Base.CanDealDamage(Owner.stateManager, stateManager))
            {
                BulletHitEnemy(stateManager);
            }
        }
        else
        {
            BulletHitWall(other);
        }
    }
    public void BulletHitEnemy(StateManager enemyState)
    {
        enemyState.TakeDamage(BulletDamage, gameObject, Owner);
        Owner.stateManager.ChangeEnergy(BulletGainEnergy);
        if (HitEnemyDestroy)
        {
            DestroyBullet();
        }
    }
    public void BulletHitWall(Collider other)
    {
        if (HitWallDestroy)
        {
            DestroyBullet();
        }
    }
}
public struct BulletInitVariables
{
    public CharacterManager owner;
    public float bulletSpeed;
    public float bulletRange;
    public float bulletDamage;
    public float bulletEnergy;
}