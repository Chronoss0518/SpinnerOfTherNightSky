using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Collections;

public class StarPosGrid : MonoBehaviour, IPointerDownHandler
{
    public Vector3 touchPosition { get; private set; } = Vector3.zero;

    public void OnPointerDown(PointerEventData eventData)
    {
        touchPosition = eventData.pointerCurrentRaycast.worldPosition;
    }


}
