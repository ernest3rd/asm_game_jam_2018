using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunbathersController : MonoBehaviour {
    private const float maxSunBurn = 100f;
    private const float maxSunLotion = 30f;
    private const float maxHydration = 100f;
    private const float sunLotionRelief = -0.2f;
    private const float sunburnRateIncrease = 0.005f;
    private const int maxTip = 100;
    private const int numberOfIdleAnims = 4;
    private const float harrasmentLimit = 1f;

    private bool alive = true;
    private float deadness = 0;
    private float harrasment = 0;

    private bool isInShade = false;
    private float amountOfSunLotion = 0f;
    private float sunburn = 0f;
    private float sunburnRate = 0.2f;

    private float dehydrationRate = 1f;
    private float hydration = 0f;
    private bool thirsty = false;

    public GameObject sunbather;
    public GameObject drinkbubble;
    public GameObject happybubble;
    public GameObject sadbubble;
    public GameObject firebubble;

    private GameObject currentBubble;
    private List<GameObject> activeRequests = new List<GameObject>();
    private float bubbleTimer = 0;
    private bool emotionActive = false;

    private float holdPoseTime = 0f;
    private Animator animator;
    private Anima2D.SpriteMeshInstance[] sr;
    private Color redSkin = new Color(1, 0.4f, 0.4f);
    private Color deadSkin = new Color(0.7f, 0.8f, 1);
    private Color justBeforeDyingSkin = new Color();

    private float tipCounter = 0;

    private void Awake()
    {
        sr = gameObject.GetComponentsInChildren<Anima2D.SpriteMeshInstance>();
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {
        alive = true;
        hydration = Random.Range(maxHydration*0.5f, maxHydration);
        amountOfSunLotion = Random.Range(0, maxSunLotion * 0.5f);
        animator.SetInteger("idleAnim", (int)(Random.value * numberOfIdleAnims));
        animator.SetBool("alive", true);
        holdPoseTime = Random.Range(5f, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            // Apply sunburn
            if (isInShade)
            {

            }
            else if (amountOfSunLotion > 0)
            {
                sunburn += sunLotionRelief * Time.deltaTime;
                amountOfSunLotion -= Time.deltaTime;
            }
            else
            {
                sunburn += sunburnRate * Time.deltaTime;
                sunburnRate += sunburnRateIncrease * Time.deltaTime;
                if (sunburn < 0)
                {
                    sunburn = 0;
                }
                amountOfSunLotion -= Time.deltaTime;
            }

            // Dehydrate
            if (isInShade){
                // dehydration is slower in shade
            }
            else {
                hydration -= Time.deltaTime * dehydrationRate;
            }

            // Order drink when thirst comes
            if (hydration < maxHydration / 2f && !thirsty)
            {
                OrderDrink();
            }

            // Change idle animation
            holdPoseTime -= Time.deltaTime;
            if (holdPoseTime < 0)
            {
                animator.SetInteger("idleAnim", (int)(Random.value * numberOfIdleAnims));
                holdPoseTime = Random.Range(1f, 10f);
            }

            // Die from sunburn
            if (sunburn > maxSunBurn || hydration < 0)
            {
                Die();
            }

            // Color the skin
            for (int i = 0; i < sr.Length; i++)
            {
                sr[i].color = Color.Lerp(Color.white, redSkin, sunburn / maxSunBurn);
            }

            if(thirsty && tipCounter > 0){
                tipCounter -= 5 * Time.deltaTime;
                if(tipCounter<0){
                    tipCounter = 0;
                }
            }

            // Bubble controlling
            if (amountOfSunLotion < 0)
            {
                AddRequest(firebubble);
            }

            // TODO: Make a method out of this
            bubbleTimer -= Time.deltaTime;
            if (bubbleTimer < 0)
            {
                if (emotionActive)
                {
                    HideEmotion();
                }
                else if(activeRequests.Count > 0)
                {
                    ShowNextRequest();
                }
            }

            if(harrasment > 0){
                harrasment -= Time.deltaTime;
            }
        }
        else {
            if (deadness < 1f)
            {
                deadness += 0.4f * Time.deltaTime;
                for (int i = 0; i < sr.Length; i++)
                {
                    sr[i].color = Color.Lerp(justBeforeDyingSkin, deadSkin, deadness);
                }
            }
        }
    }

    private void OrderDrink()
    {
        thirsty = true;
        tipCounter = maxTip;
        AddRequest(drinkbubble);
    }

    private void AddRequest(GameObject bubble){
        if(!activeRequests.Contains(bubble)){
            activeRequests.Add(bubble);
        }
    }

    private void RemoveRequest(GameObject bubble){
        if(currentBubble == bubble)
        {
            currentBubble = null;
        }
        bubble.SetActive(false);
        activeRequests.Remove(bubble);
    }

    private void ShowNextRequest(){
        if (!emotionActive)
        {
            if(activeRequests.Count > 1)
            {
                for (int i = 0; i < activeRequests.Count; i++)
                {
                    if (activeRequests[i].activeSelf)
                    {
                        activeRequests[i].SetActive(false);
                        if(i+1 < activeRequests.Count)
                        {
                            currentBubble = activeRequests[i+1];
                        }
                        else
                        {
                            currentBubble = activeRequests[0];
                        }
                        currentBubble.SetActive(true);
                        break;
                    }
                }

            }
            else if(activeRequests.Count == 1){
                currentBubble = activeRequests[0];
                currentBubble.SetActive(true);
            }
            bubbleTimer = 2f;
        }
    }
    private void HideRequest(){
        foreach(GameObject bubble in activeRequests){
            bubble.SetActive(false);
        }
    }

    private void ShowEmotion(GameObject bubble)
    {
        HideEmotion();
        HideRequest();
        currentBubble = bubble;
        currentBubble.SetActive(true);
        bubbleTimer = 2f;
        emotionActive = true;
    }

    private void HideEmotion() {
        if (currentBubble)
        {
            currentBubble.SetActive(false);
            currentBubble = null;
            emotionActive = false;
        }
    }

    public bool IsThirsty(){
        return thirsty && alive;
    }

    public bool OfferDrink(){
        if(IsThirsty()){
            return true;
        }
        ShowEmotion(sadbubble);
        return false;
    }

    public int AddHydration() {
        if (thirsty && alive) {
            hydration += maxHydration / 2f;
            if(hydration > maxHydration){
                hydration = maxHydration;
            }
            thirsty = false;
            RemoveRequest(drinkbubble);
            ShowEmotion(happybubble);
            return (int)tipCounter;
        }
        return 0;
    }

    public bool AddLotion(float amount) {
        if (alive && activeRequests.Contains(firebubble))
        {
            if(amountOfSunLotion < 0){
                amountOfSunLotion = 0;
            }
            
            amountOfSunLotion += amount;
            if (amountOfSunLotion >= maxSunLotion)
            {
                RemoveRequest(firebubble);
                ShowEmotion(happybubble);
            }
            if (amountOfSunLotion >= maxSunLotion)
            {
                amountOfSunLotion = maxSunLotion;
            }
            return true;
        }
        harrasment += 2 * Time.deltaTime;
        if(harrasment > harrasmentLimit){
            ShowEmotion(sadbubble);
        }
        return false;
    }

    private void Die(){
        deadness = 0;
        HideEmotion();
        HideRequest();
        alive = false;
        animator.SetInteger("idleAnim", -1);
        animator.SetBool("alive", false);
        animator.Play("Sunbather_dead");
        justBeforeDyingSkin = Color.Lerp(Color.white, redSkin, sunburn / maxSunBurn);
    }

    public bool IsAlive(){
        return alive;
    }
}
