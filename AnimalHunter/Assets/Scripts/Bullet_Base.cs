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
    public bool HitWallDestroy;
    public bool HitEnemyDestroy;

    private float MovedDistance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void InitialBullet(CharacterManager owner, float bulletSpeed, float bulletRange, float bulletDamage)
    {
        Owner = owner;
        BulletSpeed = bulletSpeed;
        BulletRange = bulletRange;
        BulletDamage = bulletDamage;
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
