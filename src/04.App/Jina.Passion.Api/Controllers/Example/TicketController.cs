using System.Collections.Concurrent;
using eXtensionSharp;
using Jina.Domain.Service.Infra;
using Microsoft.AspNetCore.Mvc;

namespace Jina.Passion.Api.Controllers.Example;

public class TicketController : ActionController
{
    [HttpPost]
    public IActionResult Ticket(TicketRequest request)
    {
        var ticketNo = Singleton<Ticket>.Instance.Open(request.Id);
        var result = new TicketResult() { TicketNo = ticketNo, Completes = Singleton<Ticket>.Instance.Complete.ToList()};
        return Ok(result);
    }
}

public class TicketRequest
{
    public int Id { get; set; }
}

public class TicketResult
{
    public string TicketNo { get; set; }
    public string Message => TicketNo.xIsEmpty() ? "예약불가" : "예약완료";
    public List<string> Completes { get; set; }
}

public class Singleton<T> where T : class, new()
{
    private static Lazy<T> _instance = new Lazy<T>(() => new T());
    public static T Instance => _instance.Value;
    
    private Singleton()
    {
        
    }
}

public class Ticket
{
    public ConcurrentDictionary<int, string> Tickets = new();
    public ConcurrentBag<string> Complete = new();
    public Ticket()
    {
        foreach (var i in Enumerable.Range(1, 100))
        {
            Tickets.TryAdd(i, Guid.NewGuid().ToString("N"));
        }
    }

    public string Open(int id)
    {
        if (Tickets.TryRemove(id, out var value))
        {  
            Complete.Add(value);
            return value;
        }

        return string.Empty;
    }
}