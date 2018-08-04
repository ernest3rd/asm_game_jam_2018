using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {
    public GameObject start;
    private float counter;


	// Use this for initialization
	void Start () {
        counter = 0;
        start.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (counter < 3f)
        {
            counter += Time.deltaTime;
        }
        else
        {
            start.SetActive(true);
            if (Input.GetButtonUp("Fire1"))
            {
                SceneManager.LoadScene("GameScene");
                //enabled = false;
            }
        }
	}
}
