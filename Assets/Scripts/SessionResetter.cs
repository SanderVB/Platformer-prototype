using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionResetter : MonoBehaviour {

	void Awake () {

        Destroy(GameObject.Find("GameSession"));
		
	}
	
}
