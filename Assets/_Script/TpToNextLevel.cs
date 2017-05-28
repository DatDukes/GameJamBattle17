using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TpToNextLevel : MonoBehaviour {
    public string nextLevel;
    float timer;

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
