using UnityEngine;
using UnityEngine.Events;

public class GameRestarter : MonoBehaviour
{
    public UnityAction GameEnded;

    [SerializeField] private TryCounter _tryCounter;
    [SerializeField] private CardDistributer _cardDistributer;
    [SerializeField] private CardMatcher _cardMatcher;
    [SerializeField] private int _defaultTries;
    [SerializeField] private int _defaultPairsCount;

    private int _lastTries;
    private int _level = 1;

    private void Start()
    {
        _lastTries = _defaultTries;
        _tryCounter.SetTries(_lastTries);
        _cardDistributer.AddCards(_defaultPairsCount);
    }

    private void OnEnable()
    {
        _tryCounter.TriesEnded += OnTriesEnded;
        _cardMatcher.AllMatchesFound += OnAllMatchesFound;
    }

    private void OnDisable()
    {
        _tryCounter.TriesEnded -= OnTriesEnded;
        _cardMatcher.AllMatchesFound -= OnAllMatchesFound;
    }

    private void OnTriesEnded()
    {
        GameEnded?.Invoke();
    }

    private void OnAllMatchesFound()
    {
        foreach (var card in _cardDistributer.Cards)
            card.Close();
        _cardDistributer.AddCards(1);

        _level++;
        if (_level >= 10)
            _lastTries++;

        _tryCounter.SetTries(_lastTries);
    }
}
