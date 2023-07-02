using API.Contracts;
using API.DTOs.Bookings;
using API.Models;

namespace API.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingService(IBookingRepository bookingRepository, IEmployeeRepository employeeRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _employeeRepository = employeeRepository;
            _roomRepository = roomRepository;
        }

        public IEnumerable<GetBookingDto>? GetBooking()
        {
            var bookings = _bookingRepository.GetAll();
            if (!bookings.Any())
            {
                return null; // No Booking  found
            }

            var toDto = bookings.Select(booking =>
                                                new GetBookingDto
                                                {
                                                    Guid = booking.Guid,
                                                    StartDate = booking.StartDate,
                                                    EndDate = booking.EndDate,
                                                    Status = booking.Status,
                                                    Remarks = booking.Remarks,
                                                    RoomGuid = booking.RoomGuid,
                                                    EmployeeGuid = booking.EmployeeGuid
                                                }).ToList();

            return toDto; // Booking found
        }

        public GetBookingDto? GetBooking(Guid guid)
        {
            var booking = _bookingRepository.GetByGuid(guid);
            if (booking is null)
            {
                return null; // booking not found
            }

            var toDto = new GetBookingDto
            {
                Guid = booking.Guid,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Status = booking.Status,
                Remarks = booking.Remarks,
                RoomGuid = booking.RoomGuid,
                EmployeeGuid = booking.EmployeeGuid
            };

            return toDto; // bookings found
        }

        public GetBookingDto? CreateBooking(NewBookingDto newBookingDto)
        {
            var booking = new Booking
            {
                Guid = new Guid(),
                StartDate = newBookingDto.StartDate,
                EndDate = newBookingDto.EndDate,
                Status = newBookingDto.Status,
                Remarks = newBookingDto.Remarks,
                RoomGuid = newBookingDto.RoomGuid,
                EmployeeGuid = newBookingDto.EmployeeGuid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var createdBooking = _bookingRepository.Create(booking);
            if (createdBooking is null)
            {
                return null; // Booking not created
            }

            var toDto = new GetBookingDto
            {
                Guid = createdBooking.Guid,
                StartDate = newBookingDto.StartDate,
                EndDate = newBookingDto.EndDate,
                Status = newBookingDto.Status,
                Remarks = newBookingDto.Remarks,
                RoomGuid = newBookingDto.RoomGuid,
                EmployeeGuid = newBookingDto.EmployeeGuid,
            };

            return toDto; // Booking created
        }

        public int UpdateBooking(UpdateBookingDto updateBookingDto)
        {
            var isExist = _bookingRepository.IsExist(updateBookingDto.Guid);
            if (!isExist)
            {
                return -1; // Booking not found
            }

            var getBooking = _bookingRepository.GetByGuid(updateBookingDto.Guid);

            var booking = new Booking
            {
                Guid = updateBookingDto.Guid,
                StartDate = updateBookingDto.StartDate,
                EndDate = updateBookingDto.EndDate,
                Status = updateBookingDto.Status,
                Remarks = updateBookingDto.Remarks,
                RoomGuid = updateBookingDto.RoomGuid,
                EmployeeGuid = updateBookingDto.EmployeeGuid,
                ModifiedDate = DateTime.Now,
                CreatedDate = getBooking!.CreatedDate
            };

            var isUpdate = _bookingRepository.Update(booking);
            if (!isUpdate)
            {
                return 0; // Booking not updated
            }

            return 1;
        }

        public int DeleteBooking(Guid guid)
        {
            var isExist = _bookingRepository.IsExist(guid);
            if (!isExist)
            {
                return -1; // Booking not found
            }

            var booking = _bookingRepository.GetByGuid(guid);
            var isDelete = _bookingRepository.Delete(booking!);
            if (!isDelete)
            {
                return 0; // Booking not deleted
            }

            return 1;
        }

        public IEnumerable<BookingRoomToday> BookingNow()
        {
            var bookings = _bookingRepository.GetAll();
            if (bookings == null)
            {
                return null; // No Booking  found
            }


            // versi LINQ
            var employees = _employeeRepository.GetAll();
            var rooms = _roomRepository.GetAll();

            var bookingNow = (
                from booking in bookings
                join employee in employees on booking.EmployeeGuid equals employee.Guid
                join room in rooms on booking.RoomGuid equals room.Guid
                where booking.StartDate <= DateTime.Now.Date && booking.EndDate >= DateTime.Now
                select new BookingRoomToday
                {
                    BookingGuid = booking.Guid,
                    RoomName = room.Name,
                    Status = booking.Status,
                    Floor = room.Floor,
                    BookedBy = employee.FirstName + " " + employee.LastName,
                }
            ).ToList();

            return bookingNow;
        }

        public ICollection<BookingDetailsDto>? GetBookingDetails()
        {
            var bookings = _bookingRepository.GetAll();
            if (bookings == null)
            {
                return null; // No Booking  found
            }

            var employees = _employeeRepository.GetAll();
            var rooms = _roomRepository.GetAll();

            var getBookingDetails = (
                from booking in bookings
                join employee in employees on booking.EmployeeGuid equals employee.Guid
                join room in rooms on booking.RoomGuid equals room.Guid
                select new BookingDetailsDto
                {
                    Guid = booking.Guid,
                    BookedNik = employee.Nik,
                    BookedBy = employee.FirstName + " " + employee.LastName,
                    RoomName = room.Name,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    Status = booking.Status,
                    Remarks = booking.Remarks
                }
            ).ToList();

            return getBookingDetails;
        }

        public BookingDetailsDto? GetBookingDetailsByGuid(Guid Guid)
        {
            return GetBookingDetails()?.FirstOrDefault(x => x.Guid == Guid);
        }

        public IEnumerable<BookingLengthDto> BookingDuration()
        {
            var bookings = _bookingRepository.GetAll();
            if (bookings == null)
            {
                return null;
            }

            var rooms = _roomRepository.GetAll();

            var entities = (from booking in bookings
                            join room in rooms on booking.RoomGuid equals room.Guid
                            select new
                            {
                                Guid = room.Guid,
                                StartDate = booking.StartDate,
                                EndDate = booking.EndDate,
                                RoomName = room.Name,
                            }).ToList();

            var bookingDurations = new List<BookingLengthDto>();

            foreach (var entity in entities)
            {
                TimeSpan duration = entity.EndDate - entity.StartDate;

                int totalDays = (int)duration.TotalDays;
                int weekends = 0;

                for (int i = 0; i < totalDays; i++)
                {
                    var currentDate = entity.StartDate.AddDays(i);
                    if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        weekends++;
                    }
                }

                TimeSpan bookingLenght = duration - TimeSpan.FromDays(weekends);

                var bookingDuration = new BookingLengthDto
                {
                    RoomGuid = entity.Guid,
                    RoomName = entity.RoomName,
                    BookingLenght = bookingLenght
                };

                bookingDurations.Add(bookingDuration);

            }

            return bookingDurations;

        }
    }
}
