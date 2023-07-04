using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
{
    public class BarUI:MonoBehaviour
    {
        [SerializeField] private Image barImage;

        public void ChangeFillAmount(float newValue) => barImage.fillAmount = newValue;
      
        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

    }
}
