using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace AlexaSkillsService.Hubs
{
    public class BombStopperHub : Hub
    {
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public void Hello()
        {
            Clients.All.hello();
        }
    }
}