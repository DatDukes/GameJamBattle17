using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leurre : MonoBehaviour {
    Light2D _light;
    CircleCollider2D _col;

    public float Radius;
    public float timeToRecall;

    internal bool active;

    internal GameObject hint;

	void Start () {
        _light = GetComponentInChildren<Light2D>();
        _col = _light.GetComponent<CircleCollider2D>();
        _light.gameObject.SetActive(false);
        hint = transform.Find("Help").gameObject;
        hint.SetActive(false);
    }

    public void Activation(){
        _light.gameObject.SetActive(true);
        //_col.enabled = true;
        _light.range = 0;
        _col.radius = 0;
        StartCoroutine(Lighting());
    }

    IEnumerator Lighting() {
        active = true;
        while (_light.range < Radius) {
            _light.range += Time.deltaTime * 5;
            _col.radius += Time.deltaTime * 5;
            yield return 0;
        }
    }

    public void ShutDown()
    {
        if(_light.range > 0)
        {
            _light.range -= (Time.fixedDeltaTime * Radius) / timeToRecall;
            _col.radius -= (Time.fixedDeltaTime * Radius) / timeToRecall;
        }
        else
        {
            active = false;
            _light.gameObject.SetActive(false);
        }
    }
}
