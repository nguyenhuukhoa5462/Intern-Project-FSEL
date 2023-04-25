using APICustomer.DatabaseContext;
using APICustomer.Repositories.IRepo;
using APICustomer.ViewModel.UserViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APICustomer.Repositories.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly CustomerDBContext _context;
        public UserRepo(CustomerDBContext context)
        {
            _context = context;
        }

        public async Task<string> Login(LoginModel model)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.UserName == model.UserName && p.Password == model.Password);
                if (user == null) return null;
                /*
                 *- Claim là một khái niệm trong xác thực người dùng, đại diện cho một thông tin xác thực về người dùng được lưu trữ trong hệ thống. Trong các hệ thống xác thực thông thường, người dùng sẽ được yêu cầu cung cấp một tập hợp các thông tin xác thực để chứng minh danh tính của họ và được phép truy cập vào các tài nguyên hoặc chức năng trong hệ thống. Các thông tin xác thực này được biểu diễn dưới dạng các claim.
                  - Mỗi claim được mô tả bằng một cặp key-value, trong đó key được gọi là ClaimType và value là giá trị của claim. Khi người dùng cung cấp các thông tin xác thực của mình, hệ thống sẽ kiểm tra các claim này để xác thực người dùng và quyết định các quyền truy cập của họ.
                */
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.UserName),
                new Claim(ClaimTypes.Name, user.FullName),
                /*
                 * JwtRegisteredClaimNames.Jti là một chuỗi hằng số trong thư viện xác thực JSON Web Token (JWT) của .NET, đại diện cho ClaimType "JWT ID" (JTI).
                   Claim JTI thường được sử dụng để đảm bảo tính duy nhất của JWT và phòng tránh việc sử dụng lại JWT sau khi nó đã hết hạn hoặc bị thu hồi. Mỗi JWT sẽ có một giá trị JTI duy nhất và giá trị này thường được tạo ngẫu nhiên.
                 */
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };
                /*
                 * SymmetricSecurityKey là một đối tượng trong .NET Framework được sử dụng để đại diện cho một khóa bí mật (secret key) được sử dụng để xác thực và mã hóa các thông điệp. Điều này có nghĩa là chúng ta sử dụng cùng một khóa để mã hóa và giải mã thông điệp. 

                    SymmetricSecurityKey thường được sử dụng để cấu hình các đối tượng như JwtSecurityTokenHandler hoặc EncryptionCredentials trong WCF (Windows Communication Foundation) để bảo vệ thông tin truyền trên mạng.
                 */
                var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsTheSecureKey1234567890"));

                /*
                 JwtSecurityToken là một đối tượng trong .NET Framework được sử dụng để biểu diễn cho một JSON Web Token (JWT). JWT là một chuỗi ký tự mã hóa bao gồm các phần thông tin xác thực và các chữ ký số để xác minh tính hợp lệ của token. JWT thường được sử dụng để xác thực và ủy quyền truy cập cho các ứng dụng web RESTful.

                 JwtSecurityToken cung cấp các thuộc tính để truy cập các phần khác nhau của JWT, bao gồm header, payload và signature. Nó cũng cho phép bạn kiểm tra tính hợp lệ của JWT bằng cách xác thực chữ ký số sử dụng một SymmetricSecurityKey hoặc AsymmetricSecurityKey.

                 JwtSecurityToken thường được sử dụng trong JwtSecurityTokenHandler để giải mã và xác thực JWT trong quá trình xử lý yêu cầu của ứng dụng web.
                 */
                var token = new JwtSecurityToken(
                    issuer: "https://localhost:5001",//người phát hành
                    audience: "InternFsel", //là một chuỗi đại diện cho người nhận dự kiến của JWT
                    expires: DateTime.Now.AddMinutes(20), //được sử dụng để chỉ định thời điểm hết hạn của JWT (JSON Web Token)
                    claims: authClaims, //là một thuộc tính được sử dụng để lưu trữ các thông tin về người dùng hoặc các quyền được ủy thác cho người dùng trong JWT
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature) //là một thuộc tính được sử dụng để chỉ định thông tin xác thực cho JWT, giúp đảm bảo rằng JWT được mã hóa và xác thực một cách an toàn trước khi được truyền đi giữa các bên.
                );

                return new JwtSecurityTokenHandler().WriteToken(token); //được sử dụng để mã hóa một đối tượng JWT thành một chuỗi ký tự. Khi bạn muốn truyền JWT từ phía server đến phía client, bạn cần sử dụng phương thức này để mã hóa JWT và gửi nó như một chuỗi (string) cho phía client.
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
