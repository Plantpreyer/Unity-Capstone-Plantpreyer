using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputFocusDropDown : MonoBehaviour
{
    // Cache the last value of the 'isFocused' field
    private bool _lastFocused;

    // Lost focus event. 
    // public UnityEvent onLostFocus;

    // Got focus event.
    // public UnityEvent onFocus;

    public TMP_Dropdown field;

    void Awake()
    {
        // return;
        // field = gameObject.GetComponent<TMP_Dropdown>();
        // // Call the base class to setup the input field / selectable classes
        // // base.Awake();

        // // Cache the current version of 'isFocused'
        // _lastFocused = field.IsExpanded;
    }    

    void Update()
    {
        // return;
        // bool isFocused = field.IsExpanded;
        // // Check to see if the current focus has shifted from the control. If it 
        // // has, call the appropriate event's subscribers
        // if (_lastFocused != isFocused)
        // {
        //     _lastFocused = isFocused;

        //     if (isFocused)
        //     {
        //         // This will call all subscribers to inform them ctrl has focus
        //         // onFocus.Invoke();
        //         PlayerManager.Instance.playerBusy = true;
        //     }
        //     else
        //     {
        //         // This will call all subscribers to inform them ctrl has lost focus
        //         // onLostFocus.Invoke();
        //         PlayerManager.Instance.playerBusy = false;
        //     }
        // }
    }
}