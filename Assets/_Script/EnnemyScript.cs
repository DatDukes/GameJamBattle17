using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviorState { Idle, Chase, Search};
public enum GuardAnimState { Right, Up, Left, Down };


public class EnnemyScript : MonoBehaviour {
    public float ViewDistance;
    public float speed;
    BehaviorState state;
    GuardAnimState animState;

    GameObject raycastOrigin;
    GameObject Player;
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
    Rigidbody2D _rb;
    float timer;
    float angle;
    Light2D _light;

    void Start () {
        raycastOrigin = transform.FindChild("RaycastOrigin").gameObject;
        _light = GetComponentInChildren<Light2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody2D>();
        state = BehaviorState.Idle;
        _light.range = ViewDistance;
 "   }
	
	void Update () {
        Detect();
        switch (state) {
            case (BehaviorState.Idle):
                Idle();
                break;

            case (BehaviorState.Chase):
                Chase();
                break;

            case (BehaviorState.Search):

                break;
        }
		aniamtionManager ();
    }

    void aniamtionManager() {
		Vector3 myVelocity = _rb.velocity;
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
        
    }

    void Idle(){
        timer += Time.deltaTime;
        if (timer > 2f) {
            angle += 90;
            raycastOrigin.transform.rotation = Quaternion.Euler(0, 0, angle);
            animState++;
            if ((int)animState >= 4) {
                animState = GuardAnimState.Right;
            }
            timer = 0;
        }
    }

    void Chase() {
        Vector2 dir = playerTarget - transform.position;
        dir.Normalize();
        _chaseHit = (RaycastHit2D)Physics2D.Raycast(transform.position, dir, 100f, OtherMask);
        if (_chaseHit.transform != null){
            _rb.AddForce(dir * speed, ForceMode2D.Impulse);
            raycastOrigin.transform.rotation = setRotation(dir);
        }
        else {
            dir = lightTarget - transform.position;
            _rb.AddForce(dir.normalized * speed, ForceMode2D.Impulse);
            raycastOrigin.transform.rotation = setRotation(dir);
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
                Vector2 dir = (Vector2)Player.transform.position - _ligthHit.point;
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
        else
        {
            state = BehaviorState.Search;
        }
    }
}
