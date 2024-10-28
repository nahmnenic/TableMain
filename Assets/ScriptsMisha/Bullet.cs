using System;
using ScriptsMisha.Utils;
using UnityEngine;

namespace ScriptsMisha
{
    public class Bullet : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Destroy(this.gameObject);
        }
    }
}