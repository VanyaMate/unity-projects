using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VM.UI.Seedling
{
    public class SeedlingInfoPointStatus
    {
        public static Color neutral = new Color(1, 1, 1, 1);
        public static Color good = new Color(0, 1, 0, 1);
        public static Color error = new Color(1, 0, 0, 1);
    }

    public class SeedlingInfoPointUI : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _readyStatus;
        [SerializeField] private Image _progress;
        [SerializeField] private Image _progressBar;

        public void SetInfo (Sprite icon, Color status, float progress)
        {
            this._image.sprite = icon;
            this.UpdateInfo(status, progress);    
        }

        public void UpdateInfo (Color status, float progress)
        {
            this._readyStatus.color = status;
            Vector2 parentSize = ((RectTransform)this._progress.transform).sizeDelta;
            ((RectTransform)this._progressBar.transform).sizeDelta = new Vector2(0, parentSize.y / 100 * progress);
        }

        public void SetSize ()
        {

        }

        public void SetPosition (Vector3 position)
        {
            Vector2 screenPoint = new Vector2(position.x, position.y);
            float distance = position.z;

            this._SetOpacityByDistance(distance);
            this._SetScreenPositionByDistance(screenPoint, distance);
        }

        private void _SetScreenPositionByDistance (Vector2 screenPoint, float distance)
        {
            float coef = 1;
            
            if (distance < 30)
            {
                coef = (30 - distance) / 30;
            }

            transform.position = screenPoint + new Vector2(0, 100 * coef);
        }
        
        private void _SetOpacityByDistance (float distance)
        {
            float coef = 1;
            
            if (distance > 20)
            {
                float delta = 30 - distance;

                if (delta <= 0)
                {
                    coef = 0;
                }
                else
                {
                    coef = delta / 10;
                }
            }

            this._image.color = new Color(
                this._image.color.r, 
                this._image.color.g, 
                this._image.color.b,
                coef
            );

            this._readyStatus.color = new Color(
                this._readyStatus.color.r,
                this._readyStatus.color.g,
                this._readyStatus.color.b,
                coef
            );

            this._progress.color = new Color(
                this._progress.color.r,
                this._progress.color.g,
                this._progress.color.b,
                coef
            );

            this._progressBar.color = new Color(
                this._progressBar.color.r,
                this._progressBar.color.g,
                this._progressBar.color.b,
                coef
            );
        }
    }
}