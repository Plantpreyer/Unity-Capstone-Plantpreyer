using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideAndSeek : MonoBehaviour
{

    public GameObject[] allChildrenObjects;
    [SerializeField] Tilemap currTilemap;
    [SerializeField] TileBase currentTile; // Tilebase rather than Tile because you can put rule tiles
    private Vector3Int pos = new Vector3Int(28, -16);
    private int startx = 28;
    private int endx = 29;
    private int starty = -16;
    private int endy = -16;

    public List<List<string>> randomDialogue = new List<List<string>>() {
        new List<string>() {"bro is not here"},
        new List<string>() {". . . . . . .", "You don't see him."},
        new List<string>() {"                      . . .", "                      . . .", "He isn't here."},
        new List<string>() {"Not here."},
        new List<string>() {"Nope."},
        new List<string>() {"Where is he?", ". . . Not here."},
        new List<string>() {"There he is!", ". . . Wait, nevermind, that's just a big rock."}
    };

    private List<string> foundMessage = new List<string>()
        {
            "\". . .\"","\"Aw man . . .\"",
            "\"You found me!\"",
            "\"Thanks for playing with me!\"",
            "\"If you want to play again, I'll be standing where I always am!\"",
            "\"The exit to this place? It's in the bottom right corner.\""
        };

    public List<string> storeDialogue;

    public int randomIndex;

    public GameObject prevHidingSpot;


    [SerializeField] private GameObject friend;
    private CapsuleCollider2D friendCollider;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = friend.GetComponent<SpriteRenderer>();
        friendCollider = friend.GetComponent<CapsuleCollider2D>();
    }

    void Start() // Start runs after all Awake() calls, this way we can be sure all children are initialized
    {
        int childCount = gameObject.transform.childCount;

        List<List<string>> tempRandomDialogue = randomDialogue.ConvertAll
        (
            list => list.ConvertAll
            (
                str => string.Copy(str)
            )
        );

        for (int i = 0; i < childCount; i++)
        {

            int r = UnityEngine.Random.Range(0, tempRandomDialogue.Count);
            // Debug.Log("r: " + r + " " + tempRandomDialogue.Count);
            // Debug.Log("i: " + i + " " + childCount);
            gameObject.transform.GetChild(i).gameObject.GetComponent<HidingSpotTrigger>().dialogues = tempRandomDialogue[r];
            tempRandomDialogue.RemoveAt(r);
            if (tempRandomDialogue.Count == 0)
            {
                tempRandomDialogue = randomDialogue.ConvertAll
                (
                    list => list.ConvertAll
                    (
                        str => String.Copy(str)
                    )
                );
            }
            gameObject.transform.GetChild(i).gameObject.GetComponent<HidingSpotTrigger>().enabled = false;
            if (SolvedGames.Instance.IsExperimental() || !gameObject.transform.GetChild(i).gameObject.GetComponent<DialogueTrigger>())
            {
                gameObject.transform.GetChild(i).Find("Alert").gameObject.SetActive(false);
            }
        }
        if (!SolvedGames.Instance.hideAndSeek)
            CloseExit();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerFriendHide()
    {
        // Debug.Log("Friend event triggered!");
        UIFade.Instance.FadeToBlack();
        StartCoroutine(HideRoutine());
        int childCount = gameObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.GetComponent<HidingSpotTrigger>().enabled = true;
            gameObject.transform.GetChild(i).Find("Alert").gameObject.SetActive(true);
            if (gameObject.transform.GetChild(i).gameObject.GetComponent<DialogueTrigger>())
                gameObject.transform.GetChild(i).gameObject.GetComponent<DialogueTrigger>().enabled = false;
        }
        ChooseHidingSpot();
    }

    public void TriggerFriendReturn()
    {
        UIFade.Instance.FadeToBlack();
        StartCoroutine(ReturnRoutine());
        OpenExit();
        Reset();
    }

    public void ChooseHidingSpot()
    {

        allChildrenObjects = new GameObject[gameObject.transform.childCount];
        int childCount = allChildrenObjects.Length;

        for (int i = 0; i < childCount; i++)
        {
            allChildrenObjects[i] = gameObject.transform.GetChild(i).gameObject;
        }

        randomIndex = UnityEngine.Random.Range(0, childCount);

        prevHidingSpot = allChildrenObjects[randomIndex];

        storeDialogue = prevHidingSpot.GetComponent<HidingSpotTrigger>().dialogues;

        prevHidingSpot.GetComponent<HidingSpotTrigger>().dialogues = foundMessage;

        prevHidingSpot.GetComponent<HidingSpotTrigger>().isHidingSpot = true;
    }

    public void Reset()
    {
        prevHidingSpot.GetComponent<HidingSpotTrigger>().isHidingSpot = false;
        prevHidingSpot.GetComponent<HidingSpotTrigger>().dialogues = storeDialogue;
        allChildrenObjects = new GameObject[gameObject.transform.childCount];
        int childCount = gameObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.GetComponent<HidingSpotTrigger>().enabled = false;
            gameObject.transform.GetChild(i).Find("Alert").gameObject.SetActive(false);
            if (gameObject.transform.GetChild(i).gameObject.GetComponent<DialogueTrigger>())
                gameObject.transform.GetChild(i).gameObject.GetComponent<DialogueTrigger>().enabled = true;
        }
        friend.GetComponent<FriendDialogueTrigger>().wait = false;
    }

    private IEnumerator HideRoutine()
    {
        float waitForFade = 1f;
        while (waitForFade >= 0)
        {
            waitForFade -= Time.deltaTime;
            yield return null;
        }

        friendCollider.enabled = false;
        spriteRenderer.enabled = false;
        float wait = 0.5f;
        while (wait >= 0)
        {
            wait -= Time.deltaTime;
            yield return null;
        }
        UIFade.Instance.FadeToClear();
    }

    private IEnumerator ReturnRoutine()
    {
        float waitForFade = 1f;
        while (waitForFade >= 0)
        {
            waitForFade -= Time.deltaTime;
            yield return null;
        }

        friendCollider.enabled = true;
        spriteRenderer.enabled = true;
        float wait = 0.5f;
        while (wait >= 0)
        {
            wait -= Time.deltaTime;
            yield return null;
        }
        UIFade.Instance.FadeToClear();
    }

    public void CloseExit()
    {
        currTilemap.BoxFill(pos, currentTile, startx, starty, endx, endy);
    }

    public void OpenExit()
    {
        currTilemap.BoxFill(pos, null, startx, starty, endx, endy);
    }

    public void ToggleSolved(bool toggle)
    {
        if (!SolvedGames.Instance.hideAndSeek)
        {
            Debug.Log("TOGGLED " + toggle);
            SolvedGames.Instance.hideAndSeek = toggle;
        }
    }
}
