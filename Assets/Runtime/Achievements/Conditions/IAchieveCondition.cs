using VContainer;

namespace Achievements.Conditions
{
    public interface IAchieveCondition
    {
        public bool CheckIsConditionMet();
        public void OnScopeBuilt(IObjectResolver resolver);
    }
}