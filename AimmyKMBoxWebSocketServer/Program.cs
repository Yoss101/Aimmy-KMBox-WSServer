using System;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class WebSocketServer
{
    [DllImport("KMBoxDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool ConnectKMBOX();

    [DllImport("KMBoxDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void StartListening(int vkCode);

    [DllImport("KMBoxDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Move(int x, int y);

    [DllImport("KMBoxDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Up(int key);

    [DllImport("KMBoxDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Down(int key);

    private static async Task StartAsync(HttpListener listener)
    {
        Console.WriteLine("WebSocket server listening on ws://*:8765");

        while (true)
        {
            HttpListenerContext context = await listener.GetContextAsync();
            if (context.Request.IsWebSocketRequest)
            {
                HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                Console.WriteLine("WebSocket connection established");
                WebSocket webSocket = webSocketContext.WebSocket;
                _ = Task.Run(() => HandleConnection(webSocket));
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }

    private static async Task HandleConnection(WebSocket webSocket)
    {
        byte[] buffer = new byte[1024];
        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                string[] command = message.Split();
                if (command[0] == "Down")
                {
                    int x = int.Parse(command[1]);
                    Down(x);
                }
                else if (command[0] == "Up")
                {
                    int x = int.Parse(command[1]);
                    Up(x);
                }
                else if (command[0] == "MouseMove")
                {
                    int x = int.Parse(command[1]);
                    int y = int.Parse(command[2]);
                    Move(x, y);
                    Console.WriteLine($"MouseMove command received with coordinates x: {x}, y: {y}");
                }
            }
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            if (webSocket.State != WebSocketState.Closed)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            }
            webSocket.Dispose();
        }
    }

    public static async Task Main(string[] args)
    {
        ConnectKMBOX();
        HttpListener httpListener = new HttpListener();
        httpListener.Prefixes.Add("http://*:8765/"); // Listen on all network interfaces
        httpListener.Start();
        await StartAsync(httpListener);
    }
}