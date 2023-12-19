using UnityEngine;

namespace TestShooter
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _camera;
        private Camera Camera => _camera ??= Camera.main;

        private Transform _currentTransform;

        private void Awake()
        {
            _currentTransform = transform;
        }

        private void LateUpdate()
        {
            _currentTransform.LookAt(Camera.transform);
        }
    }
}
