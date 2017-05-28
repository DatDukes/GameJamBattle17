using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode] 
public class NavObastacleUpdater : MonoBehaviour {
    BoxCollider2D col;
    NavMeshObstacle obs;
	
	void Start () {
        col = GetComponent<BoxCollider2D>();
        obs = GetComponentInChildren<NavMeshObstacle>();
    }
	
	void Update () {
        obs.size = new Vector3(col.size.x, col.size.y, col.size.y * transform.localScale.y);
    }
}
