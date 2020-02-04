using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TryCounter : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private CardMatcher _cardMatcher;

    private int _currentValue;

    public event UnityAction TriesEnded;

    private void OnEnable()
    {
        _cardMatcher.Mistaken += OnMistaken;
    }

    private void OnDisable()
    {
        _cardMatcher.Mistaken -= OnMistaken;
    }

    public void SetTries(int value)
    {
        ChangeValue(value);
    }

    private void OnMistaken()
    {
        ChangeValue(_currentValue - 1);
        if (_currentValue == 0)
            TriesEnded?.Invoke();
    }

    private void ChangeValue(int newValue)
    {
        _currentValue = newValue;
        _text.text = "Попытки: " + _currentValue.ToString();
    }
}
