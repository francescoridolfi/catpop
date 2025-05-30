using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    public GattoStats gattoStats { get; set; }
    [SerializeField] bool isPlayerUnit;

    public Gatto gatto { get; set; }

    Image image;
    Vector3 originalPos;

    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();

        if (isPlayerUnit)
            originalPos = new Vector3(-290f, -4.3905f);
        else
            originalPos = new Vector3(315f, -4.6855f);
        //originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup()
    {
        if (isPlayerUnit)
            gattoStats = PlayerController.Instance.gattoStats;

        gatto = new Gatto(gattoStats);

        image.sprite = gatto.gattoStats.FrontSprite;
        PlayEnterAnimation();
    }

    public void PlayEnterAnimation()
    {
        image.DOFade(1f, 0.5f);
        if (isPlayerUnit)
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        else
            image.transform.localPosition = new Vector3(500f, originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x, 1f).SetEase(Ease.OutBack);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        else
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }


    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }
    
    
}
