using System;
using UnityEngine;
using Zenject;

namespace LevelSetup
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform targetHolder;

        private bool IsTargetVisible => targetHolder.gameObject.activeSelf;

        public void HideTargetIfVisible()
        {
            if(IsTargetVisible) targetHolder.gameObject.SetActive(false);
        }
        
        private void ShowTarget()
        {
            targetHolder.gameObject.SetActive(true);
        }
        
        public void AdjustTarget(Vector3 possiblePos)
        {
            targetHolder.localPosition = possiblePos;
            ShowTarget();
        }
        
        public class Factory : PlaceholderFactory<Level>
        {
        }
    }
}