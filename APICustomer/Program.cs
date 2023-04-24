using APICustomer.DatabaseContext;
using APICustomer.Repositories.IRepo;
using APICustomer.Repositories.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("myHttpClient", client =>
{
    // Các thiết lập của HttpClient
    client.BaseAddress = new Uri("https://localhost:5001");
});
builder.Services.AddDbContext<CustomerDBContext>(c => c.UseSqlServer("Server=DESKTOP-T4L1DE8\\SQLEXPRESS;Database=Fsel-Customer;Trusted_Connection=True;"));

builder.Services.AddTransient<IUserRepo, UserRepo>();
builder.Services.AddTransient<ICustomerRepo, CustomerRepo>();

//AddAuthentication là một phương thức trong ASP.NET Core để xác thực người dùng và sự cho phép truy cập vào các tài nguyên của ứng dụng web. Nó được sử dụng để thiết lập và cấu hình các chính sách xác thực và phương thức xác thực, như cookie, token JWT (JSON Web Token), OpenID Connect và OAuth.
/*
 JwtBearerDefaults.AuthenticationScheme là một hằng số chuỗi trong ASP.NET Core, được sử dụng để định danh phương thức xác thực JWT (JSON Web Token) trong việc cấu hình xác thực trong ứng dụng web.

Trong ASP.NET Core, các phương thức xác thực được định danh bằng tên gọi của chúng. JwtBearerDefaults.AuthenticationScheme là tên định danh cho phương thức xác thực sử dụng JWT Bearer.
 */
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
