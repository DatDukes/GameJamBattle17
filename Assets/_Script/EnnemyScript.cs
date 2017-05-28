using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BehaviorState { Idle, Chase, Search};
public enum GuardAnimState { Right, Up, Left, Down };


public class EnnemyScript : MonoBehaviour {
    public float ViewDistance;
    public float speed;

    public Transform[] wayPoint;
    int ActualWP = 1;

    BehaviorState state;
    GuardAnimState animState;
    NavMeshAgent agent;

    GameObject raycastOrigin;
    GameObject Player;
    PlayerScript _playerScript;
    RaycastHit2D _ligthHit;
    RaycastHit2D _otherHit;
    RaycastHit2D _chaseHit;
    public LayerMask LightMask;
    public LayerMask OtherMask;
	public SpriteRenderer sprRenderer;
	public Sprite Up,Down,Left,Right;


    Vector3 lightTarget;
    Vector3 playerTarget;
    Vector3 SearchPos;
    Vector3 TargetRotation;
    Vector3 BasePosition;
    Vector3 random;
    float timer;
    float timerSearch;
    float angle;
    Light2D _light;



    void Start () {
        raycastOrigin = transform.FindChild("RaycastOrigin").gameObject;
        agent = GetComponent<NavMeshAgent>();
        _light = GetComponentInChildren<Light2D>();
        Player = GameObject.Find("PlayerOne");
        _playerScript = Player.GetComponent<PlayerScript>();
        state = BehaviorState.Idle;
        _light.range = ViewDistance;
        BasePosition = transform.position;  
    }
	
	void Update () {
        Detect();
        switch (state) {
            case (BehaviorState.Idle):
                Idle();
                break;

            case (BehaviorState.Chase):
                Chase();
                aniamtionManager();
                break;

            case (BehaviorState.Search):
                Search();
                break;
        }
        DealDomage();
    }

    void DealDomage() {
        if (Vector3.Distance(transform.position, Player.transform.position) < 2f) {
            _playerScript.loseLife();
        }
    }

    void aniamtionManager() {
		Vector3 myVelocity = agent.velocity;
		if (Mathf.Abs (myVelocity.x) >= Mathf.Abs (myVelocity.y)) {
			if (myVelocity.x >= 0) {
				sprRenderer.sprite = Right;
			} else {
				sprRenderer.sprite = Left;
			}
		} else {
			if (myVelocity.y >= 0) {
				sprRenderer.sprite = Up;
			} else {
				sprRenderer.sprite = Down;
			}
		}

    }

    void Search(){
        timerSearch += Time.deltaTime;
        if (timerSearch > 3f)
        {
            agent.SetDestination(BasePosition);
            aniamtionManager();
            if (agent.velocity.normalized.magnitude > 0.05f)
            {
                raycastOrigin.transform.rotation = setRotation(agent.velocity.normalized);
            }
            if (Vector3.Distance(transform.position, BasePosition) < 0.05f)
            {
                state = BehaviorState.Idle;
                timerSearch = 0;
            }
        }
    }

    void Idle(){
        if (wayPoint.Length <= 1)
        {
            timer += Time.deltaTime;
            if (timer > 2f)
            {
                angle += 90;
                if (angle > 360)
                {
                    raycastOrigin.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
                angle = angle % 361;
                TargetRotation = new Vector3(0, 0, angle);
                animState++;
                if ((int)animState >= 4)
                {
                    animState = GuardAnimState.Right;
                }
                switch (animState)
                {
                    case(GuardAnimState.Right):
                        sprRenderer.sprite = Right;
                        break;

                    case (GuardAnimState.Left):
                        sprRenderer.sprite = Left;
                        break;

                    case (GuardAnimState.Up):
                        sprRenderer.sprite = Up;
                        break;

                    case (GuardAnimState.Down):
                        sprRenderer.sprite = Down;
                        break;
                }

                timer = 0;
            }
            updateRotation();
        }
        else {
            agent.SetDestination(wayPoint[ActualWP].position);
            if (agent.velocity.normalized.magnitude > 0.05f)
            {
                raycastOrigin.transform.rotation = setRotation(agent.velocity.normalized);
            }

            if (Vector3.Distance(wayPoint[ActualWP].position, transform.position) < 0.5f) {
                ActualWP = (ActualWP + 1) % wayPoint.Length;
            }
            aniamtionManager();
        } 
    }

    void updateRotation() {
        raycastOrigin.transform.rotation = Quaternion.Euler(Vector3.Lerp(raycastOrigin.transform.eulerAngles, TargetRotation, Time.deltaTime * 3));
    }

    void Chase() {
        Vector2 dir = playerTarget - transform.position;
        dir.Normalize();
        _chaseHit = (RaycastHit2D)Physics2D.Raycast(transform.position, dir, 100f, OtherMask);
        if (_chaseHit.transform != null){
            agent.SetDestination(playerTarget);
            if (dir.magnitude > 0.05f)
            {
                raycastOrigin.transform.rotation = setRotation(dir);
            }
        }
        else {
            dir = lightTarget - transform.position;
            agent.SetDestination(lightTarget);
            if (dir.magnitude > 0.05f)
            {
                raycastOrigin.transform.rotation = setRotation(dir);
            }
        }
    }

    Quaternion setRotation(Vector3 direction) {
        if (direction.y >= 0)
        {
            return Quaternion.Euler(new Vector3(0, 0, Mathf.Acos(direction.x / direction.magnitude) * Mathf.Rad2Deg));
        }
        else if (direction.y < 0 && direction.x < 0)
        {
            return Quaternion.Euler(new Vector3(0, 0, Mathf.Asin(direction.x / direction.magnitude) * Mathf.Rad2Deg - 90));
        }
        else
        {
            return Quaternion.Euler(new Vector3(0, 0, Mathf.Asin(direction.y / direction.magnitude) * Mathf.Rad2Deg));
        }
    }

    void Detect() {
        _ligthHit = new RaycastHit2D();
        _otherHit = new RaycastHit2D();

        bool hit = false;
        Vector3[] rayTarget = new Vector3[3];
        rayTarget[0] = (raycastOrigin.transform.right + raycastOrigin.transform.up * 0.415f);
        rayTarget[1] = (raycastOrigin.transform.right + raycastOrigin.transform.up * -0.415f);
        rayTarget[2] = raycastOrigin.transform.right ;

        foreach(Vector3 v in rayTarget)
        {
            _ligthHit = (RaycastHit2D)Physics2D.Raycast(raycastOrigin.transform.position, v, ViewDistance, LightMask);
            if (_ligthHit.transform != null && _ligthHit.collider.tag == "Light")
            {
                Vector2 dir = (Vector2)_ligthHit.transform.position - _ligthHit.point;
                _otherHit = (RaycastHit2D)Physics2D.Raycast(_ligthHit.point, dir.normalized, 100f, OtherMask);
                lightTarget = _ligthHit.point;
                if (_otherHit.transform != null)
                {
                    if (_otherHit.transform.tag == "Player")
                    {
                        hit = true;
                        playerTarget = _otherHit.point;
                    }
                }
            }
        }

        if (hit)
        {
            state = BehaviorState.Chase;
        }
        else if(state == BehaviorState.Chase)
        {
            state = BehaviorState.Search;
        }
    }
}
