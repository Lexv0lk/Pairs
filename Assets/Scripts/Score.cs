using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private CardMatcher _cardMatcher;

    private int _currentValue;

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
        _currentValue++;
        _text.text = "Счёт: " + _currentValue;
    }
}
