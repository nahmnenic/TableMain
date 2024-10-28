using System.Collections;
using UnityEngine;

namespace ScriptsMisha.Mob
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}