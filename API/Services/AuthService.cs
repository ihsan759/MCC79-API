using API.Contracts;
using API.DTOs.Auth;
using API.Models;
using API.Utilities;
using System.Security.Claims;

namespace API.Services
{
    public class AuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IEducationRepository _educationRepository;
        private readonly ITokenHandler _tokenHandler;
        private readonly IAccountRoleRepository _accountRoleRepository;
        private readonly IRoleRepository _roleRepository;

        public AuthService(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IUniversityRepository universityRepository, IEducationRepository educationRepository, ITokenHandler tokenHandler, IAccountRoleRepository accountRoleRepository, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _employeeRepository = employeeRepository;
            _universityRepository = universityRepository;
            _educationRepository = educationRepository;
            _tokenHandler = tokenHandler;
            _accountRoleRepository = accountRoleRepository;
            _roleRepository = roleRepository;
        }

        public string Login(LoginDto loginDto)
        {
            var employee = _employeeRepository.CheckEmail(loginDto.Email);
            if (employee is null)
            {
                return "0";
            }

            var account = _accountRepository.GetByGuid(employee.Guid);
            if (account is null)
            {
                return "0";
            }

            if (!Hashing.ValidatePassword(loginDto.Password, account.Password))
            {
                return "-1";
            }

            var accountRole = _accountRoleRepository.GetByGuidEmployee(account.Guid);


            var claims = new List<Claim>() {
                new Claim("NIK", employee.Nik),
                new Claim("FullName", $"{employee.FirstName} {employee.LastName}"),
                new Claim("Email", loginDto.Email)
            };

            claims.AddRange(accountRole.Select(role => new Claim("Role", role.RoleGuid.ToString())));

            try
            {
                var getToken = _tokenHandler.GenerateToken(claims);
                return getToken;
            }
            catch
            {
                return "-2";
            }
        }

        public RegisterDto Register(RegisterDto registerDto)
        {
            try
            {

                var role = _roleRepository.GetByName("User");

                if (role is null)
                {
                    return null;
                }

                var employee = new Employee
                {
                    Guid = new Guid(),
                    PhoneNumber = registerDto.PhoneNumber,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName ?? "",
                    Gender = registerDto.Gender,
                    HiringDate = registerDto.HiringDate,
                    Email = registerDto.Email,
                    BirthDate = registerDto.BirthDate,
                    Nik = GenericNik(),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                var createdEmployee = _employeeRepository.Create(employee);

                var account = new Account
                {
                    Guid = createdEmployee.Guid,
                    IsDeleted = false,
                    IsUsed = true,
                    Otp = 0,
                    ExpiredTime = DateTime.Now,
                    Password = Hashing.HashPassword(registerDto.Password),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                var createdAccount = _accountRepository.Create(account);
                var university = new University
                {
                    Guid = new Guid(),
                    Code = registerDto.UniversityCode,
                    Name = registerDto.UniversityName,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                var createdUniversity = _universityRepository.Create(university);

                var education = new Education
                {
                    Guid = createdEmployee.Guid,
                    Major = registerDto.Major,
                    Degree = registerDto.Degree,
                    Gpa = registerDto.Gpa,
                    UniversityGuid = university.Guid,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                var createdEducation = _educationRepository.Create(education);

                var accountRole = new AccountRole
                {
                    Guid = new Guid(),
                    AccountGuid = createdAccount.Guid,
                    RoleGuid = role.Guid
                };
                var createdAccountRole = _accountRoleRepository.Create(accountRole);

                var toDto = new RegisterDto
                {
                    FirstName = createdEmployee.FirstName,
                    LastName = createdEmployee.LastName,
                    BirthDate = createdEmployee.BirthDate,
                    Gender = createdEmployee.Gender,
                    HiringDate = createdEmployee.HiringDate,
                    Email = createdEmployee.Email,
                    PhoneNumber = createdEmployee.PhoneNumber,
                    Major = createdEducation.Major,
                    Degree = createdEducation.Degree,
                    Gpa = createdEducation.Gpa,
                    UniversityCode = createdUniversity.Code,
                    UniversityName = createdUniversity.Name,
                    Password = createdAccount.Password,
                    ConfirmPassword = createdAccount.Password
                };
                return toDto;
            }
            catch
            {
                return null;
            }

        }

        public string GenericNik()
        {
            var employees = _employeeRepository.GetAll().OrderBy(e => e.Nik).LastOrDefault();
            if (employees is null)
            {
                return "111111";
            }

            var nik = int.Parse(employees.Nik) + 1;

            return nik.ToString();
        }

        public int ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var isExist = _employeeRepository.CheckEmail(changePasswordDto.Email);
            if (isExist is null)
            {
                return -1; // Account not found
            }

            var getAccount = _accountRepository.GetByGuid(isExist.Guid);
            if (getAccount.Otp != changePasswordDto.Otp)
            {
                return 0;
            }

            if (getAccount.IsUsed == true)
            {
                return 1;
            }

            if (getAccount.ExpiredTime < DateTime.Now)
            {
                return 2;
            }

            var account = new Account
            {
                Guid = getAccount.Guid,
                IsUsed = getAccount.IsUsed,
                IsDeleted = getAccount.IsDeleted,
                ModifiedDate = DateTime.Now,
                CreatedDate = getAccount!.CreatedDate,
                Otp = getAccount.Otp,
                ExpiredTime = getAccount.ExpiredTime,
                Password = Hashing.HashPassword(changePasswordDto.NewPassword),
            };

            var isUpdate = _accountRepository.Update(account);
            if (!isUpdate)
            {
                return 0; // Account not updated
            }

            return 3;
        }
    }
}
