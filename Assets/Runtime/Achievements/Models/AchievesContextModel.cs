using System;

using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Achievements.Models
{
    public class AchievesContextModel
    {
        public event Action ModelSaved;
        
        public object TargetModel => _model.Value;
        
        public IReadOnlyAsyncReactiveProperty<AchievesSnapshotContainer> Model => _model;

        private readonly AsyncReactiveProperty<AchievesSnapshotContainer> _model = 
            new AsyncReactiveProperty<AchievesSnapshotContainer>(default);

        public AchievesContextModel()
        {
            _model.Value = new AchievesSnapshotContainer();
        }

        public void SetModel(object model)
        {
            if (model is AchievesSnapshotContainer targetModel)
            {
                SetModel(targetModel);
            }
            else
            {
                Debug.LogError($"Wrong type model: {model.GetType().FullName}, expected {_model.Value.GetType().FullName}");
            }
        }
        
        public void SetModel(AchievesSnapshotContainer model)
        {
            _model.Value = model;
        }
        
        public void OnSaved()
        {
            ModelSaved?.Invoke();
        }
    }
}