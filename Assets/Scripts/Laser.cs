﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class Laser : MonoBehaviour
{

	public LineRenderer line;
	public GameObject target;
	public float t;
	AudioSource audio;

	// Use this for initialization
	void Start()
	{

		line = gameObject.GetComponent<LineRenderer>();
		line.enabled = false;
		line.useWorldSpace = false;
		line.SetVertexCount(2);
		line.SetColors(Color.cyan, Color.cyan);
		line.SetWidth(0.01f, 0.01f);
		t = 0.0f;
		audio = GameObject.Find ("Managers").GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update()
	{

		if (line.enabled) {
			t += 0.5f * Time.deltaTime;
		}
		RaycastHit hit;

		Controller controller = new Controller ();
		Frame frame = controller.Frame(); // controller is a Controller object
		if(frame.Hands.Count > 0){
			List<Hand> hands = frame.Hands;
			Hand hand = hands [0];
			int extendedFingers = 0;
			for (int f = 0; f < hand.Fingers.Count; f++) {
				Finger digit = hand.Fingers [f];
				if (digit.IsExtended)
					extendedFingers++;
			}

			if (extendedFingers == 5) {
				line.enabled = true;
				Vector3 rayDirection = transform.TransformDirection(Vector3.down);
				if (Physics.Raycast (transform.position, rayDirection, out hit)) {
					if (hit.collider) {
						t = 0f;
						target = hit.transform.gameObject;
						print (target.tag);
//						print (hit.distance);
						line.SetPosition (1, new Vector3 (0,-hit.distance,0 ));

						if (target.tag == "Enemy") {
							print ("hgtrdxtjrgxhjtf");							
							Destroy (target);
							audio.Play ();
						}
					}
				} else {
					t = 0f;
					line.SetPosition (1, new Vector3 (0,-1000000,0));
				}
			} else {
				line.enabled = false;
			}
		}
	}
}