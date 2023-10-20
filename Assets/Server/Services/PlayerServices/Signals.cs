using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Services
{
    public class OnManaSignal { public int amount; public int connectionId; }
    public class OnHealthSignal { public int amount; public int connectionId; }
    public class OnDeathSignal { public int connectionId; }
}