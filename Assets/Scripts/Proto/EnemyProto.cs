using System;
using System.Collections;
using System.Linq;
using DefaultNamespace.Proto;
using MisterGames.Collisions.Utils;
using MisterGames.Common.Layers;
using MisterGames.Common.Lists;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyProto : MonoBehaviour {
    public float hitDistance = 5;
    public GameObject navPoints;
    public Animation enemyAnimation;
    
    [Header("Think config")]
    public Vector2 thinkMinMax = new Vector2(1, 3);
    
    [Header("Speeds")]
    public float walkSpeed = 2;
    public float chaseSpeed = 3;
    public float runSpeed = 4.5f;
    
    
    private NavMeshAgent _agent;
    private LayerMask _psyMask;
    private LayerMask _wallMask;
    private readonly Collider[] _colliders = new Collider[5];
    private Transform _currentDestination;
    
    private Transform _currentPsyPoint;

    private bool _waiting;
    private State _state = State.Roaming;
    private Transform[] _points;

    private IEnumerator _currentRoutine;
    private RaycastHit _hit;
    private readonly RaycastHit[] _hitResults = new RaycastHit[5];

    enum State {
        Roaming,
        Thinking,
        Chasing,
        Running,
        Sucking,
    }
    
    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = walkSpeed;
        _points = navPoints.GetComponentsInChildren<Transform>();
        _psyMask = LayerMask.GetMask("Psy");
        _wallMask = LayerMask.GetMask("Wall", "Door");

        enemyAnimation.Play("Walk");
        SetNextRandomDestination();
    }

    void StopAgent() {
        _agent.isStopped = true;
    }

    void ResumeAgent() {
        _agent.isStopped = false;
    }

    Transform GetNearestPoint() {
        var collidersCount = Physics.OverlapSphereNonAlloc(transform.position, hitDistance, _colliders, _psyMask);
        
        var minDistance = Mathf.Infinity;
        Collider nearestCol = null;

        for (var i = 0; i < collidersCount; i++) {
            var col = _colliders[i];
            var distance = Vector3.Distance(col.transform.position, transform.position);
            
            if (distance < minDistance) {
                minDistance = distance;
                nearestCol = col;
            }
        }

        return nearestCol ? nearestCol.transform : null;
    }

    void SetNextRandomDestination() {
        SetDestination(GetRandomNavPoint());
    }

    void SetDestination(Transform destination) {
        _currentDestination = destination;
        _agent.destination = destination.position;
    }

    Transform GetRandomNavPoint() {
        Transform point;

        do {
            point = _points[Mathf.FloorToInt(Random.Range(0, _points.Length))];
        } while (Vector3.Distance(point.position, transform.position) < 2);
        
        return point;
    }

    bool CheckForPsyPointsAndSetIfPossible() {
        var point = GetNearestPoint();
        if (!point) return false;

        SetDestination(point);

        return true;
    }

    bool IsAgentReachedDestination() {
        return _agent.remainingDistance <= _agent.stoppingDistance && !_agent.isStopped;
    }

    bool IsSeeingPsyPoint() {
        var position = transform.position;
        var targetPosition = _currentDestination.position;
        var hitsCount = Physics.RaycastNonAlloc(new Ray(position, targetPosition - position), _hitResults, hitDistance, LayerMask.GetMask("Wall", "Door", "Psy", "Ignore Raycast"));

        if (hitsCount == 0) return false;
        
        _hitResults.SortByDistance(hitsCount);

        foreach (var hit in _hitResults) {
            var layer = hit.collider.gameObject.layer;

            if (_wallMask.Contains(layer)) return false;
            if (_psyMask.Contains(layer)) return true;
        }

        return false;
    }

    //Transition methods
    IEnumerator StartThink() {
        yield return new WaitForSeconds(thinkMinMax.x + Random.value * thinkMinMax.y);
        
        SetState(State.Roaming);
    }

    IEnumerator StartSuck() {
        var overloadable = _currentDestination.parent.GetComponent<IOverloadable>();

        if (overloadable != null) {
            overloadable.Overload();

            while (_currentDestination.gameObject.activeSelf) {
                yield return new WaitForSeconds(0.1f);
            }
            
            SetState(State.Roaming);
        }
    }

    IEnumerator StartChase() {
        yield return new WaitForSeconds(0.5f);
        
        enemyAnimation.CrossFade("Walk", 0.1f);
        ResumeAgent();
    }

    IEnumerator StartRun() {
        yield return new WaitForSeconds(0.6f);
        
        enemyAnimation.CrossFade("Run", 0.15f);
        ResumeAgent();
    }

    //State methods
    void StartStateRoutine(IEnumerator routine) {
        if (_currentRoutine != null) StopCoroutine(_currentRoutine);
        _currentRoutine = routine;
        StartCoroutine(routine);
    }
    
    void SetState(State newState) {
        var oldState = _state;
        
        Debug.Log(newState);
        _state = newState;
        
        switch (newState) {
            case State.Roaming:
                SetNextRandomDestination();
            
                _agent.speed = walkSpeed;
                ResumeAgent();
            
                enemyAnimation.CrossFade("Walk", 0.25f);
                break;
            case State.Thinking:
                StopAgent();

                enemyAnimation.CrossFade("Idle", 0.25f);
            
                StartStateRoutine(StartThink());
                break;
            case State.Chasing:
                StopAgent();
                _agent.speed = chaseSpeed;
                enemyAnimation.CrossFade("Attack1", 0.1f);

                StartStateRoutine(StartChase());
                break;
            case State.Running:
                StopAgent();
                _agent.speed = runSpeed;
                enemyAnimation.CrossFade("Attack2", 0.15f);

                StartStateRoutine(StartRun());
                break;
            case State.Sucking:
                enemyAnimation.CrossFade("Attack1", 0.25f);
            
                StartStateRoutine(StartSuck());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
    
    void Update() {
        switch (_state) {
            case State.Thinking: {
                if (CheckForPsyPointsAndSetIfPossible()) SetState(State.Chasing);
                
                break;
            }
            case State.Roaming: {
                if (CheckForPsyPointsAndSetIfPossible()) SetState(State.Chasing);
                
                if (IsAgentReachedDestination()) SetState(State.Thinking);
                
                break;
            }
            case State.Chasing: {
                if (IsSeeingPsyPoint()) {
                    SetState(State.Running);
                    break;
                }

                if (!CheckForPsyPointsAndSetIfPossible() && IsAgentReachedDestination()) {
                    SetState(State.Thinking);
                }
                
                break;
            }
            case State.Running: {
                SetDestination(_currentDestination);
                
                if (IsAgentReachedDestination() && !_currentDestination.CompareTag("Player")) SetState(State.Sucking);
                // if (!IsSeeingPsyPoint()) SetState(State.Chasing);

                break;
            }
            case State.Sucking:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
