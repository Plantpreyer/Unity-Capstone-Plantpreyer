using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public Dialogue dialogueScript;
    public List<string> dialogues = new()
        {
            "Aaand that's the end of the game.",
            "You finally got back home, and you can go to sleep now.",
            "Zzzz.",
            "But, before that, I'd like to ask you to take a short survey on how you felt about it."
        };
    public bool playerDetected;
    public GameObject indicator;
    public bool hasTalked;
    private Collider2D myCollider;
    public bool wait;
    public float writingSpeed;
    public bool customWritingSpeed;

    private int count;

    void Awake()
    {
        indicator = transform.Find("Alert").gameObject;
        ToggleIndicator(true);
        hasTalked = false;
        count = 0;
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
        if (hasTalked)
            ToggleIndicator(playerDetected && !dialogueScript.talking);
        else
        {
            ToggleIndicator(!dialogueScript.talking);
        }

        if (playerDetected && Input.GetKeyDown(KeyCode.E))
        {
            // if (wait)
            // {
            //     wait = false;
            //     TriggerFriendHide();
            // }
            if (dialogueScript.talking)
            {
                // Debug.Log("ITS TALKING AND I PRESSED E");
                if (dialogueScript.isWaiting())
                {
                    Debug.Log("Count: " + count);
                    count++;

                    if (count == dialogues.Count)
                    {
                        count = 0;
                        Debug.Log("LOAD SURVEY");
                        SceneManager.LoadScene("Survey");
                        SceneManagement.Instance.SetTransitionName("Survey");
                    }
                }
            }
            else if (!wait)
            {
                // Debug.Log("Started Dialogue");
                dialogueScript.dialogues = dialogues;
                if (customWritingSpeed)
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

    public void LoadSurvey()
    {
        // Debug.Log("LOAD SURVEY");
        SceneManager.LoadScene("Survey");
        SceneManagement.Instance.SetTransitionName("Survey");
    }
}