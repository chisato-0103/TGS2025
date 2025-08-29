using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace StoryScript
{
    public class StoryImageManager : MonoBehaviour
    {
        [Header("UI References")]
        public Image currentImage;
        public Image nextImage;
        
        [Header("Story Settings")]
        public TransitionSettings transitionSettings = new TransitionSettings();
        
        [Header("Story Images")]
        public List<Sprite> storySprites = new List<Sprite>();
        
        [Header("Scene Transition")]
        public string nextSceneName = "Load";
        
        private int currentIndex = 0;
        private bool isTransitioning = false;
        private InputAction rightArrowAction;
        private InputAction leftArrowAction;
        private InputAction escapeAction;
        
        private void Awake()
        {
            SetupInputActions();
        }
        
        private void Start()
        {
            if (storySprites.Count > 0 && currentImage != null)
            {
                currentImage.sprite = storySprites[0];
                currentImage.gameObject.SetActive(true);
                
                if (nextImage != null)
                {
                    nextImage.gameObject.SetActive(false);
                }
            }
        }
        
        
        private void SetupInputActions()
        {
            var inputActions = new InputActionMap("Story");
            
            rightArrowAction = inputActions.AddAction("NextImage", InputActionType.Button);
            rightArrowAction.AddBinding("<Keyboard>/rightArrow");
            rightArrowAction.performed += OnNextImage;
            
            leftArrowAction = inputActions.AddAction("PreviousImage", InputActionType.Button);
            leftArrowAction.AddBinding("<Keyboard>/leftArrow");
            leftArrowAction.performed += OnPreviousImage;
            
            escapeAction = inputActions.AddAction("Skip", InputActionType.Button);
            escapeAction.AddBinding("<Keyboard>/escape");
            escapeAction.performed += OnSkip;
            
            inputActions.Enable();
        }
        
        private void OnNextImage(InputAction.CallbackContext context)
        {
            if (isTransitioning) return;
            
            if (currentIndex < storySprites.Count - 1)
            {
                StartCoroutine(TransitionToImage(currentIndex + 1, true));
            }
            else
            {
                StartCoroutine(GoToNextScene());
            }
        }
        
        private void OnPreviousImage(InputAction.CallbackContext context)
        {
            if (isTransitioning) return;
            
            if (currentIndex > 0)
            {
                StartCoroutine(TransitionToImage(currentIndex - 1, false));
            }
        }
        
        private void OnSkip(InputAction.CallbackContext context)
        {
            if (!isTransitioning)
            {
                StartCoroutine(GoToNextScene());
            }
        }
        
        private IEnumerator TransitionToImage(int targetIndex, bool goingForward)
        {
            if (targetIndex < 0 || targetIndex >= storySprites.Count) yield break;
            
            isTransitioning = true;
            
            if (nextImage != null)
            {
                nextImage.sprite = storySprites[targetIndex];
            }
            
            switch (transitionSettings.effectType)
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
            if (nextImage != null)
            {
                nextImage.gameObject.SetActive(true);
                yield return StartCoroutine(FadeEffect.CrossFade(currentImage, nextImage, transitionSettings.duration));
                
                SwapImages();
                nextImage.gameObject.SetActive(false);
            }
        }
        
        
        private void ApplyInstantTransition()
        {
            if (nextImage != null)
            {
                currentImage.sprite = nextImage.sprite;
            }
        }
        
        private void SwapImages()
        {
            if (nextImage != null)
            {
                // スプライトを入れ替える
                Sprite temp = currentImage.sprite;
                currentImage.sprite = nextImage.sprite;
                nextImage.sprite = temp;
                
                // currentImageを完全に表示状態にする
                Color currentColor = currentImage.color;
                currentColor.a = 1f;
                currentImage.color = currentColor;
                
                // nextImageを非表示状態にする（次回の準備）
                Color nextColor = nextImage.color;
                nextColor.a = 0f;
                nextImage.color = nextColor;
            }
        }
        
        private IEnumerator GoToNextScene()
        {
            isTransitioning = true;
            
            if (transitionSettings.effectType == TransitionEffectType.Fade)
            {
                yield return StartCoroutine(FadeEffect.FadeOut(currentImage, transitionSettings.duration));
            }
            
            SceneManager.LoadScene(nextSceneName);
        }
        
        private void OnDestroy()
        {
            rightArrowAction?.Disable();
            leftArrowAction?.Disable();
            escapeAction?.Disable();
        }
        
        private void OnEnable()
        {
            rightArrowAction?.Enable();
            leftArrowAction?.Enable();
            escapeAction?.Enable();
        }
        
        private void OnDisable()
        {
            rightArrowAction?.Disable();
            leftArrowAction?.Disable();
            escapeAction?.Disable();
        }
    }
}