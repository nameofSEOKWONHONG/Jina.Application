using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Jina.Domain.Entity.Language;
using Jina.Domain.Service.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jina.Db.Test;

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
        Assert.That(result.Entity.Id, Is.Not.Zero);

        var detail = new MultilingualTopicConfig()
        {
            TenantId = "00000",
            CultureType = "ko-KR",
            CreatedBy = "SYSTEM",
            CreatedName = "SYSTEM".vToAESEncrypt(),
            CreatedOn = DateTime.Now,
            MultilingualTopicTenantId = newTopic.TenantId,
            MultilingualTopicId = newTopic.Id,
            MultilingualTopicPrimaryCultureType = newTopic.PrimaryCultureType,
        };
        var detail2 = new MultilingualTopicConfig()
        {
            TenantId = "00000",
            CultureType = "jp-JA",
            CreatedBy = "SYSTEM",
            CreatedName = "SYSTEM".vToAESEncrypt(),
            CreatedOn = DateTime.Now,
            MultilingualTopicTenantId = newTopic.TenantId,
            MultilingualTopicId = newTopic.Id,
            MultilingualTopicPrimaryCultureType = newTopic.PrimaryCultureType,
        };
        await _db.MultilingualTopicConfigs.AddRangeAsync(new []{detail, detail2});
        await _db.SaveChangesAsync();

        var selected1 = await _db.MultilingualTopics
            .FirstOrDefaultAsync(m => m.TenantId == "00000" && m.Name == "front 번역" && m.PrimaryCultureType == "en-US");

        // var selected2 = await _db.MultilingualTopics.Join(_db.MultilingualContents
        //         , topic => new { topic.TenantId, topic.Id }
        //         , contents => new { contents.TenantId, contents.Guid }
        //         , (topic, content) => new { topic.TenantId, topic.Id, topic.Name, content.CultureType })
        //     .ToListAsync();

        var selected3 = await _db.MultilingualTopics.Include(m => m.MultilingualTopicConfigs)
            .FirstOrDefaultAsync(m => m.Name == "front 번역" && m.PrimaryCultureType == "en-US");
        Assert.Multiple(() =>
        {
            Assert.That(selected3.Id, Is.Not.Zero);
            Assert.That(selected3.MultilingualTopicConfigs.Count, Is.Not.Zero);
        });
        
        var linqQuery =
            from a in _db.MultilingualTopics
            from b in _db.MultilingualTopicConfigs.Where(m => m.TenantId == a.TenantId &&
                                                              m.MultilingualTopicId == a.Id &&
                                                              m.MultilingualTopicPrimaryCultureType ==
                                                              a.PrimaryCultureType).DefaultIfEmpty()
            select new
            {
                a.Name,
                a.Id
            };
        var result3 = await linqQuery.ToListAsync();
        Assert.That(result3.Count, Is.GreaterThan(1));

        _db.MultilingualTopics.Remove(selected1);
        await _db.SaveChangesAsync();

        _db.ChangeTracker.Clear();
        
        var topic = await _db.MultilingualTopics.FirstOrDefaultAsync(m => m.Name == "front 번역" &&
                                                                     m.PrimaryCultureType == "en-US");
        var contents = await _db.MultilingualTopicConfigs.Where(m => m.MultilingualTopicTenantId == selected1.TenantId &&
                                                                 m.MultilingualTopicId == selected1.Id &&
                                                                 m.MultilingualTopicPrimaryCultureType ==
                                                                 selected1.PrimaryCultureType)
            .ToListAsync();
        Assert.Multiple(() =>
        {
            Assert.That(topic, Is.Null);
            Assert.That(contents, Is.Empty);
        });


        
    }

    [TearDown]
    public void TearDown()
    {
        _db.Dispose();
    }
}