using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Shooter
{
    public class RebindButtonUI:MonoBehaviour
    {
        [SerializeField] private Button rebindButton;
        [SerializeField] private TextMeshProUGUI keyRebindText;

        [SerializeField] private InputActionReference inputActions;
        [SerializeField] private int bindingsIndex;

       
        private void Awake()
        {
            rebindButton.onClick.AddListener(() =>
            {
                GameInput.Instance.RebindBinding(inputActions.action.id.ToString(), bindingsIndex);
                keyRebindText.SetText(GameInput.Instance.GetBindingText(inputActions.action.id.ToString(), bindingsIndex));
            });
        }

        private void Start()
        {
            keyRebindText.SetText(GameInput.Instance.GetBindingText(inputActions.action.id.ToString(), bindingsIndex));
        }

    }
}
