using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class RoundRestarter : MonoBehaviour
{
    [SerializeField] private TryCounter _tryCounter;
    [SerializeField] private CardDistributer _cardDistributer;
    [SerializeField] private CardMatcher _cardMatcher;
    [SerializeField] private int _defaultTries;
    [SerializeField] private int _defaultPairsCount;

    private int _lastTries;

    public event UnityAction LevelChanged;

    public int Level { get; private set; } = 1;

    private void Start()
    {
        _lastTries = _defaultTries;
        _tryCounter.SetTries(_lastTries);
        _cardDistributer.AddCards(_defaultPairsCount);
    }

    private void OnEnable()
    {
        _cardMatcher.AllMatchesFound += OnAllMatchesFound;
    }

    private void OnDisable()
    {
        _cardMatcher.AllMatchesFound -= OnAllMatchesFound;
    }

    private void OnAllMatchesFound()
    {
        foreach (var card in _cardDistributer.Cards)
            card.Close();

        StartCoroutine(IncreaseLevel());
    }

    private IEnumerator IncreaseLevel()
    {
        while (_cardDistributer.Cards.All(x => x.IsOpen == false) == false)
            yield return null;

        Level++;
        if (Level >= 10)
            _lastTries++;

        _tryCounter.SetTries(_lastTries);
        LevelChanged?.Invoke();

        _cardDistributer.AddCards(1);
    }
}
