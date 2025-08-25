using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace StoryScript
{
    public class FadeEffect : MonoBehaviour
    {
        public static IEnumerator FadeOut(Image image, float duration)
        {
            float startAlpha = image.color.a;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
                
                Color color = image.color;
                color.a = alpha;
                image.color = color;

                yield return null;
            }

            Color finalColor = image.color;
            finalColor.a = 0f;
            image.color = finalColor;
        }

        public static IEnumerator FadeIn(Image image, float duration)
        {
            float targetAlpha = 1f;
            float elapsed = 0f;

            Color color = image.color;
            color.a = 0f;
            image.color = color;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, targetAlpha, elapsed / duration);
                
                color = image.color;
                color.a = alpha;
                image.color = color;

                yield return null;
            }

            Color finalColor = image.color;
            finalColor.a = targetAlpha;
            image.color = finalColor;
        }

        public static IEnumerator CrossFade(Image currentImage, Image nextImage, float duration)
        {
            nextImage.color = new Color(nextImage.color.r, nextImage.color.g, nextImage.color.b, 0f);
            
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                
                Color currentColor = currentImage.color;
                currentColor.a = Mathf.Lerp(1f, 0f, t);
                currentImage.color = currentColor;
                
                Color nextColor = nextImage.color;
                nextColor.a = Mathf.Lerp(0f, 1f, t);
                nextImage.color = nextColor;
                
                yield return null;
            }
            
            Color finalCurrentColor = currentImage.color;
            finalCurrentColor.a = 0f;
            currentImage.color = finalCurrentColor;
            
            Color finalNextColor = nextImage.color;
            finalNextColor.a = 1f;
            nextImage.color = finalNextColor;
        }
    }
}