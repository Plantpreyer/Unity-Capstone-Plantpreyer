using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class HidingSpotTrigger : Singleton<HidingSpotTrigger>
{
    public Dialogue dialogueScript;
    public List<string> dialogues;
    public bool playerDetected;
    public bool playerInRange;
    public GameObject indicator;
    public bool hasTalked;
    private Collider2D myCollider;
    private bool wait;
    public float writingSpeed;
    public bool customWritingSpeed;
    private bool hiddenAlert = true;
    [SerializeField] private bool radiusTrigger;
    private HideAndSeek hideAndSeek;

    public bool isHidingSpot = false;

    protected override void Awake()
    {
        hideAndSeek = gameObject.transform.GetComponentInParent<HideAndSeek>();
        indicator = transform.Find("Alert").gameObject;
        ToggleIndicator(!radiusTrigger);
        hasTalked = false;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(!radiusTrigger) {
            return;
        }
        if(col.CompareTag("Player")){
            // Debug.Log("player detected");
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if(!radiusTrigger) {
            return;
        }
        if (col.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    //Detect trigger with player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If we triggered the player enable playerdetected and show indiactor
        if (collision.collider.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }

    //While detected if we interact start the dialogue
    private void Update()
    {
        if (hasTalked || hiddenAlert)
            ToggleIndicator((playerDetected || (radiusTrigger && playerInRange)) && !dialogueScript.talking);
        else if (!hiddenAlert)
        {
            ToggleIndicator(!dialogueScript.talking);
        }

        if (playerDetected && Input.GetKeyDown(KeyCode.E) && !dialogueScript.talking)
        {
            if (wait)
            {
                wait = false;
                if(isHidingSpot){
                    hideAndSeek.TriggerFriendReturn();
                }
            }
            else
            {
                Debug.Log("Started Dialogue");
                dialogueScript.dialogues = dialogues;
                if(customWritingSpeed)
                    dialogueScript.StartDialogueWithCustomWritingSpeed(writingSpeed);
                else
                    dialogueScript.StartDialogue();
                hasTalked = true;
                wait = true;
            }
        }
    }

    public void ToggleIndicator(bool show)
    {
        indicator.SetActive(show);
    }

}
