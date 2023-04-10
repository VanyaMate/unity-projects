using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WG.Mouse;
using DG.Tweening;

namespace WG.Point
{
    public class PointManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private PointInterface _interface;
        [SerializeField] private PointLine _line;
        [SerializeField] private PointRoads _roads;

        public void OnBeginDrag(PointerEventData eventData)
        {
            this._line.ShowLine();
            this._line.SetLineEndPoint(transform.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            this._line.SetLineEndPoint(MouseManager.Instance.WorldPosition.point);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this._line.HideLine();
            this._roads.MakeRoadTo(MouseManager.Instance.WorldPosition.point);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this._interface.OnHover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this._interface.OnExit();
        }
    }
}
