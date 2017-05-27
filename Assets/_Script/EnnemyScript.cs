using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyScript : MonoBehaviour {
    //Rigidbody2D 
    RaycastHit2D _ligthHit;
    RaycastHit2D _otherHit;

    public LayerMask LightMask;
    public LayerMask OtherMask;

    void Start () {
		
	}
	
	void Update () {
        _ligthHit = (RaycastHit2D)Physics2D.Raycast(transform.position, transform.right, 5f, LightMask);
        if (_ligthHit != null){
            _otherHit = (RaycastHit2D)Physics2D.Raycast(transform.position, transform.right, 5f, OtherMask);
            if (_otherHit != null){
                if (_otherHit.distance < _ligthHit.distance) {
                    print("Spoted");
                }
            }
            else {
                print("Spoted");
            }
        }
	}
}
