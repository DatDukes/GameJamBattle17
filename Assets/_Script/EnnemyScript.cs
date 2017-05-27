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

    Vector3 lightTarget;
    Vector3 playerTarget;
    Rigidbody2D _rb;
    float timer;
    float angle;

    void Start () {
        raycastOrigin = transform.FindChild("RaycastOrigin").gameObject;
        Player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody2D>();
        state = BehaviorState.Idle;
    }
	
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
    }

    void aniamtionManager() {
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
            print(animState);
            timer = 0;
        }
    }

    void Chase() {
        Vector2 dir = playerTarget - transform.position;
        dir.Normalize();
        _chaseHit = (RaycastHit2D)Physics2D.Raycast(transform.position, dir, 100f, OtherMask);
        if (_chaseHit.transform != null){
            _rb.AddForce(dir * speed, ForceMode2D.Impulse);
        }
        else {
            dir = lightTarget - transform.position;
            _rb.AddForce(dir.normalized * speed, ForceMode2D.Impulse);
        }
    }

    void Detect() {
        _ligthHit = (RaycastHit2D)Physics2D.Raycast(raycastOrigin.transform.position, raycastOrigin.transform.right, ViewDistance, LightMask);
        if (_ligthHit.transform != null && _ligthHit.collider.tag == "Light"){
            Vector2 dir = (Vector2)Player.transform.position - _ligthHit.point;
            _otherHit = (RaycastHit2D)Physics2D.Raycast(_ligthHit.point, dir.normalized, 100f, OtherMask);
            lightTarget = _ligthHit.point;
            if (_otherHit.transform != null){
                if (_otherHit.transform.tag == "Player"){
                    print("Spoted");
                    state = BehaviorState.Chase;
                    playerTarget = _otherHit.point;
                }
            }
        }

        _ligthHit = new RaycastHit2D();
        _otherHit = new RaycastHit2D();
    }
}
