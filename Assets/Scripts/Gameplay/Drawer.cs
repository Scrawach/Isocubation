using System;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Drawer : MonoBehaviour
    {
        [SerializeField] private float _drawingSpeed;
        
        private MeshRenderer _mesh;
        private Material _initMaterial;

        private void Awake()
        {
            _mesh = GetComponent<MeshRenderer>();
            _initMaterial = _mesh.material;
        }

        public void DrawTo(Material material)
        {
            StartCoroutine(Functions.SmoothChange(_initMaterial, material, _drawingSpeed, MaterialChanging));
        }

        public void Reset()
        {
            var currentMaterial = _mesh.material;
            StartCoroutine(Functions.SmoothChange(currentMaterial, _initMaterial, _drawingSpeed, MaterialChanging));
        }

        private void MaterialChanging(Material init, Material result, float t)
        {
            _mesh.material.Lerp(init, result, t);
        }
    }
}