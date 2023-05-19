using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Magic
{
    public interface IClickTypeCalculator
    {
        public void clicked(bool clicked);
        public bool isDown(bool clicked);
        public bool isUp(bool clicked);
        public bool isHeld(bool clicked);
    }
}
