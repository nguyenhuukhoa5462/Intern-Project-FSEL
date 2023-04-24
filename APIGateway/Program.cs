using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("APIGateway.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    //options.Authority = "https://localhost:5001";
    //options.Audience = "InternFsel";
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

app.MapGet("/", () => "Hello World!");
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();
await app.UseOcelot();
app.Run();
