using SasiadMa.Core.Common;

namespace SasiadMa.Application.Interfaces;

public interface IEmailService
{
    Task<Result<bool>> SendEmailConfirmationAsync(string email, string firstName, string confirmationToken);
    Task<Result<bool>> SendBorrowRequestNotificationAsync(string ownerEmail, string borrowerName, string itemName);
    Task<Result<bool>> SendBorrowApprovalNotificationAsync(string borrowerEmail, string ownerName, string itemName);
    Task<Result<bool>> SendReturnReminderAsync(string borrowerEmail, string itemName, DateTime dueDate);
}
