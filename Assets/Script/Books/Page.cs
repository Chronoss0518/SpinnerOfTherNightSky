using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{
    public const int CARD_SOCKET_MAX_SIZE = 4;

    [SerializeField]
    private GameObject[] frontCardSocket = null;

    [SerializeField]
    private GameObject[] backCardSocket = null;

    public GameObject[] getFrontCardSocket { get { return frontCardSocket; } }
    
    public GameObject[] getBackCardSocket { get { return backCardSocket; } }

    public void SetFrontPageActive(bool _active)
    {
        SetPageActive(_active, frontCardSocket);
    }

    public void SetBackPageActive(bool _active)
    {
        SetPageActive(_active, backCardSocket);
    }

    void SetPageActive(bool _active, GameObject[] _socket)
    {
        foreach (var obj in _socket)
        {
            obj.SetActive(_active);
        }
    }
}
