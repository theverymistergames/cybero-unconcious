using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyProto : MonoBehaviour {
    public float hitDistance = 5;

    private NavMeshAgent _agent;
    private LayerMask _mask;
    private Collider[] _colliders = new Collider[3];
    private Transform _currentDestination;

    private bool waiting;
    
    void Start() {
        _agent = GetComponent<NavMeshAgent>();

        _mask = LayerMask.GetMask("Psy");
    }
    
    void Update() {
        var hitColliders = Physics.OverlapSphere(transform.position, hitDistance, _mask);
        
        foreach (var hitCollider in hitColliders) {
            if (hitColliders[0].transform != _currentDestination) {
                waiting = false;
            }
            
            _currentDestination = hitColliders[0].transform;
            _agent.SetDestination(_currentDestination.position);
        }

        if (!waiting && Vector3.Distance(_currentDestination.position, transform.position) < 0.7f && _agent.pathStatus == NavMeshPathStatus.PathComplete) {
            waiting = true;
            
            var cleaner = _currentDestination.parent.GetComponent<Cycleaner>();
            if (cleaner) {
                cleaner.StartOverload();
            }
        }
    }
}
