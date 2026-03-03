using System;
using UnityEngine;
using UnityEngine.UI;

public class PaddleButtonUI : MonoBehaviour
{
    public Image icon;
    public GameObject lockOverlay;
    public GameObject selectedFrame;
    public Button button;

    public void Setup(PaddleDefinition def, bool unlocked, bool isSelected, Action onClick)
    {
        icon.sprite = def.icon;
        lockOverlay.SetActive(!unlocked);
        selectedFrame.SetActive(isSelected);

        button.interactable = unlocked;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke());
    }
}

