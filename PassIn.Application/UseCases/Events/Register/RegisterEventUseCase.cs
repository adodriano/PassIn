using PassIn.Communication.Requests;

namespace PassIn.Application.UseCases.Events.Register
{
    public class RegisterEventUseCase
    {
        public void Execute(RequestEventJson request)
        {
            Validate(request);
        }

        public void Validate(RequestEventJson request)
        {
            if (request.MaximumAttendees <= 0)
            {
                throw new ArgumentNullException("The Maximum atendees is invalid.");
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ArgumentNullException("The title is invalid.");
            }

            if (string.IsNullOrWhiteSpace(request.Details))
            {
                throw new ArgumentNullException("The Details is invalid.");
            }
        }
    }
}
