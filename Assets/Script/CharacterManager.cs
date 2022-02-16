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

    private float m_alfa;

    void Start()
    {
       
    }
    /// <summary>
    /// キャラクターのフェードイン
    /// </summary>
    ///<param name="charactor">フェードさせるキャラクター</param>
    public async UniTask CharacterFadeIn(string charactor, CancellationToken cancellationToken)
    {
        var image = m_charactersImage[charactor];
        Color color = image.color;
        m_alfa = 0;
        image.color = new Color(image.color.r, image.color.g, image.color.g, m_alfa);
        cancellationToken.Register(() => image.color = new Color(image.color.r, image.color.g, image.color.g, 1));
        while (image.color.a < 1)
        {

            image.color = new Color(color.r, color.g, color.g, m_alfa);
            m_alfa += m_fadeSpeed;

            await UniTask.Delay((int)m_fadeSpeed * 1000, false, PlayerLoopTiming.Update, cancellationToken);
        }
    }

    /// <summary>
    /// キャラクターのフェードアウト
    /// </summary>
    ///<param name="charactor">フェードさせるキャラクター</param>
    public async UniTask CharacterFadeOut(string charactor, CancellationToken cancellationToken)
    {
        var image = m_charactersImage[charactor];
        Color color = image.color;
        m_alfa = image.color.a;
        cancellationToken.Register(() => image.color = new Color(image.color.r, image.color.g, image.color.g, 0));

        while (image.color.a > 0)
        {
            image.color = new Color(color.r, color.g, color.g, m_alfa);
            m_alfa -= m_fadeSpeed;

            await UniTask.Delay((int)m_fadeSpeed * 1000, false, PlayerLoopTiming.Update, cancellationToken);
        }
    }


    public void CharacterEmphasis()
    {
        foreach (var emphasis in m_charactersImage)
        {
            if (emphasis.Key != m_characterNameText.text)
            {
                var color = emphasis.Value.color;
                if (emphasis.Value.color.a >= 0.5f)
                {
                    emphasis.Value.color = new Color(color.r, color.g, color.g, 0.5f);
                }

            }
            else
            {
                var color = emphasis.Value.color;
                emphasis.Value.color = new Color(color.r, color.g, color.g, 1);
            }
        }
    }
    public void CharacterImageSetUp()
    {
          foreach (var character in m_characters)
        {
            m_charactersImage.Add(character.name, character.GetComponent<Image>());
        }
    }

       
}

