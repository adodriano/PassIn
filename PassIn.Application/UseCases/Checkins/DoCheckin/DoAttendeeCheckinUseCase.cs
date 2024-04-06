
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Checkins.DoCheckin
{
    public class DoAttendeeCheckinUseCase
    {
        private readonly PassInDbContext _dbContext;

        public DoAttendeeCheckinUseCase()
        {
            _dbContext = new PassInDbContext();
        }

        public ResponseRegisteredJson Execute(Guid attendeeId)
        {
            validate(attendeeId);

            var entity = new CheckIn
            {    
                Created_At = DateTime.Now,
                Attendee_Id = attendeeId,
            };

            _dbContext.CheckIns.Add(entity);

            _dbContext.SaveChanges();

            return new ResponseRegisteredJson
            {
                Id = entity.Id
            };

        }

        public void validate(Guid attendeeId)
        {
            var existAttendee = _dbContext.Attendees.Any(x => x.Id == attendeeId);
            if (existAttendee == false)
            {
                throw new NotFoundException("The Attendee with id was not found");
            }

            var existCheckin = _dbContext.CheckIns.Any(x => x.Attendee_Id == attendeeId);
            if (existCheckin)
            {
                throw new ConflictException("The Attendee not do checking twice in the same event");
            }


        }
    }
}
