using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BulletHaunter.PlayerStats;

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
                StopCoroutine(nameof(ChangeFillAmountSlowlyDown));
                StartCoroutine(ChangeFillAmountSlowlyUp(newValue));
            }
            else
            {
                StopCoroutine(nameof(ChangeFillAmountSlowlyUp));
                StartCoroutine(ChangeFillAmountSlowlyDown(newValue));
            }
                
        } 

        private IEnumerator ChangeFillAmountSlowlyDown(float newValue)
        {
            while (newValue < barImage.fillAmount)
            {
                barImage.fillAmount -= Time.deltaTime * 2;

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        private IEnumerator ChangeFillAmountSlowlyUp(float newValue)
        {
            while (newValue > barImage.fillAmount)
            {
                barImage.fillAmount += Time.deltaTime * 2;

                yield return new WaitForSeconds(Time.deltaTime);

            }
        }


        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);



    }
}
