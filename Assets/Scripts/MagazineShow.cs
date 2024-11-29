using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMagazineChange
{
    public void ChangeToValue(int newValue);
}
public class MagazineShow : MonoBehaviour, IMagazineChange
{
    [SerializeField] private int _bullets;
    [SerializeField] private int _activeBullets;
    [SerializeField] private GameObject[] _images;

    void IMagazineChange.ChangeToValue(int newValue)
    {
        _activeBullets = Mathf.Clamp(newValue ,0 , _bullets);
        for (int i = 0; i < _bullets; i++)
        {
            _images[i].SetActive(false);
        }
        for (int i = 0; i < _activeBullets; i++)
        {
            _images[i].SetActive(true);
        }
    }
}
