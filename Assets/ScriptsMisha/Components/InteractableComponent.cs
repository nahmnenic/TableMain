using UnityEngine;
using UnityEngine.Events;

namespace ScriptsMisha.Components
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _actions;

        public void Interact()
        {
            _actions?.Invoke();
        }

        public void TestMethod()
        {
            Debug.Log("Test success");
        }
    }
}