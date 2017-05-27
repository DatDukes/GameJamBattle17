﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour {
    float _maxTimer;
    bool _colliding;
    bool _active;
    SpriteRenderer renderer;

    [SerializeField]
    private float _timer;
    [SerializeField]
    private Color _activeColor;
    [SerializeField]
    private Color _unactiveColor;

    void Start () {
        _maxTimer = _timer;
        renderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
        renderer.color = _unactiveColor;
    }

    void FixedUpdate() {
        if (!_active) { 
            if (_colliding) {
                if (Input.GetButton("Action")) {
                    _timer -= Time.fixedDeltaTime;
                    renderer.color = Color.Lerp(_activeColor, _unactiveColor, _timer / _maxTimer);
                    if (_timer <= 0) {
                        _active = true;
                    }
                }
                else {
                    _timer = _maxTimer;
                    renderer.color = _unactiveColor;
                }
            }
            else if (_timer == _maxTimer) {
                _timer = _maxTimer;
                renderer.color = _unactiveColor;
            }
            _colliding = false;
        } 
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "Player"){
            _colliding = true;
        }
    }
}