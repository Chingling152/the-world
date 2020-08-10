﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace TheChest.UI.Components.Slots
{
    /// <summary>
    /// Class to handle Drag'N drop on Slot
    /// </summary>
    [RequireComponent(typeof(UISlot))]
    [DisallowMultipleComponent]
    public sealed class SlotDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector2 originalPosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            var slot = this.GetComponent<UISlot>();
            this.originalPosition = slot.ItemSprite.rectTransform.position;
            slot.Select();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                var itemSprite = this.GetComponent<UISlot>().ItemSprite;
                itemSprite.rectTransform.position =  Input.mousePosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.GetComponent<UISlot>().ItemSprite.rectTransform.position = originalPosition;
        }
    }
}