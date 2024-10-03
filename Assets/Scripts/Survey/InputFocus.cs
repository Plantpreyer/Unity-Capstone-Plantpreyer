using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputFocus : MonoBehaviour
{
    // Cache the last value of the 'isFocused' field
    private bool _lastFocused;

    // Lost focus event. 
    // public UnityEvent onLostFocus;

    // Got focus event.
    // public UnityEvent onFocus;

    public TMP_InputField field;

    void Awake()
    {
        field = gameObject.GetComponent<TMP_InputField>();
        // Call the base class to setup the input field / selectable classes
        // base.Awake();

        // Cache the current version of 'isFocused'
        _lastFocused = field.isFocused;
    }    

    void Update()
    {
        bool isFocused = field.isFocused;
        // Check to see if the current focus has shifted from the control. If it 
        // has, call the appropriate event's subscribers
        if (_lastFocused != isFocused)
        {
            _lastFocused = isFocused;

            if (isFocused)
            {
                // This will call all subscribers to inform them ctrl has focus
                // onFocus.Invoke();
                PlayerManager.Instance.playerBusy = true;
            }
            else
            {
                // This will call all subscribers to inform them ctrl has lost focus
                // onLostFocus.Invoke();
                PlayerManager.Instance.playerBusy = false;
            }
        }
    }
}