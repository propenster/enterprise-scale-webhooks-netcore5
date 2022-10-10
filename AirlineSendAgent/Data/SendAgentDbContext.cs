using AirlineSendAgent.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineWeb.Data
{

    public class SendAgentDbContext : DbContext
    {

        public SendAgentDbContext(DbContextOptions<SendAgentDbContext> options) : base(options)
        {

        }

        public DbSet<WebhookSubscription> WebhookSubscriptions { get; set; }




    }
}