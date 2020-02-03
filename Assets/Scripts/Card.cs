using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public UnityAction<Card> Clicked;

    public bool IsOpened { get; private set; }
    public int Number
    {
        get => int.Parse(_number.text);
        set => _number.text = value.ToString();
    }
    public Transform ImageTransform => _imageTransform;
    public Transform UpperPoint => _upperPoint;

    [SerializeField] private Text _number;
    [SerializeField] private Transform _imageTransform;
    [SerializeField] private float _rotationVelocity;
    [SerializeField] private Transform _upperPoint;

    private bool _isRotating;

    public void Open()
    {
        StartCoroutine(RotateAround(_rotationVelocity));
    }

    public void Close()
    {
        StartCoroutine(RotateAround(-_rotationVelocity));
    }

    private IEnumerator RotateAround(float speed)
    {
        _isRotating = true;
        float startScale = _imageTransform.localScale.x;
        yield return StartCoroutine(ChangeCardRotation(0, speed * Time.deltaTime, Mathf.Min(startScale, 0), Mathf.Max(startScale, 0)));
        _number.enabled = !_number.enabled;
        yield return StartCoroutine(ChangeCardRotation(-startScale, speed * Time.deltaTime, Mathf.Min(-startScale, 0), Mathf.Max(-startScale, 0)));
        _isRotating = false;
        IsOpened = !IsOpened;
    }

    private IEnumerator ChangeCardRotation(float targetValue, float speed, float minimum, float maximum)
    {
        while (_imageTransform.localScale.x != targetValue)
        {
            RotateCard(speed, minimum, maximum);
            yield return null;
        }
    }

    private void RotateCard(float value, float minimum, float maximum)
    {
        _imageTransform.localScale = new Vector3(Mathf.Clamp(_imageTransform.localScale.x + value, minimum, maximum), _imageTransform.localScale.y, _imageTransform.localScale.z);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isRotating)
            return;

        Clicked?.Invoke(this);
    }
}
