﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour {

	void Update(){
		if (Input.GetKeyDown (KeyCode.D)) {
			PlayerControl.instance.DrawCard ();
		}
	}

}