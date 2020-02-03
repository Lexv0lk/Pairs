﻿using UnityEngine;
using UnityEngine.Events;

public class RoundRestarter : MonoBehaviour
{
    public UnityAction LevelChanged;

    public int Level { get; private set; } = 1;

    [SerializeField] private TryCounter _tryCounter;
    [SerializeField] private CardDistributer _cardDistributer;
    [SerializeField] private CardMatcher _cardMatcher;
    [SerializeField] private int _defaultTries;
    [SerializeField] private int _defaultPairsCount;

    private int _lastTries;

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

        Level++;
        if (Level >= 10)
            _lastTries++;

        _tryCounter.SetTries(_lastTries);
        LevelChanged?.Invoke();

        _cardDistributer.AddCards(1);
    }
}