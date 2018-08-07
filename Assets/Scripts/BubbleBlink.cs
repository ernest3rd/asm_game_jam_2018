using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBlink : MonoBehaviour {
    private float interval = 1f;
    private float onDuration = 0.5f;
    private bool blink = false;
    private float phase = 0;
    public Color color;
    private Color originalColor;
    private SpriteRenderer sr;

	private void Awake()
	{
        sr = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
        originalColor = sr.color;
	}
	
	// Update is called once per frame
	void Update () {
        if(blink){
            phase += Time.deltaTime;
            if (phase < 0)
            {
                sr.color = color;
            }
            else {
                sr.color = originalColor;
                if(phase > interval){
                    phase = -onDuration;
                }
            }
        }
	}

    public void LevelNeutral(){
        blink = false;
        phase = 0;
        //if(sr)
            sr.color = originalColor;
    }
    public void LevelWaiting(){
        blink = true;
        interval = 3f;
    }
    public void LevelAnxious(){
        blink = true;
        interval = 1f;
    }
    public void LevelUrgent(){
        blink = false;
        phase = 0;
        //if(sr)
            sr.color = color;
    }
}
