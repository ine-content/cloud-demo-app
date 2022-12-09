using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Threading.Tasks;
using T=System.Threading;
using System;
namespace csharp_webserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrimaryController : ControllerBase
    {
        private IConfiguration _config;
        public PrimaryController(IConfiguration config)
        {
            _config = config;

        }
        [HttpGet("echo"),HttpPost("echo")]
        public ActionResult<string> Echo()
        {
            string hostName = Dns.GetHostName();
            Console.WriteLine(hostName);

            // Get the IP from GetHostByName method of dns class.
            string IP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
            return new OkObjectResult(new HostData
            {
                hostName = Request.Host.ToString(),
                hostIp = IP,
                computerName = Environment.MachineName,
                remoteIp = HttpContext.Connection != null ? HttpContext.Connection.RemoteIpAddress.ToString() : "",
                message = _config.GetValue<string>("message","")
            }); ;
        }

        [HttpGet("queue"), HttpPost("queue")]
        public async Task<ActionResult<string>> AddToQueue(int itemCount){
            string queueConnection = _config.GetValue<string>("queueConnection");
            string queueName = _config.GetValue<string>("queueName");
            QueueClient client = new QueueClient(queueConnection,queueName);
            await client.CreateIfNotExistsAsync();
            for(int lcv=0;lcv<itemCount;lcv++){
                await client.SendMessageAsync($"Message {lcv} send at {DateTime.Now.ToLongTimeString()}");
            }


            return new OkObjectResult($"{itemCount} messages sent to queue.");
        }

        [HttpGet("load")]
        public ActionResult<string> LoadProcessor(int seconds, int threads){

            var endTime = DateTime.Now.AddSeconds(seconds);
            for(int lcv = 0; lcv < threads; lcv++){
                Task.Factory.StartNew((Object obj)=>{
                    var et = obj as TaskData;
                    double val = 1.001f;
                    while(DateTime.Now<et.EndTime){
                        val = Math.Pow(val,1.5);
                        val = val > double.MaxValue / 2.0f ? 1.001f : val;
                    }
                },new TaskData{EndTime= endTime});
            }

            return new OkObjectResult($"Processor load until {endTime} (current server time = {DateTime.Now})");
        }
    }

    public class TaskData {
        public DateTime EndTime { get; set; } = DateTime.Now;
    }

    public class HostData
    {
        public string hostName { get; set; } = "";
        public string hostIp { get; set; } = "";
        public string computerName { get; set; } = "";
        public string remoteIp { get; set; } = "";
        public string time { get; set; } = DateTime.Now.ToString();
        public string message { get; set; } = "";
    }
}
