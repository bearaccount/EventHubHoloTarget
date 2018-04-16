using Microsoft.Azure.EventHubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testEventHubSendUWP
{
    class EventHubSend
    {
        private static EventHubClient eventHubClient;
        private const string EhConnectionString = "Endpoint=sb://oseventhub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YwRx2oOuw4JIx8C3AKq0qRWRa8j6Urj8ax21u93vKxk="; //Event Hubs connection string
        private const string EhEntityPath = "HoloLensCmds"; // Event Hub path/name


        public void EventHubSendStart() // took out "static" as can't access method through instance then.
        {
            Debug.WriteLine("EventHubSendStart:enter");

            EventHubSendTask().GetAwaiter().GetResult();
           //EventHubSendTask().Wait();
        }


        private static async Task EventHubSendTask()
        {
            Debug.WriteLine("EventHubSendTask:enter");

            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but this simple scenario
            // uses the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EhConnectionString)
            {
                EntityPath = EhEntityPath
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            await SendMessagesToEventHub(2);

            await eventHubClient.CloseAsync();

            //Console.WriteLine("Press ENTER to exit.");
            //Console.ReadLine();
        }

        // Creates an event hub client and sends 100 messages to the event hub.
        private static async Task SendMessagesToEventHub(int numMessagesToSend)
        {
            Debug.WriteLine("SendMessagesToEventHub:enter");

            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    var message = $"Sent from UWP {i}";
                    Debug.WriteLine($"Sending message: {message}");
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Debug.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(100);
            }

            Debug.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }

}
