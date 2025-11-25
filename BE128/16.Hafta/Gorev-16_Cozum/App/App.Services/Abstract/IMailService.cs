using App.Models.DTO.Mail;
using Ardalis.Result;

namespace App.Services.Abstract
{
    public interface IMailService
    {
        Task<Result> SendAsync(MailSendRequest request);
    }
}
