using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class StonePosScript : MonoBehaviour
{
    [SerializeField, ReadOnly]
    Vector2Int position = Vector2Int.zero;

    GameManager manager = null;

    public void Init(GameManager _manager, Vector2Int _pos)
    {
        manager = _manager;
        position = _pos;
    }

    public void PushEvent()
    { 
        Debug.Log("Push Pos Is [" + position.x +  "," + position.y + "]");
    }

}
