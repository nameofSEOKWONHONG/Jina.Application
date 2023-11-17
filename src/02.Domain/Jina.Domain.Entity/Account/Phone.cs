using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Account;

[Table("Phones", Schema = "dbo")]
public class Phone : NumberEntityBase
{
    #region [phone number ]

    /*
     *  국가	국가코드	지역번호	가입자번호	최대 번호 길이
        대한민국	+82	3	8	11
        미국	+1	3	7	10
        중국	+86	3	8	11
        일본	+81	3	8	11
        영국	+44	1	10	11
        프랑스	+33	2	9	11
        독일	+49	2	9	11
        이탈리아	+39	2	9	11
     */

    [Comment("국가코드")]
    [Required, MaxLength(3)]
    public string PhoneNumberNation { get; set; }

    [Comment("제공자 번호")]
    [Required, MaxLength(3)]
    public string PhoneNumberProvider { get; set; }

    [Comment("가입자 번호1")]
    [Required, MaxLength(4)]
    public string PhoneNumber1 { get; set; }

    [Comment("가입자 번호2")]
    [Required, MaxLength(4)]
    public string PhoneNumber2 { get; set; }

    #endregion [phone number ]
}