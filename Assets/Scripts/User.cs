using UnityEngine;

public class User : MonoBehaviour
{
    [SerializeField] private int _level;

    public int Level => _level;
}
