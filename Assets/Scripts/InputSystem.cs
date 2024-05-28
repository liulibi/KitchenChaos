using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    public static InputSystem instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPuaseAction;

    public const string PLAYER_PREFS_BINDINGS = "InputBindings";

    private Vector2 inputVector2;
    private PlayerAction playerAction;

    public enum Binding 
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
    }


    private void Awake()
    {
        instance = this;
        playerAction=new PlayerAction();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerAction.PlayerMovement.Enable();


        playerAction.PlayerMovement.Interact.performed += Interact_performed;
        playerAction.PlayerMovement.InteractAlternate.performed += InteractAlternate_performed;
        playerAction.PlayerMovement.Puase.performed += Puase_performed;
    }

    private void OnDestroy()
    {
        playerAction.PlayerMovement.Interact.performed -= Interact_performed;
        playerAction.PlayerMovement.InteractAlternate.performed -= InteractAlternate_performed;
        playerAction.PlayerMovement.Puase.performed -= Puase_performed;

        playerAction.Dispose();
    }

    private void Puase_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPuaseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
         OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMoveMentVector2Normalize()
    {
        inputVector2 = playerAction.PlayerMovement.Move.ReadValue<Vector2>();

        inputVector2.Normalize();
        return inputVector2;
    }

    public string GetBindingText(Binding binding)
    {
        switch(binding)
        {
            case Binding.Move_Up:
                return playerAction.PlayerMovement.Move.bindings[1].ToDisplayString();

            case Binding.Move_Down:
                return playerAction.PlayerMovement.Move.bindings[2].ToDisplayString();

            case Binding.Move_Left:
                return playerAction.PlayerMovement.Move.bindings[3].ToDisplayString();

            case Binding.Move_Right:
                return playerAction.PlayerMovement.Move.bindings[4].ToDisplayString();
            
            case Binding.Interact:
                return playerAction.PlayerMovement.Interact.bindings[0].ToDisplayString(); 

            case Binding.InteractAlternate:
                return playerAction.PlayerMovement.InteractAlternate.bindings[0].ToDisplayString();

            case Binding.Pause:
                return playerAction.PlayerMovement.Puase.bindings[0].ToDisplayString();

            default:
                return null;
        }
    }

    public void RebindDinding(Binding binding,Action onActionRebound)
    {
        playerAction.PlayerMovement.Disable();
        switch (binding)
        {
            case Binding.Move_Up:
                playerAction.PlayerMovement.Move.PerformInteractiveRebinding(1)
                .OnComplete(callback =>
                 {
                     callback.Dispose();
                     playerAction.PlayerMovement.Enable();
                     onActionRebound();

                     PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS,playerAction.SaveBindingOverridesAsJson());
                     PlayerPrefs.Save();
                 })
                .Start();
                break;

            case Binding.Move_Down:
                playerAction.PlayerMovement.Move.PerformInteractiveRebinding(2)
                .OnComplete(callback =>
                 {
                     callback.Dispose();
                     playerAction.PlayerMovement.Enable();
                     onActionRebound();

                     PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerAction.SaveBindingOverridesAsJson());
                     PlayerPrefs.Save();
                 })
                 .Start();
                break;

            case Binding.Move_Left:
                playerAction.PlayerMovement.Move.PerformInteractiveRebinding(3)
                .OnComplete(callback =>
                 {
                     callback.Dispose();
                     playerAction.PlayerMovement.Enable();
                     onActionRebound();

                     PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerAction.SaveBindingOverridesAsJson());
                     PlayerPrefs.Save();
                 })
                .Start();
                break;

            case Binding.Move_Right:
                playerAction.PlayerMovement.Move.PerformInteractiveRebinding(4)
                .OnComplete(callback =>
                 {
                     callback.Dispose();
                     playerAction.PlayerMovement.Enable();
                     onActionRebound();

                     PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerAction.SaveBindingOverridesAsJson());
                     PlayerPrefs.Save();
                 })
                 .Start();
                break;

            case Binding.Interact:
                playerAction.PlayerMovement.Interact.PerformInteractiveRebinding(0)
               .OnComplete(callback =>
               {
                   callback.Dispose();
                   playerAction.PlayerMovement.Enable();
                   onActionRebound();

                   PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerAction.SaveBindingOverridesAsJson());
                   PlayerPrefs.Save();
               })
               .Start();
                break;

            case Binding.InteractAlternate:
                playerAction.PlayerMovement.InteractAlternate.PerformInteractiveRebinding(0)
                .OnComplete(callback =>
                {
                    callback.Dispose();
                    playerAction.PlayerMovement.Enable();
                    onActionRebound();

                    PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerAction.SaveBindingOverridesAsJson());
                    PlayerPrefs.Save();
                })
                .Start();
                break;

            case Binding.Pause:
                playerAction.PlayerMovement.Puase.PerformInteractiveRebinding(0)
                .OnComplete(callback =>
                {
                    callback.Dispose();
                    playerAction.PlayerMovement.Enable();
                    onActionRebound();

                    PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerAction.SaveBindingOverridesAsJson());
                    PlayerPrefs.Save();
                })
                .Start();
                break;
        }
    }
}
