﻿using API.Contracts;
using API.DTOs.Employees;
using API.Models;

namespace API.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEducationRepository _educationRepository;
        private readonly IUniversityRepository _universityRepository;
        public EmployeeService(IEmployeeRepository employeeRepository, IUniversityRepository universityRepository, IEducationRepository educationRepository)
        {
            _employeeRepository = employeeRepository;
            _educationRepository = educationRepository;
            _universityRepository = universityRepository;
        }

        public IEnumerable<GetEmployeeDto>? GetEmployee()
        {
            var employees = _employeeRepository.GetAll();
            if (!employees.Any())
            {
                return null; // No employee  found
            }

            var toDto = employees.Select(employee =>
                                                new GetEmployeeDto
                                                {
                                                    Guid = employee.Guid,
                                                    Nik = employee.Nik,
                                                    BirthDate = employee.BirthDate,
                                                    Email = employee.Email,
                                                    FirstName = employee.FirstName,
                                                    LastName = employee.LastName,
                                                    Gender = employee.Gender,
                                                    HiringDate = employee.HiringDate,
                                                    PhoneNumber = employee.PhoneNumber
                                                }).ToList();

            return toDto; // employee found
        }

        public GetEmployeeDto? GetEmployee(Guid guid)
        {
            var employee = _employeeRepository.GetByGuid(guid);
            if (employee is null)
            {
                return null; // employee not found
            }

            var toDto = new GetEmployeeDto
            {
                Guid = employee.Guid,
                Nik = employee.Nik,
                BirthDate = employee.BirthDate,
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                HiringDate = employee.HiringDate,
                PhoneNumber = employee.PhoneNumber
            };

            return toDto; // employees found
        }

        public GetEmployeeDto? CreateEmployee(NewEmployeeDto newEmployeeDto)
        {
            var employee = new Employee
            {
                Guid = new Guid(),
                PhoneNumber = newEmployeeDto.PhoneNumber,
                FirstName = newEmployeeDto.FirstName,
                LastName = newEmployeeDto.LastName,
                Gender = newEmployeeDto.Gender,
                HiringDate = newEmployeeDto.HiringDate,
                Email = newEmployeeDto.Email,
                BirthDate = newEmployeeDto.BirthDate,
                Nik = newEmployeeDto.Nik,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var createdEmployee = _employeeRepository.Create(employee);
            if (createdEmployee is null)
            {
                return null; // employee not created
            }

            var toDto = new GetEmployeeDto
            {
                Guid = employee.Guid,
                Nik = employee.Nik,
                BirthDate = employee.BirthDate,
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                HiringDate = employee.HiringDate,
                PhoneNumber = employee.PhoneNumber
            };

            return toDto; // employee created
        }

        public int UpdateEmployee(UpdateEmployeeDto updateEmployeeDto)
        {
            var isExist = _employeeRepository.IsExist(updateEmployeeDto.Guid);
            if (!isExist)
            {
                return -1; // employee not found
            }

            var getEmployee = _employeeRepository.GetByGuid(updateEmployeeDto.Guid);

            var employee = new Employee
            {
                Guid = updateEmployeeDto.Guid,
                PhoneNumber = updateEmployeeDto.PhoneNumber,
                FirstName = updateEmployeeDto.FirstName,
                LastName = updateEmployeeDto.LastName,
                Gender = updateEmployeeDto.Gender,
                HiringDate = updateEmployeeDto.HiringDate,
                Email = updateEmployeeDto.Email,
                BirthDate = updateEmployeeDto.BirthDate,
                Nik = updateEmployeeDto.Nik,
                ModifiedDate = DateTime.Now,
                CreatedDate = getEmployee!.CreatedDate
            };

            var isUpdate = _employeeRepository.Update(employee);
            if (!isUpdate)
            {
                return 0; // employee not updated
            }

            return 1;
        }

        public int DeleteEmployee(Guid guid)
        {
            var isExist = _employeeRepository.IsExist(guid);
            if (!isExist)
            {
                return -1; // employee not found
            }

            var employee = _employeeRepository.GetByGuid(guid);
            var isDelete = _employeeRepository.Delete(employee!);
            if (!isDelete)
            {
                return 0; // employee not deleted
            }

            return 1;
        }

        public DetailEmployeeDto? GetMasterByGuid(Guid guid)
        {
            var master = GetMaster();
            var masterByGuid = master.FirstOrDefault(x => x.Guid == guid);
            return masterByGuid;
        }

        public IEnumerable<DetailEmployeeDto>? GetMaster()
        {
            var master = (from e in _employeeRepository.GetAll()
                          join education in _educationRepository.GetAll() on e.Guid equals education.Guid
                          join u in _universityRepository.GetAll() on education.UniversityGuid equals u.Guid
                          select new DetailEmployeeDto
                          {
                              Guid = e.Guid,
                              FullName = e.FirstName + " " + e.LastName,
                              NIK = e.Nik,
                              BirthDate = e.BirthDate,
                              Email = e.Email,
                              Gender = e.Gender,
                              HiringDate = e.HiringDate,
                              PhoneNumber = e.PhoneNumber,
                              Major = education.Major,
                              Degree = education.Degree,
                              GPA = education.Gpa,
                              UniversityName = u.Name
                          });

            if (!master.Any())
            {
                return null;
            }

            return master;
        }
    }
}
