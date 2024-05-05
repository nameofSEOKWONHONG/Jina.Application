using eXtensionSharp;
using Jina.Domain.Entity.Language;
using Jina.Domain.Service.Infra;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;

namespace Jina.Db.Test;

public class MockSessionCurrentUser : ISessionCurrentUser
{
    public string TenantId { get; }
    public string Email { get; }
    public string TimeZone { get; }
    public string UserId { get; }
    public string UserName { get; }
    public string RoleName { get; }
    public List<KeyValuePair<string, string>>? Claims { get; }

    public MockSessionCurrentUser()
    {
        TenantId = "00000";
        Email = "admin@test.com";
        TimeZone = "Korea Standard Time";
        UserId = Guid.NewGuid().ToString("N");
        RoleName = "admin";
        Claims = default;
    }
}

public class MockSessionDateTime : ISessionDateTime
{
    private readonly ISessionCurrentUser _user;
    public DateTime Now
    {
        get
        {
            if (_user.TimeZone.xIsEmpty())
                throw new Exception("Session not init or Timezone is empty");
                
            var utcDateTime = DateTime.UtcNow;
            var tenantTimeZone = TimeZoneInfo.FindSystemTimeZoneById(_user.TimeZone);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, tenantTimeZone);
        }
    }

    public DateTime ToLocal(DateTime utc)
    {
        throw new NotImplementedException();
    }

    public DateTime ToUtc(DateTime local)
    {
        throw new NotImplementedException();
    }

    public MockSessionDateTime(ISessionCurrentUser user)
    {
        _user = user;
    }
}

[TestFixture]
public class Tests
{
    private AppDbContext _db;
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "jina")
            .EnableSensitiveDataLogging()
            .LogTo(s =>
            {
                TestContext.Out.WriteLine(s);
            }, LogLevel.Information)
            .Options;

        var mockUser = new MockSessionCurrentUser();
        _db = new AppDbContext(options, mockUser, new MockSessionDateTime(mockUser));
    }

    [Test]
    public async Task Test1()
    {
        var newTopic = new MultilingualTopic()
        {
            TenantId = "00000",
            Name = "front 번역",
            PrimaryCultureType = "en-US",
            CreatedBy = "SYSTEM",
            CreatedName = "SYSTEM".vToAESEncrypt(),
            CreatedOn = DateTime.Now
        };
        var result = await _db.MultilingualTopics.AddAsync(newTopic);
        await _db.SaveChangesAsync();
        Assert.That(result.Entity.Id, Is.Not.Zero);
        
        _db.ChangeTracker.Clear();

        var detail = new MultilingualContent()
        {
            TenantId = "00000",
            CultureType = "ko-KR",
            Input = string.Empty,
            Comment = string.Empty,
            CreatedBy = "SYSTEM",
            CreatedName = "SYSTEM".vToAESEncrypt(),
            CreatedOn = DateTime.Now,
            MultilingualTopicTenantId = newTopic.TenantId,
            MultilingualTopicId = newTopic.Id,
            MultilingualTopicPrimaryCultureType = newTopic.PrimaryCultureType,
        };
        var detail2 = new MultilingualContent()
        {
            TenantId = "00000",
            CultureType = "jp-JA",
            Input = string.Empty,
            Comment = string.Empty,
            CreatedBy = "SYSTEM",
            CreatedName = "SYSTEM".vToAESEncrypt(),
            CreatedOn = DateTime.Now,
            MultilingualTopicTenantId = newTopic.TenantId,
            MultilingualTopicId = newTopic.Id,
            MultilingualTopicPrimaryCultureType = newTopic.PrimaryCultureType,
        };
        await _db.MultilingualContents.AddRangeAsync(new []{detail, detail2});
        await _db.SaveChangesAsync();

        var selected1 = await _db.MultilingualTopics
            .FirstOrDefaultAsync(m => m.TenantId == "00000" && m.Name == "front 번역" && m.PrimaryCultureType == "en-US");

        var selected2 = await _db.MultilingualTopics.Join(_db.MultilingualContents
                , topic => new { topic.TenantId, topic.Id }
                , contents => new { contents.TenantId, contents.Id }
                , (topic, content) => new { topic.TenantId, topic.Id, topic.Name, content.CultureType })
            .ToListAsync();

        var selected3 = await _db.MultilingualTopics.Include(m => m.MultilingualContents)
            .FirstOrDefaultAsync(m => m.Name == "front 번역" && m.PrimaryCultureType == "en-US");


        Assert.Multiple(() =>
        {
            Assert.That(selected3.Id, Is.Not.Zero);
            Assert.That(selected3.MultilingualContents.Count, Is.Not.Zero);
        });

    }

    [TearDown]
    public void TearDown()
    {
        _db.Dispose();
    }
}