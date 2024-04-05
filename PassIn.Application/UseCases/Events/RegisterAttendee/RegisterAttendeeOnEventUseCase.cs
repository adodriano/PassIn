using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendee
{
    public class RegisterAttendeeOnEventUseCase
    {
        private readonly PassInDbContext _dbContext;
        public RegisterAttendeeOnEventUseCase()
        {
            _dbContext = new PassInDbContext();
        }
        public ResponseRegisteredJson Execute(Guid eventId, RequestRegisterEventJson request)
        {
            Validate(eventId, request);

            var entity = new Infrastructure.Entities.Attendee
            {
                Name = request.Name,
                Email = request.Email,
                Event_Id = eventId,
                Created_At = DateTime.UtcNow
            };

            _dbContext.Attendees.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseRegisteredJson
            {
                Id = entity.Id
            };
        }

        private void Validate(Guid eventId, RequestRegisterEventJson request)
        {
            var eventExist = _dbContext.Events.Any(x => x.Id == eventId);

            if (eventExist == false)
            {
                throw new NotFoundException("An event with this id dont exist.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ErrorOnValidationException("The Name is invalid.");
            }

            if (EmailIsValid(request.Email) == false)
            {
                throw new ErrorOnValidationException("The Email is invalid.");
            }

            var attendeeAlreadyRegistred = _dbContext
                .Attendees
                .Any(x => x.Email.Equals(request.Email) && x.Event_Id == eventId);

            if (attendeeAlreadyRegistred)
            {
                throw new ErrorOnValidationException("You can not register twice on the same event.");
            }
        }

        public bool EmailIsValid(string email)
        {
            try
            {
                new MailAddress(email);

                return true;
            }
            catch
            {

                return false;
            }

        }
    }
}
