using APIOder.DatabaseContext;
using APIOder.Repositories.IRepo;
using APIOder.Repositories.Repo;
using APIOder.Services.IService;
using APIOder.ViewModel.JWT_Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Refit;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

var jwtTokenConfig = Configuration.GetSection("JwtTokenConfig").Get<JwtTokenConfig>();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthenticatedHttpClientHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRefitClient<IOderService>()
        .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:5001"))
        .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();
builder.Services.AddHttpClient("myHttpClient", client =>
{
    // Các thiết lập của HttpClient
    client.BaseAddress = new Uri("https://localhost:6001");
});



builder.Services.AddDbContext<OderDBContext>(c => c.UseSqlServer("Server=DESKTOP-T4L1DE8\\SQLEXPRESS;Database=Fsel-Oder;Trusted_Connection=True;"));

builder.Services.AddTransient<IOderRepo, OderRepo>();
builder.Services.AddTransient<IOderDetailRepo, OderDetailRepo>();

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    //options.Authority = "https://localhost:5001/Login/Login"; // Điền địa chỉ API của Auth, nơi cung cấp token
    //options.Audience = "your-api-identifier"; // Điền identifier của API được bảo vệ bởi token
    options.RequireHttpsMetadata = false;//tắt yêu cầu https khi giao tiếp giữa client và server(tắt khi chạy ở localhost, nên bật khi chạy ở môi trường sản phẩm để bảo vệ thông tin truyền tải
    options.SaveToken = true; //được sử dụng để lưu trữ JWT token trong HttpContext sau khi nó được xác thực.
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, //xác định liệu issuer của token có hợp lệ hay không
        ValidateAudience = true, //xác định liệu audience của token có hợp lệ hay không
        ValidAudience = "InternFsel", //tên của audience được cho phép
        ValidIssuer = "https://localhost:5001", //tên của issuer được cho phép
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsTheSecureKey1234567890")), //khóa bí mật để xác minh tính hợp lệ của token
        ValidateIssuerSigningKey = true,
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.Use(async (context, next) =>
//{
//    // Lấy token từ cookie hoặc từ request headers
//    var token = context.Request.Cookies["access_token"] ?? context.Request.Headers["Authorization"];

//    if (!string.IsNullOrEmpty(token))
//    {
//        // Add token vào header của request
//        context.Request.Headers.Add("Authorization", "Bearer " + token);
//    }

//    await next();
//});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
