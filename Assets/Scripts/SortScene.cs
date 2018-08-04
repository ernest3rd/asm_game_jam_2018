using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SortScene : MonoBehaviour {

    private SortingGroup[] s;

	// Use this for initialization
	void Start () {
        s = FindObjectsOfType<SortingGroup>();
	}
	
	// Update is called once per frame
	void Update () {
        foreach(SortingGroup sg in s){
            sg.sortingOrder = (int)(sg.transform.position.y * -50f);
        }
	}
}
