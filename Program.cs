using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;

const string url = "http://localhost:5210/chat";

await using var connection = new HubConnectionBuilder().WithUrl(url).Build();

connection.StartAsync().Wait();

await SendMessage();

connection.On<string>("ReceiveMessage", message =>
{   
    Console.WriteLine(message);
});

Console.ReadKey();

async Task SendMessage()
{    
    var httpClient = new HttpClient();
    var response = await httpClient.PostAsJsonAsync("http://localhost:5210/send", "hihihihi!");
    
    if (response.IsSuccessStatusCode)
        Console.WriteLine("Message sent successfully!");
    else
        Console.WriteLine("Failed to send message.");
}

async Task ConnectToStreaming()
{
    await foreach (var date in connection.StreamAsync<DateTime>("Streaming"))
    {
        Console.WriteLine(date);
    }    
}

async Task JoinGroup()
{
    await connection.InvokeAsync("JoinToGroup", "Grupo");
    connection.On<string>("ReceiveMessage", message =>
    {   
        Console.WriteLine(message);
    });


}

// async Task SendMessage()
// {
//     var message = Console.ReadLine();
//     await connection.InvokeAsync("SendMessageToGroup", message);
// }


// while (true)
// {
//     Console.WriteLine("1 - Conectar ao streaming");
//     Console.WriteLine("2 - Juntar ao grupo");
//     int selectedOption = int.Parse(Console.ReadLine()!);

//     switch (selectedOption)
//     {
//         case 1:
//             await ConnectToStreaming();
//             break;
//         case 2:
//             await JoinGroup();
//             break;
//         default:
//             break;
//     }
// }