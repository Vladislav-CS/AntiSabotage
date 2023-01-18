using Exiled.Events.EventArgs.Player;

namespace AntiSabotage
{
    public class PlayerHandler
    {
        public void OnChangingGroup(ChangingGroupEventArgs ev)
        {
            if (Plugin.Instance.Abusers.Contains(ev.Player.UserId))
                ev.IsAllowed = false;
        }
    }
}