using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    public Transform ColliderTransform;
    private Collider _weaknessCollider;
    private BoostManager boostManager;

    private Transform _target;

    public Rigidbody _rigidbody;

    public float _currentPower;
    public float _currentMass;
    public int _currentScore;

    private PlayerController _playerController;
    private bool _onGround;
    #region AI
    enum State
    {
        Loot,
        Attack,
    }
    [SerializeField] private State currentState = State.Loot;
    private NavMeshAgent navMeshAgent;

    #endregion

    #region Props
    public float Power
    {
        get => _currentPower;
        set => _currentPower = value;
    }

    public float Mass
    {
        get => _currentMass;
        set => _currentMass = value;
    }

    public int Score
    {
        get => _currentScore;
        set => _currentScore = value;
    }

    #endregion

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        _weaknessCollider = ColliderTransform.GetChild(0).GetComponent<Collider>();
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        _currentPower = 2;
        _currentMass = _rigidbody.mass;
        _currentScore = 0;
        _onGround = true;
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.mass = _currentMass;
        if (GameObject.FindGameObjectsWithTag("Boost").Length == 0)
        {
            boostManager = FindObjectOfType<BoostManager>();
            boostManager.SpawnBoost(8);
            Debug.Log("23123123");
        }

        if (!_playerController._isGameFinish)
        {
            StateExecute();
        }

        if (_currentPower > _playerController.Power)
        {
            currentState = State.Attack;
        }
        else
        {
            currentState = State.Loot;
        }

        if (!_onGround)
        {
            navMeshAgent.enabled = false;
        }

        RaycastCheck();
    }

    void StateExecute()
    {
        switch (currentState)
        {
            case State.Loot:
                if (navMeshAgent.enabled)
                {
                    navMeshAgent.SetDestination(FindClosestBoost().position);
                }
                break;

            case State.Attack:
                navMeshAgent.SetDestination(FindClosestEnemy().position);
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();

        if (!collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("Boost"))
        {
            if (collision.gameObject.name == "BotWeakness" && collision.gameObject != _weaknessCollider)
            {
                
                _currentPower *= 2;
                Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
                enemyRigidbody.AddForce(awayFromPlayer * _currentPower, ForceMode.Impulse);
            }

           if (_currentMass >= enemyRigidbody.mass )
           {
               Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
               enemyRigidbody.AddForce(awayFromPlayer * _currentPower, ForceMode.Impulse);
           }
           else
           {
                navMeshAgent.enabled = false;
                StartCoroutine("BotNavMesh", 3);
           }
        }

    }

    public Transform FindClosestBoost()
    {
        GameObject[] boost;
        boost = GameObject.FindGameObjectsWithTag("Boost");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in boost)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest.transform;
    }

    public Transform FindClosestEnemy()
    {
        GameObject[] enemmy;
        enemmy = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in enemmy)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest.transform;
    }

    private IEnumerator BotNavMesh(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        navMeshAgent.enabled = true;
    }

    void RaycastCheck()
    {
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down)*2))
        {
            _onGround = false;
        }
    }
}
