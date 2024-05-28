using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public static OptionUI Instance { get; private set; }

    [SerializeField] private Transform pressToRebindKayTransform;

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;

    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;

    private void Awake()
    {
        Instance = this;
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(InputSystem.Binding.Move_Up);
        });

        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(InputSystem.Binding.Move_Down);
        });

        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(InputSystem.Binding.Move_Left);
        });

        moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(InputSystem.Binding.Move_Right);
        });

        interactButton.onClick.AddListener(() =>
        {
            RebindBinding(InputSystem.Binding.Interact);
        });

        interactAltButton.onClick.AddListener(() =>
        {
            RebindBinding(InputSystem.Binding.InteractAlternate);
        });

        pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(InputSystem.Binding.Pause);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGameUnPause += GameManager_OnGameUnPause;

        UpdateVisual();
        HidePressToRebindKay();
        Hide();

    }

    private void GameManager_OnGameUnPause(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects:"+ Mathf.Round(SoundManager.instance.GetVolume() * 10f);
        musicText.text = "Music Effects:" + Mathf.Round(MusicManager.instance.GetVolume() * 10f);

        moveUpText.text = InputSystem.instance.GetBindingText(InputSystem.Binding.Move_Up);
        moveDownText.text=InputSystem.instance.GetBindingText (InputSystem.Binding.Move_Down);
        moveLeftText.text = InputSystem.instance.GetBindingText((InputSystem.Binding.Move_Left));
        moveRightText.text = InputSystem.instance.GetBindingText(InputSystem.Binding.Move_Right);
        interactText.text = InputSystem.instance.GetBindingText(InputSystem.Binding.Interact);
        interactAltText.text = InputSystem.instance.GetBindingText(InputSystem.Binding.InteractAlternate);
        pauseText.text = InputSystem.instance.GetBindingText(InputSystem.Binding.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKay()
    {
        pressToRebindKayTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKay()
    {
        pressToRebindKayTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(InputSystem.Binding binding)
    {
        ShowPressToRebindKay();
        InputSystem.instance.RebindDinding(binding,()=> {
            HidePressToRebindKay();
            UpdateVisual();
            });

    }
}
