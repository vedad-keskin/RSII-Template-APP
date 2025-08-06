using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Versioning;
using System.Linq;
using ManiFest.Subscriber.Models;
using ManiFest.Subscriber.Interfaces;
using System.Net.Sockets;
using System.Net;

namespace ManiFest.Subscriber.Services
{
    public class BackgroundWorkerService : BackgroundService
    {
        private readonly ILogger<BackgroundWorkerService> _logger;
        private readonly IEmailSenderService _emailSender;
        private readonly string _host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        private readonly string _username = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "guest";
        private readonly string _password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest";
        private readonly string _virtualhost = Environment.GetEnvironmentVariable("RABBITMQ_VIRTUALHOST") ?? "/";

        public BackgroundWorkerService(
            ILogger<BackgroundWorkerService> logger,
            IEmailSenderService emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Check internet connectivity to smtp.gmail.com
            try
            {
                var addresses = await Dns.GetHostAddressesAsync("smtp.gmail.com");
                _logger.LogInformation($"smtp.gmail.com resolved to: {string.Join(", ", addresses.Select(a => a.ToString()))}");
                using (var client = new TcpClient())
                {
                    var connectTask = client.ConnectAsync("smtp.gmail.com", 587);
                    var timeoutTask = Task.Delay(5000, stoppingToken);
                    var completed = await Task.WhenAny(connectTask, timeoutTask);
                    if (completed == connectTask && client.Connected)
                    {
                        _logger.LogInformation("Successfully connected to smtp.gmail.com:587");
                    }
                    else
                    {
                        _logger.LogError("Failed to connect to smtp.gmail.com:587 (timeout or error)");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internet connectivity check failed: {ex.Message}");
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var bus = RabbitHutch.CreateBus($"host={_host};virtualHost={_virtualhost};username={_username};password={_password}"))
                    {
                        // Subscribe to vehicle notifications only
                        bus.PubSub.Subscribe<VehicleNotification>("Vehicle_Notifications", HandleVehicleMessage);

                        _logger.LogInformation("Waiting for vehicle notifications...");
                        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                    }
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in RabbitMQ listener: {ex.Message}");
                }
            }
        }

        private async Task HandleVehicleMessage(VehicleNotification notification)
        {
            var vehicle = notification.Vehicle;

            if (!vehicle.AdminEmails.Any())
            {
                _logger.LogWarning("No admin emails provided in the notification");
                return;
            }

            var subject = "New Vehicle Pending Review";
            var message = $"A new vehicle {vehicle.BrandName} {vehicle.Name} is ready to be accepted or rejected.\n" +
                        $"Please review and take appropriate action.";

            foreach (var email in vehicle.AdminEmails)
            {
                try
                {
                    await _emailSender.SendEmailAsync(email, subject, message);
                    _logger.LogInformation($"Notification sent to admin: {email}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to send email to {email}: {ex.Message}");
                }
            }
        }
    }
}