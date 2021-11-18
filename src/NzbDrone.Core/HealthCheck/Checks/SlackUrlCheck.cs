using System.Linq;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.Localization;
using NzbDrone.Core.Notifications;
using NzbDrone.Core.Notifications.Slack;
using NzbDrone.Core.ThingiProvider.Events;

namespace NzbDrone.Core.HealthCheck.Checks
{
    [CheckOn(typeof(ProviderUpdatedEvent<INotificationFactory>))]
    [CheckOn(typeof(ProviderDeletedEvent<INotificationFactory>))]
    [CheckOn(typeof(ProviderStatusChangedEvent<INotificationFactory>))]
    public class SlackUrlCheck : HealthCheckBase
    {
        private readonly INotificationFactory _notificationFactory;

        public SlackUrlCheck(INotificationFactory notificationFactory, ILocalizationService localizationService)
            : base(localizationService)
        {
            _notificationFactory = notificationFactory;
        }

        public override HealthCheck Check()
        {
            if (_notificationFactory.GetAvailableProviders().Where(n => n.Name.Equals("Slack")).Any(s => (s.Definition.Settings as SlackSettings).WebHookUrl.Contains("discord")))
            {
                return new HealthCheck(GetType(), HealthCheckResult.Warning, _localizationService.GetLocalizedString("DiscordUrlInSlackNotification"));
            }

            return new HealthCheck(GetType());
        }
    }
}
