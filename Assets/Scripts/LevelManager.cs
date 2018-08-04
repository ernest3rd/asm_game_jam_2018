using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    private GameObject[] sunbathers;
    private int currentLevel = 0;
    private float levelUpTime = 0;
    public int levelUpTimeSeconds = 30;
    public GameObject UIGameOver;
    public PlayerController player;
    public GameObject pressSpace;

    private float counter = 0;
    private bool gameover = false;


	// Use this for initialization
	void Start () {
        sunbathers = GameObject.FindGameObjectsWithTag("Sunbather");
        foreach(GameObject sb in sunbathers){
            sb.SetActive(false);
        }
        sunbathers[(int)(Random.value * sunbathers.Length)].SetActive(true);
        levelUpTime = 0;
        currentLevel = 0;

        UIGameOver.SetActive(false);
        pressSpace.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
        if(gameover){
            counter += Time.deltaTime;
            if(counter > 3f){
                pressSpace.SetActive(true);
                if(Input.GetButtonDown("Fire1")){
                    SceneManager.LoadScene("Title");
                }
            }
        }
        else {
            foreach (GameObject sb in sunbathers)
            {
                if (!sb.GetComponent<SunbathersController>().isAlive())
                {
                    // Game over
                    UIGameOver.SetActive(true);
                    player.gameObject.GetComponent<Animator>().Play("Idle");
                    player.enabled = false;
                    counter = 0;
                    gameover = true;
                    pressSpace.SetActive(false);
                }
            }

            levelUpTime += Time.deltaTime;
            if (levelUpTime > levelUpTimeSeconds)
            {
                levelUpTime = 0;
                if (sunbathers.Length > currentLevel)
                {
                    currentLevel++;
                    GameObject tmp;
                    do
                    {
                        tmp = sunbathers[(int)(Random.value * sunbathers.Length)];
                    } while (tmp.activeInHierarchy);
                    tmp.SetActive(true);
                }
            }
        }
    }
}
