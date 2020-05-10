using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum Backgrounds
    {
        none = -1, Blank, Hostile, Friendly
    }

    public enum Icons
    {
        none = -1, Explamation, Question
    }

    public enum Fonts
    {
        Default, Hostile, Friendly
    }

    public class NotificationManager : MonoBehaviour
    {
        public static NotificationManager local
        {
            get
            {
                return FindObjectOfType<NotificationManager>();
            }
        }

        public GameObject Prefab;

        [Space]
        public List<Sprite> backGrounds;

        public List<Sprite> icons;

        public List<Font> fonts;

        private void OnValidate()
        {
            var backGroundsLength = Enum.GetNames(typeof(Backgrounds)).Length;

            while (backGrounds.Count > backGroundsLength)
            {
                if (backGrounds.Count > 0)
                    backGrounds.Add(backGrounds[backGrounds.Count - 1]);
                else backGrounds.Add(null);
            }

            while (backGrounds.Count > backGroundsLength)
            {
                backGrounds.RemoveAt(backGrounds.Count - 1);
            }

            var iconsLength = Enum.GetNames(typeof(Icons)).Length;

            while (icons.Count > iconsLength)
            {
                if (icons.Count > 0)
                    icons.Add(icons[icons.Count - 1]);
                else icons.Add(null);
            }

            while (icons.Count > iconsLength)
            {
                icons.RemoveAt(icons.Count - 1);
            }

            var fontsLength = Enum.GetNames(typeof(Fonts)).Length;

            while (fonts.Count > fontsLength)
            {
                if (fonts.Count > 0)
                    fonts.Add(fonts[fonts.Count - 1]);
                else fonts.Add(null);
            }

            while (fonts.Count > fontsLength)
            {
                fonts.RemoveAt(icons.Count - 1);
            }
        }

        public void CreateMessage(string text, float time = 3, Fonts font = Fonts.Default, Backgrounds background = Backgrounds.none, Icons icon = Icons.none)
        {
            // Setup background
            var message = Instantiate(Prefab, transform);

            var notificationComponent = message.GetComponent<Notification>();

            notificationComponent.text = text;
            notificationComponent.font = fonts[(int)font];
            if (background != Backgrounds.none)
                notificationComponent.background = backGrounds[(int)background];

            if (icon != Icons.none)
                notificationComponent.icon = icons[(int)icon];

            StartCoroutine(RemoveMessage(message, time));
        }

        public void CreateMessage(string text, float time = 3, Fonts font = Fonts.Default, Backgrounds background = Backgrounds.none, Sprite sprite = null)
        {
            if (sprite == null)
            {
                CreateMessage(text, time, font, background, 0);

                return;
            }

            // Setup background
            var message = Instantiate(Prefab, transform);

            var notificationComponent = message.GetComponent<Notification>();

            notificationComponent.text = text;
            notificationComponent.font = fonts[(int)font];
            notificationComponent.background = backGrounds[(int)background];
            notificationComponent.icon = sprite;

            StartCoroutine(RemoveMessage(message, time));
        }

        private IEnumerator RemoveMessage(GameObject message, float time)
        {
            yield return new WaitForSecondsRealtime(time);

            float one = 1;

            var messageImage = message.GetComponent<Image>();
            var miColor = messageImage.color;

            var text = message.GetComponentInChildren<Text>();
            var tColor = text.color;

            while (one > 0)
            {
                messageImage.color = new Color(miColor.r, miColor.g, miColor.b, messageImage.color.a - 0.1f);
                text.color = new Color(tColor.r, tColor.g, tColor.b, text.color.a - 0.1f);
                one = messageImage.color.a;

                yield return null;
            }

            Destroy(message);
        }
    }
}