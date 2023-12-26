using System;
using System.Text;
using AwesomeSocialMedia.Newsfeed.API.Core.Entities;
using AwesomeSocialMedia.Newsfeed.API.Core.Repositories;
using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AwesomeSocialMedia.Newsfeed.API.Consumers
{
    public class PostCreatedConsumer
    {
       
    }

    public class PostCreatedEvent
	{
        public Guid Id { get; set; }
        public string Content { get; set; }
		public DateTime PublishedAt { get; set; }
		public User User { get; set; }
	}
}

