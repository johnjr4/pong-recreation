using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIArrow : MonoBehaviour, ISelectHandler
{
    public GameObject arrow;
    private RectTransform buttonRect;
    public AudioSource audioSource;
    [SerializeField]
    private float xOffset, yOffset;
    [SerializeField]
    private AudioClip selectSound, confirmSound;
    // private TextMeshProUGUI buttonText;

    void Awake()
    {
        buttonRect = GetComponent<RectTransform>();
        // buttonText = GetComponentInChildren<TextMeshProUGUI>();

    }
    public void OnSelect(BaseEventData eventData)
    {
        // Debug.Log("Position: " + transform.position + "   Scale: " + buttonRect.sizeDelta.x);
        arrow.transform.position = new Vector2(transform.position.x - (buttonRect.sizeDelta.x / 2 + xOffset), transform.position.y + yOffset);
        audioSource.PlayOneShot(selectSound);
    }

    public void ConfirmSound()
    {
        // Todo: This should be refactored and handled by a different script
        audioSource.PlayOneShot(confirmSound);
    }

}
