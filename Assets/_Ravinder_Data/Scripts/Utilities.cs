using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Utilities : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UniversalMessagePopup universalMessagePrefab;

    //Various variables.
    public static Utilities Instance { get; private set; }
    private UniversalMessagePopup universalPopup = default;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ShowMessage(string message, UnityAction buttonAction = null)
    {
        //If message canvas is not available than instantiate a new one.
        if (Instance.universalPopup == null)
        {
            Instance.universalPopup = GameObject.Instantiate(Instance.universalMessagePrefab);
            DontDestroyOnLoad(Instance.universalPopup.gameObject);
        }

        //Setting the message nd action on the button.
        if (!string.IsNullOrEmpty(message))
        {
            Instance.universalPopup.gameObject.SetActive(true);
            Instance.universalPopup.actionButton.onClick.AddListener(() => Instance.universalPopup.gameObject.SetActive(false));
            Instance.universalPopup.messageText.text = message;
            if (buttonAction != null) Instance.universalPopup.actionButton.onClick.AddListener(buttonAction);
        }
        else Debug.LogError("Message cannot be empty or null.");
    }
}