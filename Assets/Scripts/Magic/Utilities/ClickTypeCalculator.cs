using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Magic
{
    public class ClickTypeCalculator : IClickTypeCalculator
    {
        private bool previousClick = false;

        public void clicked(bool clicked)
        {
            previousClick = clicked;
        }

        public bool isDown(bool clicked)
        {
            if (!previousClick && clicked) return true;
            return false;
        }

        public bool isHeld(bool clicked)
        {
            if (previousClick && clicked) return true;
            return false;
        }

        public bool isUp(bool clicked)
        {
            if (!previousClick && clicked) return true;
            return false;
        }
    }
}
