using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System.Threading;

public class GameManager : MonoBehaviour
{
    private string[,] m_talkText;//表示するText

    [SerializeField] TextAsset textAsset;

    [SerializeField] private Text m_text;//表示させるTextコンポーネント

    public static float m_waitTime = 1.0f;//次の文字を表示するまでの時間

    private int m_page = 0;//現在のページ

    [SerializeField] BackgroundManager m_BackGroundManager;

    [SerializeField] CharacterManager m_characterManager;

    void Start()
    {
        string textLine = textAsset.text;//text全体いれる

        string[] textMessege = textLine.Split('\n');//一行ずつに分ける

        int columnLength = textMessege[0].Split(' ').Length;
        int rowLength = textMessege.Length;

        m_talkText = new string[rowLength, columnLength];

        for (int i = 0; i < rowLength; i++)
        {
            string[] tempWords = textMessege[i].Split(' ');

            for (int n = 0; n < columnLength; n++)
            {
                m_talkText[i, n] = tempWords[n];
                Debug.Log(m_talkText[i, n]);
            }
        }

        var next = new Next();
        Talk(next).Forget();

    }

    /// <summary>
    /// 一文字ずつ表示させる
    /// </summary>
    /// <param name="text">表示させるText</param>
    /// <returns></returns>
    async UniTask Talk(Next next)
    {
        while (m_talkText.GetLength(0) > m_page)
        {
            m_text.text = "";//Textを初期化

            m_characterManager.m_characterNameText.text = m_talkText[m_page, 1];//キャプションに名前を入れる
                                                                                //    m_characterManager.CharacterEmphasis();//話しているキャラクターを強調させる

            string valueText = m_talkText[m_page, 2];

            var cancellationToken = new CancellationTokenSource();
            Event(cancellationToken.Token).Forget();
            await UniTask.WhenAny(next.IsNextAsync(), NextPage(valueText, cancellationToken.Token));
            
            cancellationToken.Cancel();
            if (next.IsNext)
            {
                m_text.text = m_talkText[m_page, 2];
                next.IsNext = false;

            }
            await UniTask.WaitUntil(() => { return Input.GetKeyDown(KeyCode.Space); });
            m_page++;
        }
    }





    async UniTask NextPage(string valueText, CancellationToken cancellationToken)
    {
        foreach (var word in valueText)
        {
            m_text.text += word;

            await UniTask.Delay((int)(m_waitTime * 1000), false, PlayerLoopTiming.Update, cancellationToken);
        }
    }
    public class Next
    {
        public bool IsNext { get; set; }

        public async UniTask IsNextAsync()
        {
            await UniTask.WaitUntil(() => { return Input.GetKeyDown(KeyCode.Space); });
            IsNext = true;

        }
    }
   async UniTask Event(CancellationToken cancellationToken)
    {
        switch (int.Parse(m_talkText[m_page, 0]))//発生させるイベント
        {
            case 1:
               await  m_characterManager.CharacterFadeIn(m_talkText[m_page, 1], cancellationToken);
                break;
            case 2:
               await m_characterManager.CharacterFadeOut(m_talkText[m_page, 1], cancellationToken);
                break;
            case 3:
                await m_BackGroundManager.fadeIn(cancellationToken);
                m_BackGroundManager.ChangeBackGround();
                await m_BackGroundManager.fadeOut(cancellationToken);
                break;
            default:

                break;
        }
    }
}
