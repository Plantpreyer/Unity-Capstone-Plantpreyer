using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AWayOut : Singleton<AWayOut>
{
    private List<int> order = new List<int>();

    [SerializeField] private int level;

    private Dictionary<int, int[]> levelSolution = new();

    private Dictionary<string, int> directionToInt = new Dictionary<string, int>() {
        { "up" , 1 } , { "right" , 2 } , { "down" , 3 } , { "left" , 4 }
    };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Awake()
    {
        base.Awake();
        // Debug.Log("A Way Out: awake");
        if (SolvedGames.Instance.aWayOut[0])
        {
            levelSolution.Add(1, new[] { directionToInt["down"] });
        }
        else
        {
            levelSolution.Add(1, new[] { directionToInt["up"], directionToInt["up"], directionToInt["right"], directionToInt["down"] });
        }

        if (SolvedGames.Instance.aWayOut[1])
        {
            levelSolution.Add(2, new[] { directionToInt["up"] });
        }
        else
        {
            levelSolution.Add(2, new[] { directionToInt["left"], directionToInt["down"], directionToInt["left"], directionToInt["up"] });
        }

        // Debug.Log("level: " + level);
        for (int i = 1; i <= levelSolution.Count; i++)
        {
            int[] sol = levelSolution[i];
            string solution = "[";
            for (int j = 0; j < sol.Length; j++)
            {
                solution += sol[j];
                if (j != sol.Length - 1)
                {
                    solution += ", ";
                }
            }
            solution += "]";
            string message = "[" + i + ", " + solution + "]";
            if (i == level)
                Debug.Log(message);
        }
    }

    public bool AddToQueue(int n)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Forest 2":
                // Debug.Log("Forest 2");
                level = 1;
                break;
            case "Forest 3":
                // Debug.Log("Forest 3");
                level = 2;
                break;
            default:
                break;
        }

        int[] solution = levelSolution[level].Select(a => a).ToArray();
        string solu = "";
        for (int i = 0; i < solution.Length; i++)
        {
            solu += solution[i] + " ";
        }
        // Debug.Log(solu);
        order.Add(n);
        if(order.Count > solution.Length){
            order.RemoveAt(0);
        }
        for (int i = 0; i < order.Count; i++)
        {
            if (order[i] == solution[i])
            {
                if (i + 1 == solution.Length)
                {
                    Debug.Log(ReturnCombo());
                    order.Clear();
                    Debug.Log("You got it!");
                    bool solved;
                    switch (level)
                    {
                        case 1:
                            solved = SolvedGames.Instance.aWayOut[0];
                            if (!solved)
                            {
                                ChangeSolution(1, new[] { directionToInt["down"] });
                                SolvedGames.Instance.aWayOut[0] = true;
                            }
                            break;
                        case 2:
                            solved = SolvedGames.Instance.aWayOut[1];
                            if (!solved)
                            {
                                ChangeSolution(2, new[] { directionToInt["up"] });
                                SolvedGames.Instance.aWayOut[1] = true;
                            }
                            break;
                        default:
                            break;
                    }
                    if (level < 2)
                    {
                        level++;
                        Debug.Log("level: " + level);
                    }
                    return true;
                }
            }
        }
        Debug.Log(ReturnCombo());
        return false;
    }

    public bool AddToQueue(string dir)
    {
        int[] solution = levelSolution[level].Select(a => a).ToArray();
        int n = directionToInt[dir];
        return AddToQueue(n);
    }

    public string ReturnCombo()
    {
        string result = "";
        for (int i = 0; i < order.Count; i++)
        {
            result += order[i];
            if (i != order.Count - 1)
            {
                result += ", ";
            }
        }
        return "Combo: [" + result + "]";
    }

    public void ChangeSolution(int key, int[] solution)
    {
        levelSolution.Remove(key);
        levelSolution.Add(key, solution);
    }

    public void ClearOrder(int[] solution)
    {
        for (int i = 0; i < order.Count; i++)
        {
            if (order[i] != solution[i])
            {
                for (int k = 0; k <= i; k++)
                {
                    order.RemoveAt(k);
                    i--;
                }

            }
        }
    }
}
