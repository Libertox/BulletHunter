using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BulletHaunter
{
    public class RebindButtonUI:MonoBehaviour
    {
        [SerializeField] private Button rebindButton;
        [SerializeField] private TextMeshProUGUI keyRebindText;

        [SerializeField] private InputActionReference inputActions;
        [SerializeField] private int bindingsIndex;

        [SerializeField] private GameObject rebindMessage;
        [SerializeField] private TextMeshProUGUI rebindText;

        private void Awake()
        {
            rebindButton.onClick.AddListener(() =>
            {
                rebindText.SetText("PRESS A KEY TO REBIND");
                ShowRebindMessage();
                SoundManager.Instance.PlayButtonSound();

                GameInput.Instance.RebindBinding(inputActions.action.id.ToString(), bindingsIndex,() =>
                {
                    UpdateBindText();
                    HideRebindMessage();

                },(string message) => rebindText.SetText(message));
            });
        }

        private void Start() => UpdateBindText();
      
        private void UpdateBindText() => keyRebindText.SetText(GameInput.Instance.GetBindingText(inputActions.action.id.ToString(), bindingsIndex));

        private void ShowRebindMessage() => rebindMessage.SetActive(true);

        private void HideRebindMessage() => rebindMessage.SetActive(false);
       
    }
}
