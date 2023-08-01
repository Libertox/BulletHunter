using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter
{
    public class MessagePanelUI:MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button continueButton;



        private void Awake()
        {
            continueButton.onClick.AddListener(() => Hide());
        }

        private void Start()
        {



            Hide();
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

        private void SetMessageText(string message) => messageText.SetText(message);

    }
}
