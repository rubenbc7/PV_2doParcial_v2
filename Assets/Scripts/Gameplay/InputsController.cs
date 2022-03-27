using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsController : MonoBehaviour
{
    GameInputs gameInputs;

    Hero hero;

    void Awake()
    {
        gameInputs = new GameInputs();
    }

    void OnEnable()
    {
        gameInputs.Enable();
    }

    void OnDisable()
    {
        gameInputs.Disable();
    }

    void Start()
    {
        hero = GetComponent<Hero>();
        ChangeJob(hero.GetJobsOptions);
        gameInputs.Gameplay.ChangeJob.performed += _=> ChangeJob(hero.GetJobsOptions);
        gameInputs.Gameplay.ChangeLeader.performed += _=> PassLeaderToNextone();
    }

    void ChangeJob(JobsOptions job)
    {

        if(hero.CurrentJob)
        {
            Destroy(hero.CurrentJob);
        }
        switch(job)
        {
            case JobsOptions.MAGE:
            hero.CurrentJob = gameObject.AddComponent<Mage>();
            break;
            case JobsOptions.ARCHER:
            hero.CurrentJob = gameObject.AddComponent<Archer>();
            break;
            case JobsOptions.WARRIOR:
            hero.CurrentJob = gameObject.AddComponent<Warrior>();
            break;
        }
    }

    void PassLeaderToNextone()
    {
        hero.GetAgent.enabled = false;
        Gamemanager.Instance.CurrentGameMode.ChangeLeader(transform);
        Hero heroLeader = Gamemanager.Instance.CurrentGameMode.GetPartyLeader.GetComponent<Hero>();
        heroLeader.GetAgent.enabled = true;
        heroLeader.GetInputsController.enabled = true;
        this.enabled = false;
    }

    public GameInputs GetGameinputs => gameInputs;

    public Vector3 Axis => new Vector3(gameInputs.Gameplay.Horizontal.ReadValue<float>(), 0f, gameInputs.Gameplay.Vertical.ReadValue<float>());
}
