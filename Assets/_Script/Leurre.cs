using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leurre : MonoBehaviour {
    Light2D _light;
    CircleCollider2D _col;

    public float Radius;
    public float timeToRecall;

    internal bool active;

	void Start () {
        _light = GetComponentInChildren<Light2D>();
        _col = _light.GetComponent<CircleCollider2D>();
        _light.gameObject.SetActive(false);
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
        StartCoroutine(ShutDown());
    }

    IEnumerator ShutDown()
    {
        while (_light.range > 0)
        {
            _light.range -= Time.deltaTime / timeToRecall;
            _col.radius -= Time.deltaTime / timeToRecall;
            yield return 0;
        }

        active = false;
        _light.gameObject.SetActive(false);
    }
}
