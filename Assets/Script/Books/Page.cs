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
}
