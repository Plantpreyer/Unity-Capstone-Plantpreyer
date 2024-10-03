using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SolvedGames : Singleton<SolvedGames>
{
    public bool hideAndSeek = false;
    public bool[] aWayOut = new[] { false, false };

    private bool experimental;
    private bool hasRun = false;

    private bool startTracking = false;

    [SerializeField] private int numInteractions = 0;

    [SerializeField] private int numInteractionsInForest = 0;

    [SerializeField] private int numInteractionsInForest3 = 0;

    protected override void Awake(){
        base.Awake();
        if (!hasRun)
        {
            experimental = UnityEngine.Random.value < 0.5f;
            // experimental = true;
            Debug.Log("GROUP IS " + (experimental ? "Experimental" : "Control"));
            hasRun = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log(startTracking);
        if (!experimental)
        {
            hideAndSeek = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("Forest 2"))
        {
            startTracking = true;
        }
    }

    public void IncrementInteractionCount()
    {
        if (startTracking)
        {
            numInteractions++;
        }
    }

    public void IncrementInteractionCountInForest()
    {
        if (startTracking)
        {
            numInteractionsInForest++;
        }
        if(SceneManager.GetActiveScene().name.Equals("Forest 3")){
            numInteractionsInForest3++;
        }
    }

    public bool IsExperimental()
    {
        return experimental;
    }

    public int GetTotal()
    {
        return numInteractions;
    }

    public int GetForest()
    {
        return numInteractionsInForest;
    }

    public int GetForest3(){
        return numInteractionsInForest3;
    }
}
