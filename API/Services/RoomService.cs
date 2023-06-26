using API.Contracts;
using API.DTOs.Rooms;
using API.Models;

namespace API.Services
{
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public IEnumerable<GetRoomDto>? GetRoom()
        {
            var rooms = _roomRepository.GetAll();
            if (!rooms.Any())
            {
                return null; // No room  found
            }

            var toDto = rooms.Select(room =>
                                                new GetRoomDto
                                                {
                                                    Guid = room.Guid,
                                                    Name = room.Name,
                                                    Capacity = room.Capacity,
                                                    Floor = room.Floor,
                                                }).ToList();

            return toDto; // room found
        }

        public GetRoomDto? GetRoom(Guid guid)
        {
            var room = _roomRepository.GetByGuid(guid);
            if (room is null)
            {
                return null; // room not found
            }

            var toDto = new GetRoomDto
            {
                Guid = room.Guid,
                Name = room.Name,
                Capacity = room.Capacity,
                Floor = room.Floor,
            };

            return toDto; // rooms found
        }

        public GetRoomDto? CreateRoom(NewRoomDto newRoomDto)
        {
            var room = new Room
            {
                Guid = new Guid(),
                Name = newRoomDto.Name,
                Capacity = newRoomDto.Capacity,
                Floor = newRoomDto.Floor,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var createdRoom = _roomRepository.Create(room);
            if (createdRoom is null)
            {
                return null; // room not created
            }

            var toDto = new GetRoomDto
            {
                Guid = room.Guid,
                Name = room.Name,
                Capacity = room.Capacity,
                Floor = room.Floor,
            };

            return toDto; // room created
        }

        public int UpdateRoom(UpdateRoomDto updateRoomDto)
        {
            var isExist = _roomRepository.IsExist(updateRoomDto.Guid);
            if (!isExist)
            {
                return -1; // room not found
            }

            var getRole = _roomRepository.GetByGuid(updateRoomDto.Guid);

            var room = new Room
            {
                Guid = updateRoomDto.Guid,
                Name = updateRoomDto.Name,
                Capacity = updateRoomDto.Capacity,
                Floor = updateRoomDto.Floor,
                ModifiedDate = DateTime.Now,
                CreatedDate = getRole!.CreatedDate
            };

            var isUpdate = _roomRepository.Update(room);
            if (!isUpdate)
            {
                return 0; // room not updated
            }

            return 1;
        }

        public int DeleteRoom(Guid guid)
        {
            var isExist = _roomRepository.IsExist(guid);
            if (!isExist)
            {
                return -1; // room not found
            }

            var room = _roomRepository.GetByGuid(guid);
            var isDelete = _roomRepository.Delete(room!);
            if (!isDelete)
            {
                return 0; // room not deleted
            }

            return 1;
        }
    }
}
