using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _chaseDistance = 5;
    [SerializeField] private float _attackDistance = 2;
    [SerializeField] private float _changeNodeDistance = 0.1f;
    
    NavMeshAgent _agent;

    [SerializeField] List<Transform> _navMeshNodes;

    private Transform _playerTarget, _currentNode;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //_playerTarget = PlayerRef.player.transform;
        _playerTarget = PlayerReference.Instance.player.transform;

        Initialize();

        StartCoroutine(BurnDamage());
    }

    void Initialize()
    {
        _currentNode = GetNewNode();
        _agent.SetDestination(_currentNode.position);
    }

    Transform GetNewNode()
    {
        var newNode = _navMeshNodes[Random.Range(0, _navMeshNodes.Count)];

        return newNode;
    }

    private void Update()
    {
        if (!_playerTarget)
        {
            Debug.LogError("No player found");
            return;
        }

        //SqrMagnitude no aplica la raiz cuadrada para obtener la magnitude
        var distanceToPlayer = Vector3.SqrMagnitude(_playerTarget.position - transform.position);
        
        //Si el jugador esta cerca como para seguirlo
        if (distanceToPlayer <= Mathf.Pow(_chaseDistance, 2))
        {
            //Y ademas esta cerca para atacarlo
            if (distanceToPlayer <= Mathf.Pow(_attackDistance, 2))
            {
                //Lo atacamos
                if (!_agent.isStopped)
                    _agent.isStopped = true;
                
                Debug.Log("Japish");
            }
            else // Sino
            {
                //Lo seguimos
                if (_agent.isStopped)
                    _agent.isStopped = false;

                _agent.SetDestination(_playerTarget.position);
            }
        }
        else
        {
            var distanceToNode = Vector3.SqrMagnitude(_currentNode.position - transform.position);



            if (distanceToNode <= Mathf.Pow(_changeNodeDistance, 2))
            {
                _currentNode = GetNewNode();
                _agent.SetDestination(_currentNode.position);
            }
            else
            {
                //Debug.LogWarning(Vector3.Distance(_currentNode.position, transform.position));
                _agent.SetDestination(_currentNode.position);
            }
        }
    }

    IEnumerator BurnDamage()
    {
        int burningTimes = 3;

        int currentBurningTimes = 0;
        
        float damageTicks = 1;
        
        while (currentBurningTimes < burningTimes)
        {
            currentBurningTimes++;
            Debug.LogWarning("Damage");
            yield return new WaitForSeconds(damageTicks);
        }
    }
}
