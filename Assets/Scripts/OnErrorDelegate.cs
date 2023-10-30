namespace BestHTTP.SignalR
{
	public delegate void OnErrorDelegate(Connection connection, string error);
}
namespace BestHTTP.ServerSentEvents
{
	public delegate void OnErrorDelegate(EventSource eventSource, string error);
}