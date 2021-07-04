using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    public StateManager stateManager;
    [SerializeField]
    private Transform WeaponSlot;

    public float MoveSpeed;
    public float Acceleration;
    public float TurnSpeed;
    public MovementSetting[] movementSettings;
    public int CurrentMoveType;
    public int DefaultMoveType;

    public Vector3 ActionDirection;
    public Vector3 MoveDirection;

    public Action_Base CurrentAction;

    public Weapon_base CurrentWeapon;
    public GameObject DefaultWeaponPrefab;
    public GameObject UnitUIPrefab;
    private GameObject UnitUIInstance;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        SetDefaultMoveMode(1);
        GameObject weaponObject = Instantiate(DefaultWeaponPrefab,WeaponSlot);
        CurrentWeapon = weaponObject.GetComponent<Weapon_base>();
        CurrentWeapon.Equip(this);
        UnitUIInstance = Instantiate(UnitUIPrefab, transform.position, Quaternion.identity);
        UnitUIInstance.GetComponent<UI_UnitState>().InitialUnitState(gameObject);
    }
    private void Update()
    {
        Turn();
        if (CurrentAction != null)
        {
            CurrentAction.UpdateAction(Time.deltaTime);
        }
    }
    public void Move(Vector3 direction)
    {
        if(direction.sqrMagnitude> 0.01f)
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, direction.normalized * MoveSpeed, Acceleration * Time.deltaTime);
            MoveDirection = direction;
        }
        else
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity,Vector3.zero, Acceleration * Time.deltaTime);
            MoveDirection = Vector3.zero;
        }
        UpdateAnimationSpeed(rb.velocity);
    }
    public void Turn()
    {
        Quaternion targetRotation = transform.rotation;
        if (CurrentAction != null)
        {
            targetRotation = Quaternion.LookRotation(ActionDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime * 2);
        }
        else if (MoveDirection != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(rb.velocity);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);
        }
    
    }
    public void SetMoveMode(int index)
    {
        if (movementSettings.Length > index)
        {
            CurrentMoveType = index;
            animator.SetFloat("MoveType", movementSettings[index].MoveTypeIndex);
            MoveSpeed = movementSettings[index].MoveSpeed;
            TurnSpeed = movementSettings[index].TurnSpeed;
            Acceleration = movementSettings[index].Acceleration;
        }
    }
    public void SetDefaultMoveMode(int index)
    {
        DefaultMoveType = index;
        if (CurrentAction == null)
        {
            SetMoveMode(index);
        }     
    }
    public void ResetMoveMode()
    {
        SetMoveMode(DefaultMoveType);
    }
    public void UpdateAnimationSpeed(Vector3 velocity)
    {
        float direction = 0;
        float speed = velocity.sqrMagnitude / MoveSpeed / MoveSpeed;
        direction = Vector3.SignedAngle(velocity, transform.forward, Vector3.up);
        animator.SetFloat("Direction", direction, 0.5f, Time.deltaTime);
        animator.SetFloat("Speed", speed, 0.5f, Time.deltaTime);
    }
    public void Attack(Vector3 direction)
    {
        if (CurrentWeapon != null)
        {
            if (CurrentAction == null && CurrentWeapon.CanAttack())
            {
                Action_Base newAction = Instantiate(CurrentWeapon.AttackAction);
                newAction.IntializeAction(this, direction);
                CurrentWeapon.Attack(direction);

                ActionDirection = direction;
                StartAction(newAction);
            }
        }

    }
    public void Skill(Vector3 direction)
    {
        if (CurrentWeapon != null)
        {
            if (CurrentAction == null && CurrentWeapon.CanSkill())
            {
                Action_Base newAction = Instantiate(CurrentWeapon.SkillAction);
                newAction.IntializeAction(this, direction);
                CurrentWeapon.Skill(direction);

                ActionDirection = direction;
                StartAction(newAction);
            }
        }
    }
    public void StartAction(Action_Base action)
    {
        CurrentAction = action;
        if (action.AnimClip != null)
        {
            animator.Play(action.AnimClip.name,1,0);
        }
    }
    public void EndAction(Action_Base action)
    {
        if (action == CurrentAction)
        {
            CurrentAction = null;
        }
    }
    public void Death(GameObject damageCauser, CharacterManager damageInstigator)
    {
        Destroy(UnitUIInstance);
        Destroy(gameObject);
    }
}
