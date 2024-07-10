using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button alternateButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadAlternateButton;
    [SerializeField] private Button gamepadPauseButton;

    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

    [SerializeField] private TextMeshProUGUI upButtonText;
    [SerializeField] private TextMeshProUGUI downButtonText;
    [SerializeField] private TextMeshProUGUI leftButtonText;
    [SerializeField] private TextMeshProUGUI rightButtonText;
    [SerializeField] private TextMeshProUGUI interactButtonText;
    [SerializeField] private TextMeshProUGUI alternateButtonText;
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private TextMeshProUGUI gamepadInteractButtonText;
    [SerializeField] private TextMeshProUGUI gamepadAlternateButtonText;
    [SerializeField] private TextMeshProUGUI gamepadPauseButtonText;

    [SerializeField] private Transform pressToRebindTransform;

    private Action onCloseButtonAction;

    private void Awake()
    {
        Instance = this;

        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });

        upButton.onClick.AddListener(() => { RebindBinding(InputController.Binding.Move_Up); });
        downButton.onClick.AddListener(() => { RebindBinding(InputController.Binding.Move_Down); });
        leftButton.onClick.AddListener(() => { RebindBinding(InputController.Binding.Move_Left); });
        rightButton.onClick.AddListener(() => { RebindBinding(InputController.Binding.Move_Right); });
        interactButton.onClick.AddListener(() => { RebindBinding(InputController.Binding.Interact); });
        alternateButton.onClick.AddListener(() => { RebindBinding(InputController.Binding.Alternate); });
        pauseButton.onClick.AddListener(() => { RebindBinding(InputController.Binding.Pause); });

        gamepadInteractButton.onClick.AddListener(() => { RebindBinding(InputController.Binding.Gamepad_Interact); });
        gamepadAlternateButton.onClick.AddListener(() => { RebindBinding(InputController.Binding.Gamepad_InteractAlternate); });
        gamepadPauseButton.onClick.AddListener(() => { RebindBinding(InputController.Binding.Gamepad_Pause); });

    }

    private void Start()
    {
        GameManager.Instance.OnPauseToggle += Instance_OnPauseToggle;

        UpdateVisual();

        Hide();
        HidePressToRebindKey();
    }

    private void Instance_OnPauseToggle(object sender, GameManager.OnPauseToggleEventArgs e)
    {
        if (e.pauseState == false)
        {
            // Not paused
            Hide();
        }
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        upButtonText.text = InputController.Instance.GetBindingText(InputController.Binding.Move_Up);
        downButtonText.text = InputController.Instance.GetBindingText(InputController.Binding.Move_Down);
        leftButtonText.text = InputController.Instance.GetBindingText(InputController.Binding.Move_Left);
        rightButtonText.text = InputController.Instance.GetBindingText(InputController.Binding.Move_Right);
        interactButtonText.text = InputController.Instance.GetBindingText(InputController.Binding.Interact);
        alternateButtonText.text = InputController.Instance.GetBindingText(InputController.Binding.Alternate);
        pauseButtonText.text = InputController.Instance.GetBindingText(InputController.Binding.Pause);

        gamepadInteractButtonText.text = InputController.Instance.GetBindingText(InputController.Binding.Gamepad_Interact);
        gamepadAlternateButtonText.text = InputController.Instance.GetBindingText(InputController.Binding.Gamepad_InteractAlternate);
        gamepadPauseButtonText.text = InputController.Instance.GetBindingText(InputController.Binding.Gamepad_Pause);

    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;

        gameObject.SetActive(true);

        soundEffectsButton.Select();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        pressToRebindTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(InputController.Binding binding)
    {
        ShowPressToRebindKey();
        InputController.Instance.RebindBinding(binding, () => {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
    
}
