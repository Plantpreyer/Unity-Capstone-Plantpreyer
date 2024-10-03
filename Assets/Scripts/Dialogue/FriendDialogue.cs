using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Rendering;
using Unity.VisualScripting;

public class FriendDialogue : Singleton<FriendDialogue>
{
    //Fields
    //Window
    public GameObject window;
    //Indicator
    public GameObject indicator;
    //Text component
    public TMP_Text dialogueText;
    //Dialogues list
    public List<string> dialogues;
    //Writing speed
    public float writingSpeed;
    //Index on dialogue
    private int index;
    //Character index
    private int charIndex;
    //Started boolean
    private bool started;
    //Wait for next boolean
    private bool waitForNext;
    public PlayerController playerController;
    public bool talking;
    private float temp;

    protected override void Awake()
    {
        // ToggleIndicator(false);
        ToggleWindow(false);
        // window = this.gameObject.transform.Find("Dialogue").gameObject;
        temp = writingSpeed;
    }


    public void ToggleWindow(bool show)
    {
        // Debug.Log(this.gameObject);
        window.gameObject.SetActive(show);
    }

    public void ToggleIndicator(bool show)
    {
        talking = !show;
    }

    //Start Dialogue
    public void StartDialogue()
    {
        if (started)
        {
            // Debug.Log("Cannot start, already talking");
            return;
        }
        // this.indicator = indicator;

        //Set player to busy
        playerController.ToggleBusy(true);
        //Toggle startbool
        started = true;
        //Show the window
        ToggleWindow(true);
        //hide the indicator
        ToggleIndicator(false);
        //Start with the first dialogue
        index = 0;
        if (index < dialogues.Count)
        {
            GetDialogue(index);
        }
        else
        {
            // end this dialogue
            EndDialogue();
        }
    }

    public void StartDialogueWithCustomWritingSpeed(float customWritingSpeed)
    {
        writingSpeed = customWritingSpeed;
        if (started)
        {
            // Debug.Log("Cannot start, already talking");
            return;
        }
        // this.indicator = indicator;

        //Set player to busy
        playerController.ToggleBusy(true);
        //Toggle startbool
        started = true;
        //Show the window
        ToggleWindow(true);
        //hide the indicator
        ToggleIndicator(false);
        //Start with the first dialogue
        index = 0;
        if (index < dialogues.Count)
        {
            GetDialogue(index);
        }
        else
        {
            // end this dialogue
            EndDialogue();
        }
    }

    private void GetDialogue(int i)
    {
        index = i;
        //Reset charIndex
        charIndex = 0;
        //Clear text component
        dialogueText.text = "";
        //start writing
        StartCoroutine(Writing());
    }

    //End Dialogue
    public void EndDialogue()
    {
        // Debug.Log("Ending dialogue");
        writingSpeed = temp;
        ToggleIndicator(true);
        started = false;
        waitForNext = false;
        StopAllCoroutines();
        //Hide the window
        ToggleWindow(false);
        //Set player to not busy
        playerController.ToggleBusy(false);
        dialogues = new List<string>();
    }
    //Writing logic
    IEnumerator Writing()
    {
        yield return new WaitForSeconds(writingSpeed);

        string currDialogue = dialogues[index];
        //Write the character
        dialogueText.text += currDialogue[charIndex];
        //charIndex++
        charIndex++;
        //check if string ended
        if (charIndex < currDialogue.Length)
        {
            //Wait x seconds
            yield return new WaitForSeconds(writingSpeed);
            //Loop
            StartCoroutine(Writing());
        }
        else
        {
            // stop writing, wait
            // Debug.Log("Waiting");
            waitForNext = true;
        }

    }

    private void Update()
    {
        if (!started)
            return;

        if (waitForNext && Input.GetKeyDown(KeyCode.E))
        {
            waitForNext = false;
            index++;
            if (index < dialogues.Count)
            {
                GetDialogue(index);
            }
            else
            {
                // end this dialogue
                EndDialogue();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndDialogue();
        }
    }
}
