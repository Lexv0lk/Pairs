using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public UnityAction<Card> Clicked;

    public bool IsOpened => _number.enabled;
    public int Number
    {
        get => int.Parse(_number.text);
        set => _number.text = value.ToString();
    }
    public Transform ImageTransform => _imageTransform;

    [SerializeField] private Text _number;
    [SerializeField] private Transform _imageTransform;
    [SerializeField] private float _rotationVelocity;

    private bool _isRotating;

    public void Open()
    {
        StartCoroutine(RotateAround(_rotationVelocity, () => _number.enabled = true));
    }

    public void Close()
    {
        StartCoroutine(RotateAround(-_rotationVelocity, () => _number.enabled = false));
    }

    private IEnumerator RotateAround(float speed, UnityAction OnHalfRotated)
    {
        _isRotating = true;
        float startScale = _imageTransform.localScale.x;
        yield return StartCoroutine(ChangeCardRotation(0, speed * Time.deltaTime, Mathf.Min(startScale, 0), Mathf.Max(startScale, 0)));
        OnHalfRotated?.Invoke();
        yield return StartCoroutine(ChangeCardRotation(-startScale, speed * Time.deltaTime, Mathf.Min(-startScale, 0), Mathf.Max(-startScale, 0)));
        _isRotating = false;
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
