using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter.UI
{
    public class FadeImageUI:MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        [SerializeField] private bool fadeOnAwake;

        private void Awake() => canvasGroup = GetComponent<CanvasGroup>();

        private void Start()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnGameFinished += GameManager_OnGameFinished;


            if (fadeOnAwake)
                StartCoroutine(FadeFromBlack(() => Hide()));
            else
                Hide();  
        }

        private void Hide() => gameObject.SetActive(false);

        private void Show() => gameObject.SetActive(true);

        private void GameManager_OnGameFinished(object sender, GameManager.OnGameFinishedEventArgs e)
        {
            Show();
            StartCoroutine(FadeToBlack(e.gameFinishAction));
        }

        private IEnumerator FadeToBlack(Action actionAfterFade)
        {
            while(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            actionAfterFade();
        }

        private IEnumerator FadeFromBlack(Action actionAfterFade)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            actionAfterFade();
        }

    }
}
