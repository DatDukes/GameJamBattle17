using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum animationState {Idle, Left, Right, Up, Down};

public class PlayerScript : MonoBehaviour {
    Rigidbody2D _rb;
    Animator _anim;
    Light2D _light;
    animationState animState;
    Camera _cam;
    SpriteRenderer sp;
    float _speed;
    float _remainingLight;
    bool _running;
    internal bool freeze;
    CircleCollider2D collider;

    //Variable internal
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
    [SerializeField]
    private float _cameraOffset;

    //feedback var
    bool dmgPlaying;

    void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _cam = GetComponentInChildren<Camera>();
        _light = GetComponentInChildren<Light2D>();
        collider = _light.GetComponent<CircleCollider2D>();
        _anim = GetComponentInChildren<Animator>();
        sp = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _remainingLight = _maxLight;
        animState = animationState.Idle;
    }
	
	void Update () {
        if (!freeze){
            MovementUpdate();
            LightUpdate();
            AnimationManager();
            CameraManagement();
        }
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
        }
        else if (_remainingLight < _maxLight) {
            _remainingLight += Time.deltaTime * _lightIncreaseSpeed;
        }

        if (_remainingLight <= 0)
        {
            Die();
        }

        _light.range = _remainingLight;
        collider.radius = _remainingLight;
    }

    void CameraManagement() {
        Vector3 pos = new Vector3(Input.GetAxis("CamX"), Input.GetAxis("CamY"),0);
        _cam.transform.localPosition = Vector3.Lerp(_cam.transform.localPosition, pos * _cameraOffset + Vector3.back * 10, Time.deltaTime * 5);
    }

    void AnimationManager() {
		
		transform.localScale = Vector3.one;

        if (Input.GetAxis("Horizontal") > 0.3f){
            if (animState != animationState.Right) {
                _anim.SetTrigger("Right");
                animState = animationState.Right;
            }
        }
        else if (Input.GetAxis("Horizontal") < -0.3f){
            if (animState != animationState.Left)
            {
                _anim.SetTrigger("Left");
				transform.localScale = new Vector3 (-1f, 1f, 1f);
                animState = animationState.Left;
            }
        }
        else if (Input.GetAxis("Vertical") > 0.3f){
            if (animState != animationState.Up)
            {
                _anim.SetTrigger("Up");
                animState = animationState.Up;
            }
        }
        else if (Input.GetAxis("Vertical") < -0.3f){
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

    public void loseLife() {
        _remainingLight -= Time.deltaTime * _lightDecreaseSpeed * 3;
        domageFeedback();
    }

    void domageFeedback() {
        if (!dmgPlaying) {
            StartCoroutine(LoseLifeFeedback());
        }
    }

    IEnumerator LoseLifeFeedback()
    {
        dmgPlaying = true;
        sp.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        sp.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        dmgPlaying = false;
    }

    void Die(){
        freeze = true;
        StartCoroutine(Reset());
    }

    IEnumerator Reset() {
        yield return new WaitForSeconds (2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
