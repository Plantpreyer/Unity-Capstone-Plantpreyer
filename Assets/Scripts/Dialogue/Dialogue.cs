using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Rendering;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class Dialogue : Singleton<Dialogue>
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
    public float writingSpeed = 0.01f;
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

    private bool fast;

    protected override void Awake()
    {
        // ToggleIndicator(false);
        ToggleWindow(false);
        // window = this.gameObject.transform.Find("Dialogue").gameObject;
        temp = writingSpeed;
    }

    public bool isWaiting()
    {
        return waitForNext;
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

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        if (started)
        {
            // Debug.Log("Cannot start, already talking");
            return;
        }

        //Set player to busy
        PlayerManager.Instance.playerBusy = true;
        // playerController.ToggleBusy(true);
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
        // playerController.ToggleBusy(true);
        PlayerManager.Instance.playerBusy = true;
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
        // playerController.ToggleBusy(false);
        PlayerManager.Instance.playerBusy = false;
        dialogues = new List<string>();
        GameObject door;
        if (door = GameObject.Find("Door"))
        {
            door.GetComponent<Door>().LoadSurvey();
        }
    }
    //Writing logic
    IEnumerator Writing()
    {
        yield return new WaitForSeconds(writingSpeed);

        string currDialogue = dialogues[index];

        if (fast)
        {
            for (int i = charIndex; i < currDialogue.Length; i++)
            {
                dialogueText.text += currDialogue[i];
                charIndex++;
            }
            fast = false;
        }
        else
        {

            //Write the character
            dialogueText.text += currDialogue[charIndex];
            //charIndex++
            charIndex++;
        }

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (waitForNext)
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
            } else {
                SetFast();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndDialogue();
        }
    }

    public void SetFast()
    {
        fast = true;
    }
}
