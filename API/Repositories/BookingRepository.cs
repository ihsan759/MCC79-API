using API.Contracts;
using API.Data;
using API.DTOs.Bookings;
using API.Models;

namespace API.Repositories
{
    public class BookingRepository : GeneralRepository<Booking>, IBookingRepository
    {
        public BookingRepository(BookingDbContext context) : base(context) { }

        // versi Repository
        public ICollection<BookingRoomToday>? GetByDateNow()
        {
            return _context.Set<Booking>()
               .Join(
                   _context.Set<Employee>(),
                   booking => booking.EmployeeGuid,
                   employee => employee.Guid,
                   (booking, employees) => new { Bookings = booking, Employees = employees }
               )
               .Join(
                   _context.Set<Room>(),
                   joinedData => joinedData.Bookings.RoomGuid,
                   room => room.Guid,
                   (joinedData, room) => new { joinedData.Employees, joinedData.Bookings, Rooms = room }
               )
               .Where(b => b.Bookings.StartDate <= DateTime.Now).Where(b => b.Bookings.EndDate >= DateTime.Now)
               .Select(joinData => new BookingRoomToday
               {

                   BookingGuid = joinData.Bookings.Guid,
                   RoomName = joinData.Rooms.Name,
                   Status = joinData.Bookings.Status,
                   Floor = joinData.Rooms.Floor,
                   BookedBy = joinData.Employees.FirstName + " " + joinData.Employees.LastName
               })
               .ToList();
        }
    }
}
