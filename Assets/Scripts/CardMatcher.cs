using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardMatcher : MonoBehaviour
{
    [SerializeField] private CardDistributer _cardDistributer;
    [SerializeField] private float _matchDelay;

    private List<Card> _openedCards = new List<Card>();
    private int _totalOpened = 0;

    public event UnityAction MatchFound;
    public event UnityAction Mistaken;
    public event UnityAction AllMatchesFound;

    private void OnEnable()
    {
        _cardDistributer.Distributed += OnDistributed;
        foreach (var card in _cardDistributer.Cards)
            card.Clicked += OnCardClicked;
    }

    private void OnDisable()
    {
        _cardDistributer.Distributed -= OnDistributed;
        foreach (var card in _cardDistributer.Cards)
            card.Clicked -= OnCardClicked;
    }

    private void OnCardClicked(Card card)
    {
        if (card.IsOpen || _openedCards.Count == 2 || _cardDistributer.IsDistributing)
            return;

        _openedCards.Add(card);
        card.Open();

        if (_openedCards.Count == 2)
            StartCoroutine(TryMatchCards(_openedCards[0], _openedCards[1]));
    }

    private void OnDistributed(Card card)
    {
        card.Clicked += OnCardClicked;
    }

    private IEnumerator TryMatchCards(Card firstCard, Card secondCard)
    {
        yield return new WaitForSecondsRealtime(_matchDelay);

        if (firstCard.Number != secondCard.Number)
        {
            foreach (var card in _openedCards)
                card.Close();
            Mistaken?.Invoke();
        }
        else
        {
            MatchFound?.Invoke();
            _totalOpened += 2;
            if (_totalOpened == _cardDistributer.Cards.Count)
            {
                _totalOpened = 0;
                AllMatchesFound?.Invoke();
            }
        }

        _openedCards.Clear();
    }
}
