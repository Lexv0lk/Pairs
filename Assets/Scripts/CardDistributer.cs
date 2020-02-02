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
    [SerializeField] private float _speed;

    private List<Card> _cards = new List<Card>();

    public void AddCards(int pairsCount)
    {
        StartCoroutine(AddingCards(pairsCount * 2));
    }
    
    private IEnumerator AddingCards(int count)
    {
        IsDistributing = true;

        for (int i = 0; i < count; i++)
        {
            Card newCard = Instantiate(_cardPrefab, _cardGrid.transform);
            newCard.ImageTransform.position = transform.position;
            _cards.Add(newCard);
            yield return StartCoroutine(MoveCardToCell(newCard));
            Distributed?.Invoke(newCard);
        }

        SeedCards();
        IsDistributing = false;
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

    private IEnumerator MoveCardToCell(Card card)
    {
        while(card.ImageTransform.localPosition != Vector3.zero)
        {
            card.ImageTransform.localPosition = Vector2.MoveTowards(card.ImageTransform.localPosition, Vector3.zero, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
