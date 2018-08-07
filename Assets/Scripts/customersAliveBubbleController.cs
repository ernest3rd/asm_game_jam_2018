using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customersAliveBubbleController : MonoBehaviour {
    private float timer = 0f;
    private bool visible = false;

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if(!(visible) && timer > 2f) {
            visible = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }

        if (timer > 7f) {
            gameObject.SetActive(false);
        }
	}
}
