using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringSpriteRenderer : MonoBehaviour {
    public float radius = 1f;
    public float speed = 1f;
    public bool enableX = true;
    public bool enableY = true;
    public bool direction = true;

    private Vector3 originalOffset;

	// Use this for initialization
	void Start () {
        originalOffset = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 offset = Vector3.zero;
        if(enableX){
            offset.x = Mathf.Sin(Time.time * speed * (direction ? 1 : -1)) * radius;
        }
        if(enableY){
            offset.y = Mathf.Cos(Time.time * speed * (direction ? 1 : -1)) * radius;
        }

        transform.localPosition = originalOffset + offset;
	}
}
