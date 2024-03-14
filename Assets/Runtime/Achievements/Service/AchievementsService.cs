using System;
using System.Linq;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using JetBrains.Annotations;

using Sirenix.Utilities;

using Achievements.Data;
using Achievements.Configs;
using Achievements.Models;

namespace Achievements.Service
{
    public class AchievementsService : IDisposable
    {
        private readonly AchievesContainerConfig _achievesContainerConfig;
        private readonly AchievesContextModel _achievesContextModel;
        
        private readonly Dictionary<long, AchieveCollection> _profileAchieveCollections;

        private bool _isGotLastData = false;
        private IDisposable _profileIdSub;
        
        public AchievementsService(AchievesContainerConfig achievesContainerConfig, 
            AchievesContextModel achievesContextModel)
        {
            _achievesContainerConfig = achievesContainerConfig;
            _achievesContextModel = achievesContextModel;

            _profileAchieveCollections = new Dictionary<long, AchieveCollection>(1);
        }

        public void Dispose()
        {
            _profileIdSub?.Dispose();
            _profileIdSub = null;
        }

        [CanBeNull]
        public AchieveItem GetAchieveById(string achieveId)
        {
            // For example, id can will come from anywhere
            long profileId = 1;

            if (_profileAchieveCollections.TryGetValue(profileId, out var collection) && collection!= null)
            {
                return collection.Get().Find(item => item.AchieveID.Equals(achieveId));
            }

            return null;
        }

        public async UniTask<List<AchieveItem>> GetAchievesAsync()
        {
            // For example, id can will come from anywhere
            long profileId = 1;

            if (!_profileAchieveCollections.ContainsKey(profileId))
            {
                _profileAchieveCollections.Add(profileId, new AchieveCollection());
            }

            if (_isGotLastData)
            {
                return _profileAchieveCollections[profileId].Get();
            }

            ProfileAchievements[] profileAchievementsArray = null;

            try
            {
                profileAchievementsArray = await GetAchievesFromServerAsync(profileId);
            }
            catch (Exception exception)
            {
                if (profileAchievementsArray == null)
                {
                    return _profileAchieveCollections[profileId].Get();
                }
            }
                
            foreach (var configAchieveItem in _achievesContainerConfig.AchieveItems)
            {
                var passed = profileAchievementsArray
                    .FirstOrDefault(x => x.AchieveName == configAchieveItem.Id);
                
                var (achieveId, count)= passed == null ? (configAchieveItem.Id, 0) : (passed.AchieveName, passed.Count);
                
                _profileAchieveCollections[profileId].AddItem(new AchieveItem(achieveId, 
                    configAchieveItem.AchieveIconReference, configAchieveItem.AchieveShadowIconReference,
                    count, configAchieveItem.MaxCountAchieves, configAchieveItem.Category,
                    configAchieveItem.VisibleRegions));
            }

            _isGotLastData = true;

            return _profileAchieveCollections[profileId].Get();
        }

        private async UniTask<ProfileAchievements[]> GetAchievesFromServerAsync(long profileId)
        {
            // Here the logic to respond to server and get data as you needed
            /*var respond = await _playerService.GetAchievementsAsync(profileId);
            
            if (respond.Error != null && respond.Error.Code.Equals(404))
            {
                // we dont have any achieves in this profile id
                Debug.LogError($"{nameof(AchievementsService)}: {respond.Error}");

                return null;
            }

            return respond.Data;*/

            return Array.Empty<ProfileAchievements>();
        }

        public async UniTaskVoid ClaimAchievementAsync(AchieveItem item)
        {
            // For example, id can will come from anywhere
            long profileId = 1;
            
            // Here the logic to respond to server and send data
            /*if(!_profileAchieveCollections[profileId].AddItemIncrement(item)) return;
            
            _achievesContextModel.Model.Value.Achieves.Add(new AchieveNameAndGradeSnapshotContainer()
            {
                Count = item.CountOfAchieves,
                Name = item.AchieveID
            });
            
            _achievesContextModel.OnSaved();
            
            var respond = await _playerService.ChangeAchievementsAsync(GetRequestProfileAchievementArray(profileId));
            
            if (respond.Error != null)
            {
                Debug.LogError($"{nameof(AchievementsService)}: {respond.Error}");
            }*/
        }

        private void OnProfileIdUpdated(AsyncReactiveProperty<long> profileId)
        {
            _profileIdSub?.Dispose();
            _profileIdSub = null;

            _profileIdSub = profileId.WithoutCurrent().SkipWhile(id => id == 0)
                .Subscribe(x => LoadedDataFromPreferencesAsync(_achievesContextModel.Model.Value, profileId.Value));

            LoadedDataFromPreferencesAsync(_achievesContextModel.Model.Value, profileId.Value);
        }

        private void LoadedDataFromPreferencesAsync(AchievesSnapshotContainer achievesSnapshotContainer, long profileId)
        {
            var id = profileId;

            if (!_profileAchieveCollections.ContainsKey(id))
            {
                _profileAchieveCollections.Add(id, new AchieveCollection());
            }

            foreach (var configAchieveItem in _achievesContainerConfig.AchieveItems)
            {
                var passed = achievesSnapshotContainer
                    .Achieves.FirstOrDefault(x => x.Name == configAchieveItem.Id);
                
                var achieveId= passed.Name.IsNullOrWhitespace() ? configAchieveItem.Id : passed.Name;
                
                _profileAchieveCollections[id].AddItem(new AchieveItem(achieveId, 
                    configAchieveItem.AchieveIconReference, configAchieveItem.AchieveShadowIconReference,
                    passed.Count, configAchieveItem.MaxCountAchieves, configAchieveItem.Category,
                    configAchieveItem.VisibleRegions));
            }
        }
        
        private RequestProfileAchievement[] GetRequestProfileAchievementArray(long profileId)
        {
            var achieveItems = _profileAchieveCollections[profileId].Get();
            var profileAchievements = new RequestProfileAchievement[achieveItems.Count];

            for (int i = 0; i < profileAchievements.Length; i++)
            {
                profileAchievements[i] = new RequestProfileAchievement()
                {
                    ProfileId = profileId,
                    AchieveName = achieveItems[i].AchieveID,
                    Count = achieveItems[i].CountOfAchieves
                };
            }

            return profileAchievements;
        }
    }
}