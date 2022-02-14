using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] m_characters;

    public Text m_characterNameText;

    public Dictionary<string, Image> m_charactersImage = new Dictionary<string, Image>();

    [SerializeField] float m_fadeSpeed;



    void Start()
    {
        foreach (var character in m_characters)
        {
            m_charactersImage.Add(character.name, character.GetComponent<Image>());
        }
    }
    /// <summary>
    /// キャラクターのフェードイン
    /// </summary>
    ///<param name="charactor">フェードさせるキャラクター</param>
    public async UniTask CharacterFadeIn(string charactor, CancellationToken cancellationToken)
    {
        var image = m_charactersImage[charactor];
        Color color = image.color;
        while (image.color.a < 1)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                image.color = image.color = new Color(color.r, color.g, color.g, 1);
                return;
            }
            else
            {
                image.color = new Color(color.r, color.g, color.g, color.a);
                color.a += m_fadeSpeed;
            }
          await UniTask.Delay((int)m_fadeSpeed * 1000,false, PlayerLoopTiming.Update, cancellationToken);
        }
    }

    /// <summary>
    /// キャラクターのフェードアウト
    /// </summary>
   ///<param name="charactor">フェードさせるキャラクター</param>
    public  async UniTask CharacterFadeOut(string charactor, CancellationToken cancellationToken)
    {
        var image = m_charactersImage[charactor];
        Color color = image.color;
        while (image.color.a > 0)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                image.color = new Color(color.r, color.g, color.g, 0);
            }
            else
            {
                image.color = new Color(color.r, color.g, color.g, color.a);
                color.a -= m_fadeSpeed;
            }
           

            await UniTask.Delay((int)m_fadeSpeed * 1000, false, PlayerLoopTiming.Update, cancellationToken);
        }
    }


    public  void CharacterEmphasis()
    {
        foreach (var emphasis in m_charactersImage)
        {
            if (emphasis.Key != m_characterNameText.text)
            {
                var color = emphasis.Value.color;
                emphasis.Value.color = new Color(color.r, color.g, color.g, 0.5f);
            }
            else
            {
                var color = emphasis.Value.color;
                emphasis.Value.color = new Color(color.r, color.g, color.g, 1);
            }
        }
    }
}
