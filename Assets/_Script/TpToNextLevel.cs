using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TpToNextLevel : MonoBehaviour {
    public string nextLevel;
    float timer;

    public SwitchScript[] switchs;

    BoxCollider2D collider;
    GameObject sprite;

    private void Start()
    {
        switchs = FindObjectsOfType<SwitchScript>();
        collider = GetComponent<BoxCollider2D>();
        sprite = transform.Find("Sprite").gameObject;
        collider.enabled = false;
        sprite.SetActive(false);
    }

    private void Update()
    {
        if (!sprite.activeSelf) {
            bool ok = true;
            foreach (SwitchScript s in switchs)
            {
                if (!s._active)
                {
                    ok = false;
                }
            }
            if (ok)
            {
                collider.enabled = true;
                sprite.SetActive(true);
            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            other.GetComponent<PlayerScript>().freeze = true;
            StartCoroutine(SwitchLevel());
        }
    }

    IEnumerator SwitchLevel() {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(nextLevel);
    }
}
