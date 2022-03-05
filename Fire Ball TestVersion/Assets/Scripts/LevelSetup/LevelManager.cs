using System;
using System.Collections.Generic;
using PlayerSetup;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace LevelSetup
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LevelManager : IInitializable, ITickable
    {
        private Settings _settings;
        private Player _player;
        private Level.Factory _levelFactory;
        
        private readonly List<Level> _columnList = new List<Level>();
        private int _oldestColumnIndex;
        private float _lastAppliedBallZ;
        private float _hue;
        private int _targetCounter;
        private const float LaneSize = 5;
        private Vector3 _prevPos = Vector3.back * 15f;
            
        [Inject]
        public void Construct(Settings settings, Player player, Level.Factory levelFactory)
        {
            _settings = settings;
            _player = player;
            _levelFactory = levelFactory;
        }

        public void Initialize()
        { 
            for (var i = 0; i < _settings.count; i++)
            {
                var wall = _levelFactory.Create();
                _columnList.Add(wall);
                _prevPos = ReUseWall(_columnList.Count - 1, _prevPos, 1);
            }
            _hue = 0;
            _oldestColumnIndex = 0;
        }
        
        public void Tick()
        {
            if (!(_player.transform.position.z > _lastAppliedBallZ + LaneSize)) return;
        
            _prevPos = ReUseWall(_oldestColumnIndex, _prevPos, _settings.maxVariance);
            _oldestColumnIndex = (_oldestColumnIndex + 1) % _columnList.Count;
            _lastAppliedBallZ += LaneSize;
        }
        
        private Vector3 ReUseWall(int columnIndex, Vector3 prevColumnPos, float maximumVariance)
        {
            var range = Random.Range(prevColumnPos.y - maximumVariance, prevColumnPos.y + maximumVariance);
            var movePos = new Vector3(prevColumnPos.x, range, prevColumnPos.z + LaneSize);
        
            _columnList[columnIndex].transform.position = movePos;
            SetWallColor(columnIndex);
            _targetCounter++;

            _columnList[columnIndex].HideTargetIfVisible();
            
            if (_targetCounter < _settings.targetFrequency) return movePos;
        
            var possiblePos = new Vector3(0, Random.Range(-4f, -14f), 0);
            
            _columnList[columnIndex].AdjustTarget(possiblePos);
            _targetCounter = 0;

            return movePos;
        }

        private void SetWallColor(int wallIndex)
        {
            _hue += 1f / 250f;
            if (_hue > 1f)
                _hue = 0;
     
            var saturation = .7f;
            if (wallIndex % 2 == 0) saturation = .5f;
        
            foreach (var meshRenderer in _columnList[wallIndex].GetComponentsInChildren<MeshRenderer>())
                meshRenderer.material.color = Color.HSVToRGB(_hue, saturation, .7f);
        }

        [Serializable]
        public class Settings
        {
            public float maxVariance = 2f;
            public int count = 16;
            public int targetFrequency = 20;
        }

    }
}