using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private CardMatcher _cardMatcher;

    public int Value { get; private set; }

    private void OnEnable()
    {
        _cardMatcher.MatchFound += OnMatchFound;
    }

    private void OnDisable()
    {
        _cardMatcher.MatchFound -= OnMatchFound;
    }

    private void OnMatchFound()
    {
        Value++;
        _text.text = "Счёт: " + Value;
    }
}
