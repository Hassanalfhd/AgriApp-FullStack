using Agricultural_For_CV.Middleware;
using Agricultural_For_CV_DAL;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Settings;
using Agricultural_For_CV_BLL.Services;
using Agricultural_For_CV_DAL.Repositories;
using Agricultural_For_CV_DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Agricultural_For_CV_BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Agricultural_For_CV.Authorization;
using MassTransit;
using Agricultural_For_CV.Events;

var builder = WebApplication.CreateBuilder(args);


//// 1. define  CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AgriculturalApiCorsPolicy",
        policy =>
        {
            policy
            .WithOrigins("https://localhost:7170", "http://localhost:5173", "http://localhost:5171") // رابط واجهة React
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});



//MassTransit Settings
//builder.Services.AddMassTransit(x =>
//{

//    // 1. تسجيل الـ Consumer
//    x.AddConsumer<OrderPlacedConsumer>();

//    // 2. ضبط الـ Outbox والـ Inbox مع Entity Framework
//    x.AddEntityFrameworkOutbox<AppDbContext>(o =>
//    {
//        o.UseSqlServer();
//        o.UseBusOutbox(); // تفعيل الـ Outbox للمرسل
//    });

//    x.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host("localhost", "/", h =>
//        {
//            h.Username("guest");
//            h.Password("guest");
//        });

//        cfg.ConfigureEndpoints(context);

//    });

    
//});



// --- Logger Configuration ---
// تنظيف المزودين الافتراضيين
builder.Logging.ClearProviders();

// 1. Console Logger - لعرض جميع الرسائل من Information وما فوق
builder.Logging.AddConsole(options =>
{
    options.TimestampFormat = "[HH:mm:ss] ";
    options.IncludeScopes = true;
});


// 2. Debug Logger - فقط للأخطاء لتحليل الأداء في التطوير
builder.Logging.AddDebug();

// 3. EventLog Logger - في Windows Event Viewer للأخطاء والتحذيرات فقط
builder.Logging.AddEventLog(settings =>
{
    settings.SourceName = "AgriculturalApp"; // اسم التطبيق في Event Viewer
    settings.Filter = (source, logLevel) =>
        logLevel >= LogLevel.Warning; // فقط التحذيرات والأخطاء
});





//Get AppSettings Section from appSettings.json

var appSettingsSection = builder.Configuration.GetSection("AppSettings");

builder.Services.Configure<AppSettings>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSettings>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // 



// 2. Register EF Core DbContext
builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseSqlServer(connectionString, b => b.MigrationsAssembly("Agricultural_For_CV_DAL"));
});

// Get The SecretKey by Encoded it
var key = Encoding.ASCII.GetBytes(appSettings!.SecretKey); // to Encoding  the Token



// Authentication
// Add the Authentication Service and notified the system that we use [Authorization:Bearer <token>] 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    // Add JWT settings  
 .AddJwtBearer(options =>
 {

     options.RequireHttpsMetadata = false; // This property determines whether the authentication middleware requires HTTPS if it true (if set to true, only HTTPS requests are accepted)
     options.SaveToken = true; // This tells the authentication system to save the JWT token in the HttpContext.User
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true, //Checks if the token was issued by a trusted source
         ValidateAudience = true, //Ensures the token is intended for your application or specific users
         ValidateIssuerSigningKey = true, // Verifies the token’s signature using your secret key to ensure it hasn’t been tampered with
         ValidateLifetime = true,
         ValidIssuer = appSettings.Issuer,
         ValidAudience = appSettings.Audience,
         IssuerSigningKey = new SymmetricSecurityKey(key),
         ClockSkew = TimeSpan.Zero

     };
 });




builder.Services.AddAuthentication();


// AuthorizationHandler Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomerOwnerOrAdmin", policy =>
    {
        policy.Requirements.Add(new CustomerOwnerOrAdminRequirement());
    });

    options.AddPolicy("FarmerOwnerOrAdmin", policy =>
    {
        policy.Requirements.Add(new FarmerOwnerOrAdminRequirement());
    });

});

builder.Services.AddSingleton<IAuthorizationHandler, FarmerOwnerOrAdminHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, CustomerOwnerOrAdminHandler>();



// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Agricultural API",
        Version = "v1",
        Description = "An ASP.Net Core Web API for managing agricultural data",
        Contact = new OpenApiContact
        {
            Name = "Hasan Ameen Hasan Al-fahad",
            Email = "alfahdhassan42@gmail.com",
            // TODO: GitHub  Url = ""
        }
    });


    // XML File (Documentation Comments)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
    else
    {
        // يمكن تسجيل تحذير في الـ console لتعرف أن الملف غير موجود
        Console.WriteLine($"XML documentation file not found: {xmlPath}");
    }

    
    //Authorization (JWT)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token"

    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {

        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },

            },

            Array.Empty<string>()
        }
    });

    c.OperationFilter<SwaggerFileOperationFilter>();
});


builder.Services.AddHealthChecks();

// 2. DbContext -- DAL Layer 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Repositories -- DAL Layer 
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICropRepository, CropRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();


// 4. Services -- BLL Layer 
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICropService, CropService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IFarmerService, FarmerService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<IReportService, ReportService>();

// 5. Controllers + API Versioning
builder.Services.AddControllers();

//API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});



// 6. Rate Limiting 

// per token(User Id)  and  IP address -- we user token(User Id)   for more security   
builder.Services.AddRateLimiter(options =>
{

    options.AddPolicy("UserRateLimit", context =>
    {
        string userKey;

        // if user is login IsAuthenticated by token 
        if (context.User?.Identity?.IsAuthenticated == true)
        {
    
            var userId = context.User.FindFirst("sub")?.Value
                                    ?? context.User.FindFirst("id")?.Value
                                    ?? context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            userKey = !string.IsNullOrEmpty(userId) ? $"token:{userId}" : $"token:unknown";
        
        }
        // if user(unknown) is not login IsAuthenticated by ip 
        else
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            userKey = $"ip:{ip}";
        }


        return RateLimitPartition.GetSlidingWindowLimiter(userKey, _ => new SlidingWindowRateLimiterOptions
        {
            PermitLimit = 40,
            Window = TimeSpan.FromMinutes(1),
            SegmentsPerWindow = 10,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        });
    });

    options.AddPolicy("AuthLimiter", HttpContext =>
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });


    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync("❌ لقد تجاوزت الحد المسموح به من الطلبات. حاول بعد قليل.");
    };
});


builder.Services.AddHttpContextAccessor();


var app = builder.Build();



// for navigate in project  and EF  know  how to access all Url
// 7. Middleware

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<ProfilingMiddleware>();
app.UseHttpsRedirection();// HTTP --> HTTPS
app.UseRouting();
app.UseCors("AgriculturalApiCorsPolicy");//// Enable CORS
app.UseRateLimiter(); 
app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();


// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Download Static Files like (Custom.css)

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agricultural API v1");

        c.RoutePrefix = string.Empty;

        c.InjectStylesheet("/swagger-ui/custom.css");
    });
}



// Map Controllers
app.MapControllers().RequireRateLimiting("UserRateLimit");


app.Run();
