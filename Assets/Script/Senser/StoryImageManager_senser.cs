using System.Collections;
using System.Collections.Generic;
using StoryScript;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryImageManager_senser : MonoBehaviour
{
    // M5StickReaderへの参照を保持する変数
    private M5StickReader m5StickReader;
    // StoryImageManagerへの参照を保持する変数
    private StoryScript.StoryImageManager storyImageManager;

    private int currentIndex = 0;
    private bool isTransitioning = false;

    // スキップ判定用
    private float skipTimer = 0f;
    private float skipThreshold = 3f; // 3秒間続いたらスキップ

    [Header("Scene Transition")]
    public string nextSceneName = "GameScene";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // シーン内からM5StickReaderコンポーネントを探してくる
        m5StickReader = M5StickReader.Instance;

        // シーン内から StoryImageManager を探して参照
        storyImageManager = FindFirstObjectByType<StoryScript.StoryImageManager>();
        if (storyImageManager == null)
        {
            Debug.LogError("StoryImageManager がシーンに見つかりません！");
        }


    }

    // Update is called once per frame
    void Update()
    {
        // 一度だけフラグを読む
        bool throwAction = m5StickReader.Consumepushedbutton();
        bool throwedFlag = m5StickReader.getButtonFlag();
        float roll = m5StickReader.getRoll();

        // --- スキップ判定 ---
        bool skipCondition = !throwedFlag && roll <= -40.0;
        if (skipCondition)
        {
            skipTimer += Time.deltaTime;

            if (skipTimer >= skipThreshold)
            {
                Debug.Log("条件が3秒間続いたのでストーリーをスキップします！");
                StartCoroutine(GoToNextScene());
                skipTimer = 0f; // リセット
            }
        }
        else
        {
            skipTimer = 0f; // 条件が途切れたらリセット
        }

        if (m5StickReader.getButtonFlag())
        {
            m5StickReader.setPushedButton(true);
        }

        // --- 通常のページ送り処理 ---
        if (throwAction && !throwedFlag && !isTransitioning)
        {
            m5StickReader.setPushedButton(false);
            if (isTransitioning) return;

            if (currentIndex < storyImageManager.storySprites.Count - 1)
            {
                StartCoroutine(TransitionToImage(currentIndex + 1, true));
            }
            else
            {
                StartCoroutine(GoToNextScene());
            }
            m5StickReader.setThrowedActionFlag(true);
        }
        else
        {
            //m5StickReader.setThrowedActionFlag(false);
            //m5StickReader.SendFlag(false);
        }

        /*
        // 一度だけフラグを読む
        bool throwAction = m5StickReader.ConsumeThrowActionFlag();
        bool throwedFlag = m5StickReader.getThrowedActionFlag();
        float targetY = m5StickReader.getTarget_y();

        // --- スキップ判定 ---
        bool skipCondition = !throwedFlag && targetY > 3;

        if (skipCondition)
        {
            skipTimer += Time.deltaTime;

            if (skipTimer >= skipThreshold)
            {
                Debug.Log("条件が3秒間続いたのでストーリーをスキップします！");
                StartCoroutine(GoToNextScene());
                skipTimer = 0f; // リセット
            }
        }
        else
        {
            skipTimer = 0f; // 条件が途切れたらリセット
        }
        // --- 通常のページ送り処理 ---
        if (throwAction && !throwedFlag && !isTransitioning)
        {
            if (isTransitioning) return;

            if (currentIndex < storyImageManager.storySprites.Count - 1)
            {
                StartCoroutine(TransitionToImage(currentIndex + 1, true));
            }
            else
            {
                StartCoroutine(GoToNextScene());
            }
            m5StickReader.setThrowedActionFlag(true);
        }
        else
        {
            m5StickReader.setThrowedActionFlag(false);
            m5StickReader.SendFlag(false);
        }
        */

    }

    private IEnumerator TransitionToImage(int targetIndex, bool goingForward)
    {
        if (targetIndex < 0 || targetIndex >= storyImageManager.storySprites.Count) yield break;

        isTransitioning = true;

        if (storyImageManager.nextImage != null)
        {
            storyImageManager.nextImage.sprite = storyImageManager.storySprites[targetIndex];
        }

        switch (storyImageManager.transitionSettings.effectType)
        {
            case TransitionEffectType.Fade:
                yield return StartCoroutine(ApplyFadeTransition());
                break;

            case TransitionEffectType.None:
            default:
                ApplyInstantTransition();
                break;
        }

        currentIndex = targetIndex;
        isTransitioning = false;
    }

    private IEnumerator ApplyFadeTransition()
    {
        if (storyImageManager.nextImage != null)
        {
            storyImageManager.nextImage.gameObject.SetActive(true);
            yield return StartCoroutine(FadeEffect.CrossFade(storyImageManager.currentImage, storyImageManager.nextImage, storyImageManager.transitionSettings.duration));

            SwapImages();
            storyImageManager.nextImage.gameObject.SetActive(false);
        }
    }


    private void ApplyInstantTransition()
    {
        if (storyImageManager.nextImage != null)
        {
            storyImageManager.currentImage.sprite = storyImageManager.nextImage.sprite;
        }
    }

    private void SwapImages()
    {
        if (storyImageManager.nextImage != null)
        {
            // スプライトを入れ替える
            Sprite temp = storyImageManager.currentImage.sprite;
            storyImageManager.currentImage.sprite = storyImageManager.nextImage.sprite;
            storyImageManager.nextImage.sprite = temp;

            // currentImageを完全に表示状態にする
            Color currentColor = storyImageManager.currentImage.color;
            currentColor.a = 1f;
            storyImageManager.currentImage.color = currentColor;

            // nextImageを非表示状態にする（次回の準備）
            Color nextColor = storyImageManager.nextImage.color;
            nextColor.a = 0f;
            storyImageManager.nextImage.color = nextColor;
        }
    }

    private IEnumerator GoToNextScene()
    {
        isTransitioning = true;

        if (storyImageManager.transitionSettings.effectType == TransitionEffectType.Fade)
        {
            yield return StartCoroutine(FadeEffect.FadeOut(storyImageManager.currentImage, storyImageManager.transitionSettings.duration));
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
