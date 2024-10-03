using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WayOut : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    [SerializeField] private string direction;

    private void Awake()
    {
        sceneToLoad = transform.GetComponentInParent<RoomExits>().GetNextRoom();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (AWayOut.Instance.AddToQueue(direction))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
        }
    }
}