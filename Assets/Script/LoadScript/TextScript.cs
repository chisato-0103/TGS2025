using UnityEngine;
using UnityEngine.UI; 

public class TextScript : MonoBehaviour
{
    int rnd = 0;
    public Text TriviaText;
    string[] Trivia = { "実はゴリラは案外繊細な生き物なんだぞ!そのため優しく接してあげよう！", "ゴリラは求愛の際にうんちをメスに投げることがあるんだぞ！(諸説アリ)", "ゴリラの胸を叩く行為は威嚇のために使うこともあるが求愛のために使うこともあるんだぞ！" };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rnd = Random.Range(0, 3);
        TriviaText.text = Trivia[rnd].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
