using Exiled.Events.EventArgs;

namespace AntiSabotage
{
    public class Handler
    {
        public void OnChangingGroup(ChangingGroupEventArgs ev)
        {
            if (Plugin.Instance.Abusers.Contains(ev.Player.UserId))
                ev.IsAllowed = false;
        }
    }
}