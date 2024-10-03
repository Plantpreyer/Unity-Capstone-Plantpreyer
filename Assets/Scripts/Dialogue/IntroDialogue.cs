using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroDialogue : MonoBehaviour
{
    public Dialogue dialogueScript;
    public readonly List<string> dialogues = new()
        {
            "Welcome to my game! Press E to see the next text section.",
            "I made this game as a short and hopefully entertaining experience for you guys.",
            "Feel free to explore and interact with objects as much as you like!",
            "Walk up to an object and press E to interact with it.",
            "Some of them have something to say, some of them don't.",
            "If you accidentally enter a dialogue, you can press escape to end it early.",
            "You can tell if you can interact with an object if you're touching it and it has an exclamation mark floating above it.",
            "Some of them have the exclamation mark floating by default, but for a lot of them you need to walk up and touch to see the icon.",
            "Use WASD to move around.",
            "Try walking up to the bush and the tree with the icons, and to the other trees without the icons as well!",
            "There will be a short survey after the game to get your opinions on the experience!",
            "The point of the game is that your character has started in an unknown place and is trying to get home.",
            "Progress from room to room, and sometimes there will be a puzzle or minigame you have to complete."
        };

    public float writingSpeed = 0.01f;
    public bool customWritingSpeed = true;

    void Awake()
    {

    }

    void Start() {
        dialogueScript.dialogues = dialogues;
        if(SceneManager.GetActiveScene().name.Equals("Intro")){
            dialogueScript.StartDialogueWithCustomWritingSpeed(writingSpeed);
        }
    }

    private void Update()
    {
        
    }

}
