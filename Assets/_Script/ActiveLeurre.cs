using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveLeurre : MonoBehaviour {
    bool _colliding;
    Leurre ls;

    private void Start()
    {
        ls = transform.parent.GetComponent<Leurre>();
    }

    void FixedUpdate()
    {
        if (!ls.active && Input.GetButtonDown("Action") && _colliding) {
            ls.Activation();
        }
        _colliding = false;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            _colliding = true;
        }
    }
}
