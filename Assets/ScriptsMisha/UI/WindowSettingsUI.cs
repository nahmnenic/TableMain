using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScriptsMisha.UI
{
    public class WindowSettingsUI : MonoBehaviour
    {
        private CameraController _cam;
        
        [Header("Text Property")]
        [SerializeField] private Text _sens;
        [SerializeField] private Text _rotSmooth;
        [SerializeField] private Text _moveSmooth;
                
        [Header("Sliders")]
        [SerializeField] private Slider _sensSlider;
        [SerializeField] private Slider _rotSmoothSlider;
        [SerializeField] private Slider _moveSmoothSlider;
        

        private void Start()
        {
            _cam = FindObjectOfType<CameraController>();
            SetFloat();
        }

        public void SetFloat()
        {
            _sens.text = _sensSlider.value.ToString();
            _rotSmooth.text = _rotSmoothSlider.value.ToString();
            _moveSmooth.text = _moveSmoothSlider.value.ToString();
            SetPropertyInGame();
        }

        private void SetPropertyInGame()
        {
            _cam.mouseSense = _sensSlider.value;
            _cam.rotSmoothSpeed = _rotSmoothSlider.value;
            _cam.camSmoothSpeed = _moveSmoothSlider.value;
        }
    }
}