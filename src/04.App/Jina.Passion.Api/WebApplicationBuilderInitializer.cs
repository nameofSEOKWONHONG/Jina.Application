﻿using eXtensionSharp;
using Hangfire;
using Jina.Domain.Entity.Account;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared.Consts;
using Jina.Injection;
using Jina.Lang;
using Jina.Lang.Abstract;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using Jina.Application.Configuration;
using Jina.Base.Service;
using Jina.Database;
using Jina.Database.Abstract;
using Jina.Domain.Service.Infra.Middleware;
using Jina.Domain.Service.Net.Notification;
using Jina.Validate;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Results = Jina.Domain.Shared.Results;
using Role = Jina.Domain.Entity.Account.Role;

namespace Jina.Passion.Api
{
	public class WebApplicationBuilderInitializer
    {
        const string DOMAIN_NAME = "Jina.Domain";
        
        public static WebApplicationBuilder Initialize(WebApplicationBuilder builder)
        {
            #region [test]

            builder.Services.AddScoped<MessageHub>();

            #endregion [test]

            #region [session context]

            builder.Services.AddScoped<ISessionContext, SessionContext>();
            builder.Services.AddScoped<ISessionCurrentUser, SessionCurrentUser>();
            builder.Services.AddScoped<ISessionDateTime, SessionDateTime>();

            #endregion [session context]

            #region [localizer]

            builder.Services.AddSingleton<ILocalizer, Localizer>();

            #endregion [localizer]

            #region [identity]

            builder.Services
                .AddIdentity<User, Role>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                    var allowKr = options.User.AllowedUserNameCharacters + " " +
                        "가각간갇갈갉갊감갑값갓갔강갖갗같갚갛개객갠갤갬갭갯갰갱갸갹갼걀걋걍걔걘걜거걱건걷걸걺검겁것겄겅겆겉겊겋게겐겔겜겝겟겠겡겨격겪견겯결겸겹겻겼경곁계곈곌곕곗고곡곤곧골곪곬곯곰곱곳공곶과곽관괄괆괌괍괏광괘괜괠괩괬괭괴괵괸괼굄굅굇굉교굔굘굡굣구국군굳굴굵굶굻굼굽굿궁궂궈궉권궐궜궝궤궷귀귁귄귈귐귑귓규균귤그극근귿글긁금급긋긍긔기긱긴긷길긺김깁깃깅깆깊까깍깎깐깔깖깜깝깟깠깡깥깨깩깬깰깸깹깻깼깽꺄꺅꺌꺼꺽꺾껀껄껌껍껏껐껑께껙껜껨껫껭껴껸껼꼇꼈꼍꼐꼬꼭꼰꼲꼴꼼꼽꼿꽁꽂꽃꽈꽉꽐꽜꽝꽤꽥꽹꾀꾄꾈꾐꾑꾕꾜꾸꾹꾼꿀꿇꿈꿉꿋꿍꿎꿔꿜꿨꿩꿰꿱꿴꿸뀀뀁뀄뀌뀐뀔뀜뀝뀨끄끅끈끊끌끎끓끔끕끗끙끝끼끽낀낄낌낍낏낑나낙낚난낟날낡낢남납낫났낭낮낯낱낳내낵낸낼냄냅냇냈냉냐냑냔냘냠냥너넉넋넌널넒넓넘넙넛넜넝넣네넥넨넬넴넵넷넸넹녀녁년녈념녑녔녕녘녜녠노녹논놀놂놈놉놋농높놓놔놘놜놨뇌뇐뇔뇜뇝뇟뇨뇩뇬뇰뇹뇻뇽누눅눈눋눌눔눕눗눙눠눴눼뉘뉜뉠뉨뉩뉴뉵뉼늄늅늉느늑는늘늙늚늠늡늣능늦늪늬늰늴니닉닌닐닒님닙닛닝닢다닥닦단닫달닭닮닯닳담답닷닸당닺닻닿대댁댄댈댐댑댓댔댕댜더덕덖던덛덜덞덟덤덥덧덩덫덮데덱덴델뎀뎁뎃뎄뎅뎌뎐뎔뎠뎡뎨뎬도독돈돋돌돎돐돔돕돗동돛돝돠돤돨돼됐되된될됨됩됫됬됴두둑둔둘둠둡둣둥둬뒀뒈뒝뒤뒨뒬뒵뒷뒹듀듄듈듐듕드득든듣들듦듬듭듯등듸디딕딘딛딜딤딥딧딨딩딪따딱딴딸땀땁땃땄땅땋때땍땐땔땜땝땟땠땡떠떡떤떨떪떫떰떱떳떴떵떻떼떽뗀뗄뗌뗍뗏뗐뗑뗘뗬또똑똔똘똥똬똴뙈뙤뙨뚜뚝뚠뚤뚫뚬뚱뛔뛰뛴뛸뜀뜁뜅뜨뜩뜬뜯뜰뜸뜹뜻띄띈띌띔띕띠띤띨띰띱띳띵라락란랄람랍랏랐랑랒랖랗래랙랜랠램랩랫랬랭랴략랸럇량러럭런럴럼럽럿렀렁렇레렉렌렐렘렙렛렝려력련렬렴렵렷렸령례롄롑롓로록론롤롬롭롯롱롸롼뢍뢨뢰뢴뢸룀룁룃룅료룐룔룝룟룡루룩룬룰룸룹룻룽뤄뤘뤠뤼뤽륀륄륌륏륑류륙륜률륨륩륫륭르륵른를름릅릇릉릊릍릎리릭린릴림립릿링마막만많맏말맑맒맘맙맛망맞맡맣매맥맨맬맴맵맷맸맹맺먀먁먈먕머먹먼멀멂멈멉멋멍멎멓메멕멘멜멤멥멧멨멩며멱면멸몃몄명몇몌모목몫몬몰몲몸몹못몽뫄뫈뫘뫙뫼묀묄묍묏묑묘묜묠묩묫무묵묶문묻물묽묾뭄뭅뭇뭉뭍뭏뭐뭔뭘뭡뭣뭬뮈뮌뮐뮤뮨뮬뮴뮷므믄믈믐믓미믹민믿밀밂밈밉밋밌밍및밑바박밖밗반받발밝밞밟밤밥밧방밭배백밴밸뱀뱁뱃뱄뱅뱉뱌뱍뱐뱝버벅번벋벌벎범법벗벙벚베벡벤벧벨벰벱벳벴벵벼벽변별볍볏볐병볕볘볜보복볶본볼봄봅봇봉봐봔봤봬뵀뵈뵉뵌뵐뵘뵙뵤뵨부북분붇불붉붊붐붑붓붕붙붚붜붤붰붸뷔뷕뷘뷜뷩뷰뷴뷸븀븃븅브븍븐블븜븝븟비빅빈빌빎빔빕빗빙빚빛빠빡빤빨빪빰빱빳빴빵빻빼빽뺀뺄뺌뺍뺏뺐뺑뺘뺙뺨뻐뻑뻔뻗뻘뻠뻣뻤뻥뻬뼁뼈뼉뼘뼙뼛뼜뼝뽀뽁뽄뽈뽐뽑뽕뾔뾰뿅뿌뿍뿐뿔뿜뿟뿡쀼쁑쁘쁜쁠쁨쁩삐삑삔삘삠삡삣삥사삭삯산삳살삵삶삼삽삿샀상샅새색샌샐샘샙샛샜생샤샥샨샬샴샵샷샹섀섄섈섐섕서석섞섟선섣설섦섧섬섭섯섰성섶세섹센셀셈셉셋셌셍셔셕션셜셤셥셧셨셩셰셴셸솅소속솎손솔솖솜솝솟송솥솨솩솬솰솽쇄쇈쇌쇔쇗쇘쇠쇤쇨쇰쇱쇳쇼쇽숀숄숌숍숏숑수숙순숟술숨숩숫숭숯숱숲숴쉈쉐쉑쉔쉘쉠쉥쉬쉭쉰쉴쉼쉽쉿슁슈슉슐슘슛슝스슥슨슬슭슴습슷승시식신싣실싫심십싯싱싶싸싹싻싼쌀쌈쌉쌌쌍쌓쌔쌕쌘쌜쌤쌥쌨쌩썅써썩썬썰썲썸썹썼썽쎄쎈쎌쏀쏘쏙쏜쏟쏠쏢쏨쏩쏭쏴쏵쏸쐈쐐쐤쐬쐰쐴쐼쐽쑈쑤쑥쑨쑬쑴쑵쑹쒀쒔쒜쒸쒼쓩쓰쓱쓴쓸쓺쓿씀씁씌씐씔씜씨씩씬씰씸씹씻씽아악안앉않알앍앎앓암압앗았앙앝앞애액앤앨앰앱앳앴앵야약얀얄얇얌얍얏양얕얗얘얜얠얩어억언얹얻얼얽얾엄업없엇었엉엊엌엎에엑엔엘엠엡엣엥여역엮연열엶엷염엽엾엿였영옅옆옇예옌옐옘옙옛옜오옥온올옭옮옰옳옴옵옷옹옻와왁완왈왐왑왓왔왕왜왝왠왬왯왱외왹왼욀욈욉욋욍요욕욘욜욤욥욧용우욱운울욹욺움웁웃웅워웍원월웜웝웠웡웨웩웬웰웸웹웽위윅윈윌윔윕윗윙유육윤율윰윱윳융윷으윽은을읊음읍읏응읒읓읔읕읖읗의읜읠읨읫이익인일읽읾잃임입잇있잉잊잎자작잔잖잗잘잚잠잡잣잤장잦재잭잰잴잼잽잿쟀쟁쟈쟉쟌쟎쟐쟘쟝쟤쟨쟬저적전절젊점접젓정젖제젝젠젤젬젭젯젱져젼졀졈졉졌졍졔조족존졸졺좀좁좃종좆좇좋좌좍좔좝좟좡좨좼좽죄죈죌죔죕죗죙죠죡죤죵주죽준줄줅줆줌줍줏중줘줬줴쥐쥑쥔쥘쥠쥡쥣쥬쥰쥴쥼즈즉즌즐즘즙즛증지직진짇질짊짐집짓징짖짙짚짜짝짠짢짤짧짬짭짯짰짱째짹짼쨀쨈쨉쨋쨌쨍쨔쨘쨩쩌쩍쩐쩔쩜쩝쩟쩠쩡쩨쩽쪄쪘쪼쪽쫀쫄쫌쫍쫏쫑쫓쫘쫙쫠쫬쫴쬈쬐쬔쬘쬠쬡쭁쭈쭉쭌쭐쭘쭙쭝쭤쭸쭹쮜쮸쯔쯤쯧쯩찌찍찐찔찜찝찡찢찧차착찬찮찰참찹찻찼창찾채책챈챌챔챕챗챘챙챠챤챦챨챰챵처척천철첨첩첫첬청체첵첸첼쳄쳅쳇쳉쳐쳔쳤쳬쳰촁초촉촌촐촘촙촛총촤촨촬촹최쵠쵤쵬쵭쵯쵱쵸춈추축춘출춤춥춧충춰췄췌췐취췬췰췸췹췻췽츄츈츌츔츙츠측츤츨츰츱츳층치칙친칟칠칡침칩칫칭카칵칸칼캄캅캇캉캐캑캔캘캠캡캣캤캥캬캭컁커컥컨컫컬컴컵컷컸컹케켁켄켈켐켑켓켕켜켠켤켬켭켯켰켱켸코콕콘콜콤콥콧콩콰콱콴콸쾀쾅쾌쾡쾨쾰쿄쿠쿡쿤쿨쿰쿱쿳쿵쿼퀀퀄퀑퀘퀭퀴퀵퀸퀼큄큅큇큉큐큔큘큠크큭큰클큼큽킁키킥킨킬킴킵킷킹타탁탄탈탉탐탑탓탔탕태택탠탤탬탭탯탰탱탸턍터턱턴털턺텀텁텃텄텅테텍텐텔템텝텟텡텨텬텼톄톈토톡톤톨톰톱톳통톺톼퇀퇘퇴퇸툇툉툐투툭툰툴툼툽툿퉁퉈퉜퉤튀튁튄튈튐튑튕튜튠튤튬튱트특튼튿틀틂틈틉틋틔틘틜틤틥티틱틴틸팀팁팃팅파팍팎판팔팖팜팝팟팠팡팥패팩팬팰팸팹팻팼팽퍄퍅퍼퍽펀펄펌펍펏펐펑페펙펜펠펨펩펫펭펴편펼폄폅폈평폐폘폡폣포폭폰폴폼폽폿퐁퐈퐝푀푄표푠푤푭푯푸푹푼푿풀풂품풉풋풍풔풩퓌퓐퓔퓜퓟퓨퓬퓰퓸퓻퓽프픈플픔픕픗피픽핀필핌핍핏핑하학한할핥함합핫항핳해핵핸핼햄햅햇했행햐향허헉헌헐헒험헙헛헝헤헥헨헬헴헵헷헹혀혁현혈혐협혓혔형혜혠혤혭호혹혼홀홅홈홉홋홍홑화확환활홧황홰홱홴횃횅회획횐횔횝횟횡효횬횰횹횻후훅훈훌훑훔훗훙훠훤훨훰훵훼훽휀휄휑휘휙휜휠휨휩휫휭휴휵휸휼흄흇흉흐흑흔흖흗흘흙흠흡흣흥흩희흰흴흼흽힁히힉힌힐힘힙힛힝힣ㄱㄴㄷㄹㅁㅂㅅㅇㅈㅊㅋㅌㅍㅎㄲㄸㅃㅆㅉㄺㅀㄻㄼㅄㄳㄶㄵㄽㅏㅑㅓㅕㅗㅛㅜㅠㅡㅣㅒㅖ";
                    options.User.AllowedUserNameCharacters = allowKr;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            var key = Encoding.UTF8.GetBytes(builder.Configuration["ApplicationConfig:Secret"]);
            builder.Services
                            .AddAuthentication(authentication =>
                            {
                                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                            })
                            .AddJwtBearer(bearer =>
                            {
                                bearer.RequireHttpsMetadata = false;
                                bearer.SaveToken = true;
                                bearer.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(key),
                                    ValidateIssuer = false,
                                    ValidateAudience = false,
                                    RoleClaimType = ClaimTypes.Role,
                                    ClockSkew = TimeSpan.Zero
                                };

                                bearer.Events = new JwtBearerEvents
                                {
                                    OnMessageReceived = context =>
                                    {
                                        var accessToken = context.Request.Query["access_token"];

                                        // If the request is for our hub...
                                        var path = context.HttpContext.Request.Path;
                                        if (!string.IsNullOrEmpty(accessToken)
                                            && (path.StartsWithSegments(ApplicationConsts.SignalR.NotificationHubName)))
                                        {
                                            // Read the token out of the query string
                                            context.Token = accessToken;
                                        }
                                        return Task.CompletedTask;
                                    },
                                    OnAuthenticationFailed = c =>
                                    {
                                        if (c.Exception is SecurityTokenExpiredException)
                                        {
                                            c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                            c.Response.ContentType = "application/json";
                                            var result = Results.Fail("The Token is expired.").xToJson();
                                            return c.Response.WriteAsync(result);
                                        }
                                        else
                                        {
                                            c.NoResult();
                                            c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                            c.Response.ContentType = "text/plain";
                                            var result = Results.Fail("You are not Authorized.").xToJson();
                                            return c.Response.WriteAsync(result);
                                        }
                                    },
                                    OnChallenge = context =>
                                    {
                                        context.HandleResponse();
                                        if (!context.Response.HasStarted)
                                        {
                                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                            context.Response.ContentType = "application/json";
                                            var result = Results.Fail("You are not Authorized.").xToJson();
                                            return context.Response.WriteAsync(result);
                                        }

                                        return Task.CompletedTask;
                                    },
                                    OnForbidden = context =>
                                    {
                                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                                        context.Response.ContentType = "application/json";
                                        var result = Results.Fail("You are not authorized to access this resource.").xToJson();
                                        return context.Response.WriteAsync(result);
                                    },
                                };
                            });

            builder.Services.AddAuthorization(options =>
            {
                // Here I stored necessary permissions/roles in a constant
                foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
                {
                    var propertyValue = prop.GetValue(null);
                    if (propertyValue is not null)
                    {
                        options.AddPolicy(propertyValue.xValue<string>(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.xValue<string>()));
                    }
                }
            });

            #endregion [identity]

            #region [service injection]

            builder.Services.AddJinaScan(new []
            {
                Jina.Domain.Abstract.PersistenceAssembly.Assembly,
                Jina.Domain.Service.Account.PersistenceAssembly.Assembly,
                Jina.Domain.Service.Example.PersistenceAssembly.Assembly,
                Jina.Domain.Service.Net.PersistenceAssembly.Assembly
            });
            builder.Services.AddValidator(new []
            {
                Jina.Domain.PersistenceAssembly.Assembly
            });

            #endregion [service injection]

            #region [add httpcontextaccessor]

            // Add services to the container.
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient();

            #endregion

            #region [cors]

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowedCorsOrigins",
                    builder =>
                    {
                        builder
                            .SetIsOriginAllowed(IsOriginAllowed)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            #endregion [cors]

            #region [hangfire]

            builder.Services.AddHangfire(config =>
                config.UseSqlServerStorage(() => new Microsoft.Data.SqlClient.SqlConnection(
                        builder.Configuration.GetConnectionString("HangfireConnection"))));
            builder.Services.AddHangfireServer();

            #endregion [hangfire]

            #region [dbcontext]
            builder.Services.AddTransient<ServicePipeline>();
            builder.Services
                .AddScoped<IDbContext, AppDbContext>()
                .AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
                    sqlServerOptions => sqlServerOptions.CommandTimeout(int.MaxValue))
                 );
            builder.Services.AddJinaDatabase<MsSqlProvider>(() =>
                new MsSqlProvider(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddFSql(builder.Configuration.GetConnectionString("DefaultConnection"));
            
            #endregion [dbcontext]

            #region [limiter]

            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = (int)HttpStatusCode.TooManyRequests;
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(httpContext =>
                {
                    if (httpContext.Request.Path.Value != null && httpContext.Request.Path.Value.Contains("/api/"))
                    {
                        var remoteIpAddress = httpContext?.Connection?.RemoteIpAddress;
                        if (IPAddress.IsLoopback(remoteIpAddress!) == false)
                        {
                            return RateLimitPartition.GetFixedWindowLimiter(remoteIpAddress,
                                _ => new FixedWindowRateLimiterOptions
                                {
                                    AutoReplenishment = true,
                                    PermitLimit = 10,
                                    Window = TimeSpan.FromSeconds(5),
                                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                    QueueLimit = 3
                                });
                        }
                    }
                    return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);
                });
            });

            #endregion [limiter]

            #region [signar]

            builder.Services.AddSignalR()
                // .AddStackExchangeRedis(builder.Configuration.GetConnectionString("RedisConnection"))
                ;
            
            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                      new[] { "application/octet-stream" });
            });

            #endregion [signar]

            #region [add controllers]

            builder.Services.AddControllers();

            #endregion

            #region [swagger]

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // add a custom operation filter which sets default values
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    // Description =
                    //     "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",                    
                    // Name = "Authorization",
                    // In = ParameterLocation.Header,
                    // Type = SecuritySchemeType.ApiKey,
                    // Scheme = "Bearer",
                    // BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"                    
                });
                
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            #endregion [swagger]

            #region [backgroundService]
            //builder.Services.AddHostedService<SaveExchangeRateBackgroundService>();
            #endregion

            #region [distribute cache]
            //기본 redis 설정, 만약 연결 문제 있을 경우 memorycache로 연결
            try
            {
                IConnectionMultiplexer connectionMultiplexer =
                    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("redisConnectionString"));
                builder.Services.AddSingleton(connectionMultiplexer);
                builder.Services.AddStackExchangeRedisCache(options =>
                    options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer)); 
            }
            catch
            {
                builder.Services.AddDistributedMemoryCache();    
            }

			//redis
			//builder.Services.AddStackExchangeRedisCache(options =>
			//{
			//    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
			//    options.InstanceName = "SampleInstance";
			//});
			//sql server
			//builder.Services.AddDistributedSqlServerCache(options =>
			//{
			//    options.ConnectionString = builder.Configuration.GetConnectionString(
			//        "SqlServerCacheConnection");
			//    options.SchemaName = "dbo";
			//    options.TableName = "CacheTable";
			//});
			#endregion

			#region [configuration]
			builder.Services.Configure<ApplicationConfig>(builder.Configuration.GetSection(nameof(ApplicationConfig)));
            builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection(nameof(EmailConfig)));
			#endregion

            #region [global exception handing]

            builder.Services.AddExceptionHandler<GlobalErrorHandler>();
            builder.Services.AddProblemDetails();
            
            #endregion

            #region [cache header]

            //builder.Services.AddHttpCacheHeaders();

            #endregion
            

			return builder;
        }

        private static bool IsOriginAllowed(string origin)
        {
            var uri = new Uri(origin);
            var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "n/a";
            var allowUrls = new[] { "example.com", "another-example.com", ".example.com" };
            var isAllowed = uri.Host.xContains(allowUrls);
            if (!isAllowed && env.Contains("DEV", StringComparison.OrdinalIgnoreCase))
                isAllowed = uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase);

            return isAllowed;
        }
    }
}