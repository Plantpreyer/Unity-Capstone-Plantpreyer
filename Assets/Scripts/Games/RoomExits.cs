using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExits : MonoBehaviour
{
    [SerializeField] private string nextRoom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetNextRoom(){
        return nextRoom;
    }
}
