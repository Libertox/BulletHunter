using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BulletHaunter.UI
{
    public class BarUI:MonoBehaviour
    {
        [SerializeField] private Image barImage;

        public void ChangeFillAmountImmediately(float newValue) => barImage.fillAmount = newValue;

        public void ChangeFillAmountSlowly(float newValue) 
        {
            if (newValue > barImage.fillAmount)
            {
                StopCoroutine(nameof(ChangeFillAmountDownCoroutine));
                StartCoroutine(ChangeFillAmountUpCoroutine(newValue));
            }
            else
            {
                StopCoroutine(nameof(ChangeFillAmountUpCoroutine));
                StartCoroutine(ChangeFillAmountDownCoroutine(newValue));
            }         
        } 

        private IEnumerator ChangeFillAmountDownCoroutine(float newValue)
        {
            while (newValue < barImage.fillAmount)
            {
                barImage.fillAmount -= Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator ChangeFillAmountUpCoroutine(float newValue)
        {
            while (newValue > barImage.fillAmount)
            {
                barImage.fillAmount += Time.deltaTime;
 
                yield return new WaitForEndOfFrame();
            }
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}
