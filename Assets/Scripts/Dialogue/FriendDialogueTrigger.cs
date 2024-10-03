using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FriendDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueScript;
    public List<string> dialogues;
    public bool playerDetected;
    public GameObject indicator;
    public bool hasTalked;
    private Collider2D myCollider;
    public bool wait;
    public float writingSpeed;
    public bool customWritingSpeed;

    private int count;
    [SerializeField] public HideAndSeek hideAndSeek;

    void Awake()
    {
        indicator = transform.Find("Alert").gameObject;
        ToggleIndicator(true);
        hasTalked = false;
        count = 0;
        if (SolvedGames.Instance.IsExperimental())
            dialogues = new() { "Hey there friend!", "Wanna play a game?", "...          ...          ...", "I'll take that as a yes.", "I love playing HIDE AND SEEK!", "Just so you know I have some pretty good hiding spots.", "Are you ready?", "Alright, here we go!" };
        else
        {
            dialogues = new() { "Hey there friend!", "How ya doing?", "Oh.", "You wanna know how to get out of here?", "The exit's in the bottom right." };
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
                if (dialogueScript.isWaiting())
                {
                    count++;

                    if (count == dialogues.Count)
                    {
                        count = 0;
                        if (SolvedGames.Instance.IsExperimental())
                        {
                            hideAndSeek.TriggerFriendHide();
                        }
                        else
                        {
                            wait = false;
                        }
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
}
