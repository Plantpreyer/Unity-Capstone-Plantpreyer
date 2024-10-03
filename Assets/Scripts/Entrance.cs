using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    [SerializeField] private string transitionName;
    [SerializeField] private string entranceId;

    void Awake()
    {
        try
        {
            if (transitionName == SceneManagement.Instance.SceneTransitionName)
            {
                switch (entranceId)
                {
                    case ("hide"):
                        GameObject.Find("HideAndSeek").GetComponent<HideAndSeek>().ToggleSolved(false);
                        break;
                    case ("goBackHide"):
                        GameObject.Find("HideAndSeek").GetComponent<HideAndSeek>().ToggleSolved(true);
                        break;
                    default:
                        break;
                }
            }
        } catch {
            ;
        }
    }

    private void Start()
    {
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();

        }
    }
}
