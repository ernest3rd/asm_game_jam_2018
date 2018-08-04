using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenderRandomizer : MonoBehaviour {

    public Anima2D.SpriteMesh ladySprite;

	// Use this for initialization
	void Start () {
        if(Random.value > 0.5f){
            GetComponent<Anima2D.SpriteMeshInstance>().spriteMesh = ladySprite;
        }
	}
}
