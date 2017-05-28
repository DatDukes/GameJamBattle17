using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public Image UI1;
	public Image UI2;
	public Image UI3;
	int nb_objectifs_atteints=0;
	// Use this for initialization

	public void coloration(){
		if (nb_objectifs_atteints == 0) {
			coloration3 ();
			nb_objectifs_atteints = 1;
		}
		else if (nb_objectifs_atteints == 1) {
			coloration2 ();
			nb_objectifs_atteints = 2;
		}
		else if (nb_objectifs_atteints == 2) {
			coloration1 ();
			nb_objectifs_atteints=3;
		}
	}

	void coloration1(){
		UI1.color = Color.red;
	}
	public void coloration2(){
		UI2.color = Color.red;
	}
	public void coloration3(){
		UI3.color = Color.red;
	}
}
