using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FB.Spawner
{
    public struct ColumnData
    {
        public float Height;
        public float Speed;
        public Sprite Sprite;
        public Vector2 Position;
    }

    public class ColumnManager : MonoBehaviour
    {
        [SerializeField] private Transform _topColumn;
        [SerializeField] private Transform _bottomColumn;
        [SerializeField] private BoxCollider2D _scoreTrigger;

        private SpriteRenderer _topColumnSR;
        private SpriteRenderer _bottomColumnSR;
        private bool _moving = false;
        private ColumnData _data;

        public bool Moving { 
            get => this._moving; 
            set => this._moving = value; 
        }

        private void Awake()
        {
            this._topColumnSR = this._topColumn.GetComponent<SpriteRenderer>();
            this._bottomColumnSR = this._bottomColumn.GetComponent<SpriteRenderer>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name == "DeleteColumns")
            {
                MapManager.Instance.ColumnList.Remove(this);
                Destroy(gameObject);
            }
        }

        public void SetData(ColumnData data)
        {
            this._data = data;
            this._topColumnSR.sprite = data.Sprite;
            this._bottomColumnSR.sprite = data.Sprite;
            this._topColumn.localPosition = new Vector2(0, data.Height / 2);
            this._bottomColumn.localPosition = new Vector2(0, -data.Height / 2);
            this._scoreTrigger.size = new Vector2(1, data.Height);

            transform.localPosition = data.Position;

            this._moving = true;
        }

        private void Update()
        {
            if (this._moving == true)
            {
                transform.localPosition = transform.localPosition - new Vector3(this._data.Speed, 0, 0) * Time.deltaTime;
            }
        }
    }
}
