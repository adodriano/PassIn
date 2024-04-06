using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Attendees.GetAllByEventId
{
    public class GetAllAttendeesByEventIdUseCase
    {
        private readonly PassInDbContext _dbContext;

        public GetAllAttendeesByEventIdUseCase()
        {
            _dbContext = new PassInDbContext();
        }
        public ResponseAllAttendeesJson Execute(Guid eventId)
        {
           var entity = _dbContext.Events.Include(e => e.Attendees).ThenInclude(e => e.CheckIn).FirstOrDefault(e => e.Id == eventId);
            if (entity is null)
                throw new NotFoundException("An event with this id dont exist.");

            return new ResponseAllAttendeesJson()
            {
                Attendees = entity.Attendees.Select(attendee => new ResponseAttendeeJson
                {
                    Id = eventId,
                    Name = attendee.Name,
                    Email = attendee.Email,
                    CreatedAt = attendee.Created_At,
                    CheckedInAt = attendee.CheckIn?.Created_At               
                }).ToList()
            };
        }
    }
}
