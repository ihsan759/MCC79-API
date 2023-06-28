using API.Contracts;
using API.DTOs.Auth;
using API.Models;
using API.Utilities;

namespace API.Services
{
    public class AuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IEducationRepository _educationRepository;

        public AuthService(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IUniversityRepository universityRepository, IEducationRepository educationRepository)
        {
            _accountRepository = accountRepository;
            _employeeRepository = employeeRepository;
            _universityRepository = universityRepository;
            _educationRepository = educationRepository;
        }

        public RegisterDto Register(RegisterDto registerDto)
        {
            try
            {
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
    }
}
