using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System.Threading;
public class BackgroundManager : MonoBehaviour
{
    public float m_speed = 0.01f;//フェードのスピード

    private float m_alfa;//α値を操作するための変数

    private float m_red, m_green, m_blue;//rgbを操作するための変数

    [SerializeField] GameObject m_backgroundPanel;

    [SerializeField] private Image m_backGround;

    [SerializeField] private Sprite[] m_backGroundSprite;

    private int m_count = 0;
    void Start()
    {
        m_red = m_backgroundPanel.transform.GetComponent<Image>().color.r;
        m_green = m_backgroundPanel.transform.GetComponent<Image>().color.g;
        m_blue = m_backgroundPanel.transform.GetComponent<Image>().color.b;
    }

    public async UniTask fadeIn(CancellationToken cancellationToken)
    {
        var image = m_backgroundPanel.GetComponent<Image>();
        cancellationToken.Register(() => image.color = new Color(m_red, m_green, m_blue, 1));
        while (m_alfa < 1)
        {     
            image.color = new Color(m_red, m_green, m_blue, m_alfa);
            m_alfa += m_speed;

            await UniTask.Delay((int)m_speed * 1000, false, PlayerLoopTiming.Update,cancellationToken);
        }
        
    }
    public async UniTask fadeOut(CancellationToken cancellationToken)
    {
        var image = m_backgroundPanel.GetComponent<Image>();
        cancellationToken.Register(() => image.color = new Color(m_red, m_green, m_blue, 0));

        while (m_alfa > 0)
        {
            image.color = new Color(m_red, m_green, m_blue, m_alfa);
            Debug.Log(m_alfa);
            m_alfa -= m_speed;

            await UniTask.Delay((int)m_speed * 1000, false, PlayerLoopTiming.Update, cancellationToken);
        }
      
    }

    public void ChangeBackGround()
    {
        m_count++;
        m_backGround.sprite = m_backGroundSprite[m_count];

    }
}
