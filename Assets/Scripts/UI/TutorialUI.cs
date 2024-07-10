using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyAltText;
    [SerializeField] private TextMeshProUGUI keyPauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadAltText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;

    private void Start()
    {
        InputController.Instance.OnBindingRebind += InputController_OnBindingRebind;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        UpdateVisual();

        Show();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void InputController_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = InputController.Instance.GetBindingText(InputController.Binding.Move_Up);
        keyMoveDownText.text = InputController.Instance.GetBindingText(InputController.Binding.Move_Down);
        keyMoveLeftText.text = InputController.Instance.GetBindingText(InputController.Binding.Move_Left);
        keyMoveRightText.text = InputController.Instance.GetBindingText(InputController.Binding.Move_Right);
        keyInteractText.text = InputController.Instance.GetBindingText(InputController.Binding.Interact);
        keyAltText.text = InputController.Instance.GetBindingText(InputController.Binding.Alternate);
        keyPauseText.text = InputController.Instance.GetBindingText(InputController.Binding.Pause);
        gamepadInteractText.text = InputController.Instance.GetBindingText(InputController.Binding.Gamepad_Interact);
        gamepadAltText.text = InputController.Instance.GetBindingText(InputController.Binding.Gamepad_InteractAlternate);
        gamepadPauseText.text = InputController.Instance.GetBindingText(InputController.Binding.Gamepad_Pause);

    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
