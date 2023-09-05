using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace BulletHaunter
{
    public class RebindInput
    {
        private PlayerInput playerInput;

        public RebindInput(PlayerInput playerInput) => this.playerInput = playerInput;
       

        public void RebindBinding(string inputActionId, int bindingIndex, Action afterBindAction, Action<string> duplicateBindAaction)
        {
            InputAction inputAction = playerInput.FindAction(inputActionId);
            playerInput.Disable();

            inputAction.PerformInteractiveRebinding(bindingIndex)
                .WithControlsExcluding("Mouse")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnMatchWaitForAnother(0.1f)
                .OnCancel(callback =>
                {
                    callback.Dispose();
                    playerInput.Enable();
                    PlayerPrefs.SetString(GameInput.PLAYER_PREFS_BINDING, playerInput.SaveBindingOverridesAsJson());
                    afterBindAction();
                })
                .OnComplete(callback =>
                {
                    playerInput.Enable();

                    if (CheckDuplicateBindings(inputAction, bindingIndex))
                    {
                        inputAction.RemoveBindingOverride(bindingIndex);                     
                        callback.Dispose();
                        duplicateBindAaction("KEY IS USED");
                        RebindBinding(inputActionId, bindingIndex, afterBindAction, duplicateBindAaction);
                        return;
                    }

                    callback.Dispose();
                    afterBindAction();

                    PlayerPrefs.SetString(GameInput.PLAYER_PREFS_BINDING, playerInput.SaveBindingOverridesAsJson());
                    PlayerPrefs.Save();

                })
               .Start();
        }


        private bool CheckDuplicateBindings(InputAction inputAction, int bindingIndex)
        {
            InputBinding newInputBinding = inputAction.bindings[bindingIndex];
            foreach (InputBinding inputBinding in playerInput.bindings)
            {
                if (inputBinding.id == newInputBinding.id)
                    continue;

                if (inputBinding.effectivePath == newInputBinding.effectivePath)
                    return true;
            }

            return false;
        }

        public string GetBindingText(string inputActionId, int bindingIndex) => 
            playerInput.FindAction(inputActionId).bindings[bindingIndex].ToDisplayString();

        
     
    }
}
