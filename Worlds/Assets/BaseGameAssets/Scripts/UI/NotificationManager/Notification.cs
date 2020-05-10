using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Notification : MonoBehaviour
    {
        public Image _backGroundObject;
        public Image _iconObject;
        public Text _textObject;

        internal Font font
        {
            get
            {
                return _textObject.font;
            }
            set
            {
                _textObject.font = value;
            }
        }

        internal string text
        {
            get
            {
                return _textObject.text;
            }
            set
            {
                _textObject.text = value;
            }
        }

        internal Sprite background
        {
            get
            {
                return _backGroundObject.sprite;
            }
            set
            {
                _backGroundObject.sprite = value;
            }
        }

        internal Sprite icon
        {
            get
            {
                return _iconObject.sprite;
            }
            set
            {
                if (value != null)
                {
                    _iconObject.sprite = value;
                }
                else
                {
                    _iconObject.gameObject.SetActive(false);
                }
            }
        }
    }
}