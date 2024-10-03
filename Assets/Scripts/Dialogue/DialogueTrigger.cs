using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : Singleton<DialogueTrigger>
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
    [SerializeField] private bool hiddenAlert;
    [SerializeField] private bool radiusTrigger;

    protected override void Awake()
    {
        indicator = transform.Find("Alert").gameObject;
        ToggleIndicator(!radiusTrigger);
        hasTalked = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!radiusTrigger)
        {
            return;
        }
        if (col.CompareTag("Player"))
        {
            // Debug.Log("player detected");
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!radiusTrigger)
        {
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
            }
            else
            {
                CheckIncrement();
                Debug.Log("Started Dialogue");
                dialogueScript.dialogues = dialogues;
                if (SolvedGames.Instance.GetForest() > 13)
                {
                    if (SceneManager.GetActiveScene().name.Equals("Forest 3") && SolvedGames.Instance.GetForest3() > 8)
                    {
                        dialogueScript.dialogues = new() {
                            {"Try starting at the square with one dot, the going to the square with two dots, and so on."},
                            {"Remember the directions that you walked in to get to each square, then use the same direction exits to walk out of this room in that order."}
                        };
                    }
                }

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

    private void CheckIncrement()
    {
        SolvedGames.Instance.IncrementInteractionCount();
        if (SceneManager.GetActiveScene().name.Equals("Forest 2") || SceneManager.GetActiveScene().name.Equals("Forest 3"))
        {
            SolvedGames.Instance.IncrementInteractionCountInForest();
        }
    }

}
