using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum animationState {Idle, Left, Right, Up, Down};

public class PlayerScript : MonoBehaviour {
    Rigidbody2D _rb;
    Animator _anim;
    Light2D _light;
    animationState animState;
    float _speed;
    float _remainingLight;
    bool _running; 

    internal float _speedRatio = 1;

    //Variable visible dans l'inspector
    [SerializeField]
    private float _walkspeed;
    [SerializeField]
    private float _runSpeed;
    [SerializeField]
    private float _maxLight;
    [SerializeField]
    private float _lightDecreaseSpeed;
    [SerializeField]
    private float _lightIncreaseSpeed;

    void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _light = GetComponentInChildren<Light2D>();
        _anim = GetComponentInChildren<Animator>();
        _remainingLight = _maxLight;
        animState = animationState.Idle;
    }
	
	void Update () {
        MovementUpdate();
        LightUpdate();
        AnimationManager();
    }

    void MovementUpdate() {
        if (Input.GetButton("Run")){
            _speed = _runSpeed * _speedRatio;
            _running = true;
        }
        else {
            _speed = _walkspeed * _speedRatio;
            _running = false;
        }

        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        _rb.AddForce(movement.normalized * _speed, ForceMode2D.Impulse);
    }

    void LightUpdate() {
        if (_running && _rb.velocity.magnitude > 1) {
            _remainingLight -= Time.deltaTime * _lightDecreaseSpeed;
            if (_remainingLight <= 0) {
                Die();
            }
        }
        else if (_remainingLight < _maxLight) {
            _remainingLight += Time.deltaTime * _lightIncreaseSpeed;
        }

        _light.range = _remainingLight;
    }

    void AnimationManager() {
        if (Input.GetAxis("Horizontal") > 0.1f){
            if (animState != animationState.Right) {
                _anim.SetTrigger("Right");
                animState = animationState.Right;
            }
        }
        else if (Input.GetAxis("Horizontal") < -0.1f){
            if (animState != animationState.Left)
            {
                _anim.SetTrigger("Left");
                animState = animationState.Left;
            }
        }
        else if (Input.GetAxis("Vertical") > 0.1f){
            if (animState != animationState.Up)
            {
                _anim.SetTrigger("Up");
                animState = animationState.Up;
            }
        }
        else if (Input.GetAxis("Vertical") < -0.1f){
            if (animState != animationState.Down)
            {
                _anim.SetTrigger("Down");
                animState = animationState.Down;
            }
        }
        else {
            if (animState != animationState.Idle)
            {
                _anim.SetTrigger("Idle");
                animState = animationState.Idle;
            }
        }
    }

    void Die(){
        print("Dead");
    }
}
