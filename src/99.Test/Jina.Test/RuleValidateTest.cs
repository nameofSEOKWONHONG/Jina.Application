using Jina.Validate.RuleValidate.Abstract;
using Jina.Validate.RuleValidate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Test
{
    internal class RuleValidateTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void rule_try_validate_test()
        {
            if (RuleValidatorCore.Core.TryValidate(new Validate.RuleValidate.Abstract.RuleValidateOption()
            {
                Key = "Name",
                Value = "",
                ValidateRule = ENUM_VALIDATE_RULE.NotEmpty,
                CustomMessage = string.Empty
            }, out var msg1))
            {
                Assert.That(msg1, Is.EqualTo($"Name(은)는 빈값일 수 없습니다."));
            }
            else
            {
                Assert.Fail();
            }

            if (RuleValidatorCore.Core.TryValidate(new Validate.RuleValidate.Abstract.RuleValidateOption()
            {
                Key = "Age",
                Value = 10,
                CompareValue = 20,
                ValidateRule = ENUM_VALIDATE_RULE.GraterThen,
                CustomMessage = string.Empty
            }, out var msg2))
            {
                Assert.That(msg2, Is.EqualTo($"Age(은)는 20 보다 커야 합니다."));
            }
            else
            {
                Assert.Fail();
            }

            if (RuleValidatorCore.Core.TryValidate(new Validate.RuleValidate.Abstract.RuleValidateOption()
            {
                Key = "Amount",
                Value = 30m,
                CompareValue = 20m,
                ValidateRule = ENUM_VALIDATE_RULE.LessThen,
                CustomMessage = string.Empty
            }, out var msg3))
            {
                Assert.That(msg3, Is.EqualTo($"Amount(은)는 20 보다 작아야 합니다."));
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void rule_validates_test()
        {
            var items = new List<RuleValidateOption>();
            items.Add(new RuleValidateOption()
            {
                Key = "Name",
                Value = "",
                ValidateRule = ENUM_VALIDATE_RULE.NotEmpty,
                CustomMessage = "test1"
            });
            items.Add(new RuleValidateOption()
            {
                Key = "Age",
                Value = 10,
                CompareValue = 20,
                ValidateRule = ENUM_VALIDATE_RULE.GraterThen,
                CustomMessage = "test2"
            });
            items.Add(new RuleValidateOption()
            {
                Key = "Amount",
                Value = 30m,
                CompareValue = 20m,
                ValidateRule = ENUM_VALIDATE_RULE.LessThen,
                CustomMessage = "test3"
            });

            var results = RuleValidatorCore.Core.Validates(items);
            foreach (var item in results)
            {
                TestContext.Out.WriteLine(item.Key);
                TestContext.Out.WriteLine(item.Message);
                TestContext.Out.WriteLine("=======================");
                Assert.That(item.IsValid, Is.False);
            }
        }
    }
}