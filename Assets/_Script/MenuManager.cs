using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    bool CreditIsOn;
    bool freeze;
    Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        Cursor.visible =false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Play(string s)
    {
        if(s != "" && !CreditIsOn) {
            SceneManager.LoadScene(s);
        }
    }

    public void Quit () {
        if (!CreditIsOn){
            Application.Quit();
        }
	}

    public void GoToCredit() {
        if (!CreditIsOn) {
            StartCoroutine(ShowCredit());
        }
    }

    private void Update()
    {
        if (CreditIsOn && !freeze) {
            if (Input.anyKeyDown) {
                StartCoroutine(HideCredit());
            }
        }
    }

    IEnumerator HideCredit()
    {
        anim.SetTrigger("Hide");
        freeze = true;
        yield return new WaitForSeconds(1.1f);
        freeze = false;
        CreditIsOn = false;
    }

    IEnumerator ShowCredit() {
        anim.SetTrigger("Show");
        CreditIsOn = true;
        freeze = true;
        yield return new WaitForSeconds(1.1f);
        freeze = false;
    }
}
