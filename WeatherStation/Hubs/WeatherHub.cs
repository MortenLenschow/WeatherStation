using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherStation.Hubs
{
    public class WeatherHub : Hub
    {
        public async Task SendMessage(string weatherforecast)
        {
            await Clients.All.SendAsync("ReceiveMessage", weatherforecast);
        }
    }
}
