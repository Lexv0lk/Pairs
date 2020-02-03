using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardDistributer : MonoBehaviour
{
    public UnityAction<Card> Distributed;
    public UnityAction DistributionEnded;

    public IReadOnlyCollection<Card> Cards => _cards.AsReadOnly();
    public bool IsDistributing { get; private set; }

    [SerializeField] private Card _cardPrefab;
    [SerializeField] private GridLayoutGroup _cardGrid;
    [SerializeField] private float _distributionSpeed;
    [SerializeField] private Transform _frameUpperBorder;
    [SerializeField, Range(0, 1)] private float _scalingSpeed;

    private List<Card> _cards = new List<Card>();

    public void AddCards(int pairsCount)
    {
        StartCoroutine(AddingCards(pairsCount * 2));
    }
    
    private IEnumerator AddingCards(int count)
    {
        IsDistributing = true;
        yield return StartCoroutine(RegivingCards());

        for (int i = 0; i < count; i++)
        {
            Card newCard = Instantiate(_cardPrefab, _cardGrid.transform);
            newCard.ImageTransform.position = transform.position;
            _cards.Add(newCard);
            yield return StartCoroutine(MoveCardToCell(newCard));
            Distributed?.Invoke(newCard);
        }

        SeedCards();
        yield return StartCoroutine(ScaleCards());
        IsDistributing = false;
    }

    private IEnumerator RegivingCards()
    {
        if (_cards.Count == 0)
            yield break;

        foreach (var card in _cards)
            card.ImageTransform.position = transform.position;

        foreach (var card in _cards)
            yield return StartCoroutine(MoveCardToCell(card));
    }

    private void SeedCards()
    {
        List<Card> unseededCards = new List<Card>();
        foreach (var card in _cards)
            unseededCards.Add(card);

        int currentNumber = 1;
        while (unseededCards.Count != 0)
        {
            Card card1 = unseededCards[Random.Range(0, unseededCards.Count)];
            Card card2;
            do
                card2 = unseededCards[Random.Range(0, unseededCards.Count)];
            while (card2 == card1);

            card1.Number = currentNumber;
            card2.Number = currentNumber;
            currentNumber++;
            unseededCards.Remove(card1);
            unseededCards.Remove(card2);
        }
    }

    private IEnumerator ScaleCards()
    {
        Transform firstCard = _cards[0].UpperPoint;
        Vector3 highestCardScreenPosition = Camera.main.WorldToScreenPoint(firstCard.position);
        Vector3 upperBorderScreenPosition = Camera.main.WorldToScreenPoint(_frameUpperBorder.position);
        if (upperBorderScreenPosition.y > highestCardScreenPosition.y)
            yield break;

        while (upperBorderScreenPosition.y < highestCardScreenPosition.y)
        {
            _cardGrid.cellSize = new Vector2(_cardGrid.cellSize.x * (1 - _scalingSpeed), _cardGrid.cellSize.y * (1 - _scalingSpeed));
            yield return null;
            highestCardScreenPosition = Camera.main.WorldToScreenPoint(firstCard.position);
        }
    }

    private IEnumerator MoveCardToCell(Card card)
    {
        while(card.ImageTransform.localPosition != Vector3.zero)
        {
            card.ImageTransform.localPosition = Vector2.MoveTowards(card.ImageTransform.localPosition, Vector3.zero, _distributionSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
