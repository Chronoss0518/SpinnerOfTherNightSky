using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPage : MonoBehaviour
{
    public const int CARD_SOCKET_MAX_SIZE = 4;

    [SerializeField]
    private BookSocket[] frontCardSocket = null;

    [SerializeField]
    private BookSocket[] backCardSocket = null;

    public BookSocket[] getFrontCardSocket { get { return frontCardSocket; } }
    
    public BookSocket[] getBackCardSocket { get { return backCardSocket; } }

    public void SetFrontPageActive(bool _active)
    {
        SetPageActive(_active, frontCardSocket);
    }

    public void SetBackPageActive(bool _active)
    {
        SetPageActive(_active, backCardSocket);
    }

    void SetPageActive(bool _active, BookSocket[] _socket)
    {
        foreach (var obj in _socket)
        {
            obj.gameObject.SetActive(_active);
        }
    }
}
