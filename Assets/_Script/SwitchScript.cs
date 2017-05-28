using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchScript : MonoBehaviour {
    float _maxTimer;
    bool _colliding;
    internal bool _active;
    SpriteRenderer renderer;

	public UIController objectifs;

    Slider slide;

    [SerializeField]
    private float _timer;
    [SerializeField]
    private Color _activeColor;
    [SerializeField]
    private Color _unactiveColor;

	public Renderer[] maListeDeLumieres;
	public Material newColor;


    void Start () {
        _maxTimer = _timer;
        renderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
        renderer.color = _unactiveColor;
        slide = transform.parent.GetComponentInChildren<Slider>();
    }

    void FixedUpdate() {
        if (!_active) { 
            if (_colliding) {
                if (Input.GetButton("Action")) {
                    _timer -= Time.fixedDeltaTime;
                    slide.value = (_maxTimer - _timer) / _maxTimer;
                    if (_timer <= 0) {
						_active = true;
						objectifs.coloration ();
						foreach (Renderer myRenderer in maListeDeLumieres) {
							myRenderer.material = newColor;
						}
					}
                	}
                else {
                    _timer = _maxTimer;
                    renderer.color = _unactiveColor;
                }
            }
            else if (_timer == _maxTimer) {
                _timer = _maxTimer;
                slide.value = 0;
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

