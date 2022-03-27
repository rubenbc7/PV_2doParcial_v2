using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(InputsController))]
public class Hero : Character, IHostile
{
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected JobsOptions jobsOptions;
    [SerializeField]
    CharacterJob currentJob;
    [SerializeField]
    float leaderMinDistance;

    bool IsFollowing = false;

    [SerializeField]
    Vector2 minMaxAngle;
    protected float movementValue;
    protected InputsController inputsController;

    new void Awake()
    {
        base.Awake();
        inputsController = GetComponent<InputsController>();
    }

    void Start()
    {
        agent.speed = moveSpeed;
        agent.stoppingDistance = leaderMinDistance;
        agent.enabled = !ImLeader;
    }

    protected override void Movement()
    {
        Hero leader = Gamemanager.Instance.CurrentGameMode.GetPartyLeader.GetComponent<Hero>();

        if(ImLeader)
        {
            //IsFollowing = false;
            base.Movement();
            transform.Translate(inputsController.Axis.normalized.magnitude * Vector3.forward * moveSpeed * Time.deltaTime);
            FacingDirection();
            movementValue = leader.IsMoving ? 1 : 0f;
        }
        else
        {
            agent.destination = leader.transform.position;
            movementValue = agent.velocity != Vector3.zero ? 1 : 0f;
        }
    }

    protected void LateUpdate()
    {

    }

    public void Attack()
    {

    }

    public int GetDamage()
    {
        return damage;
    }

/// <summary>
/// Checks if you are the leader of the party.
/// </summary>
/// <returns>Returns a true/false depending of the comparing with leader transform.</returns>
    public bool ImLeader => Gamemanager.Instance.CurrentGameMode.CompareToLeader(transform);

    protected void FacingDirection()
    {
        if(IsMoving)
        {
            transform.rotation = RotationDirection;
        }
    }

    Quaternion RotationDirection => Quaternion.LookRotation(inputsController.Axis);

    public bool IsMoving => inputsController.Axis != Vector3.zero;

    public CharacterJob CurrentJob{get => currentJob; set => currentJob = value;}
    public JobsOptions GetJobsOptions => jobsOptions;
    public NavMeshAgent GetAgent => agent;

    public InputsController GetInputsController => inputsController;

}
