using System.Collections.Concurrent;
using eXtensionSharp;
using Jina.Domain.Service.Infra;
using Microsoft.AspNetCore.Mvc;

namespace Jina.Passion.Api.Controllers.Example;

public class TicketController : ActionController
{
    [HttpGet]
    public IActionResult GetTicket(int id)
    {
        var ticketNo = TicketImpl.Instance.Open(id);
        return Ok(ticketNo);
    }

    [HttpPost]
    public IActionResult Ticket(TicketRequest request)
    {
        var ticketNo = TicketImpl.Instance.Open(request.Id);
        
        var result = new TicketResult() { TicketNo = ticketNo, Completes = TicketImpl.Instance.Complete.ToList()};
        
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Tickets(TicketRequest[] requests)
    {
        Task[] tasks = new Task[requests.Length];
        var list = new ConcurrentBag<TicketResult>();

        requests.xForEach((i, request) =>
        {
            tasks[i] = Task.Run(() =>
            {
                var ticketNo = TicketImpl.Instance.Open(request.Id);
                list.Add(new TicketResult()
                {
                    TicketNo = ticketNo,
                    Completes = TicketImpl.Instance.Complete.ToList()
                });
            });            
        });

        // 모든 Task가 완료될 때까지 대기
        Task.WaitAll(tasks);
        return Ok(list);
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

//직접 구현했지만, DI를 이용해 Singleton으로 구현하는게 맞다.
//최종적으로 구현하다면 Zone 설정은 Sub/Pub을 이용해서 하던가, Config에서 읽어서 처리해야 함.
//넥슨 인터뷰 관련 구현임.
public class TicketImpl
{
    private static Lazy<TicketImpl> _instance = new Lazy<TicketImpl>(() => new TicketImpl());
    public static TicketImpl Instance => _instance.Value;
    
    public ConcurrentDictionary<int, string> Tickets = new();
    public ConcurrentBag<string> Complete = new();
    public bool IsSoldOut = false;
    public Func<Task> OnSoldOut { get; set; }
    private static object _sync = new();
    
    private TicketImpl()
    {
        foreach (var i in Enumerable.Range(1, 5))
        {
            Tickets.TryAdd(i, Guid.NewGuid().ToString("N"));
        }
    }

    public Task SetZone(string zone)
    {
        //read data from database about ticket zone.
        return Task.CompletedTask;
    }

    public string Open(int id)
    {
        if (Tickets.TryRemove(id, out var value))
        {
            IsSoldOut = Tickets.Count <= 0;
            if (IsSoldOut)
            {
                lock (_sync)
                {
                    OnSoldOut();    
                }
            }
            
            Complete.Add(value);
            return value;
        }

        return string.Empty;
    }
}