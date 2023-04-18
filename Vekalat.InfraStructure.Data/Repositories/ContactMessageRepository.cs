using Vekalat.Application.Common.Helpers;
using Vekalat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using static Vekalat.Application.Features.ContactMessageFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class ContactMessageRepository : Repository<ContactMessage>, IContactMessageRepository
    {
        private readonly VekalatDataContext _context;
        public ContactMessageRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<ContactMessage> GetKetabdaryMessagesForAdmin(string sender, int messageType, int messageStatusType, string startSendDate, string endSendDate, string startAnswerDate, string endAnswerDate, int sortOrder)
        {
            var result = _context.ContactMessages.AsQueryable();
            if (!string.IsNullOrEmpty(sender))
            {
                result = result.Where(s => EF.Functions.Like(s.Name, $"%{sender}%"));
            }
            if (messageStatusType != -1)
            {
                result = result.Where(s => s.IsRespone == Convert.ToBoolean(messageStatusType));
            }

            //if (!string.IsNullOrEmpty(startSendDate))
            //{
            //    result = result.Where(entry => string.Compare(entry.Fdate, startSendDate.ToLatingDigit()) >= 0);
            //}
            //if (!string.IsNullOrEmpty(endSendDate))
            //{
            //    result = result.Where(entry => string.Compare(entry.Fdate, endSendDate.ToLatingDigit()) <= 0);
            //}
            //if (!string.IsNullOrEmpty(startAnswerDate))
            //{
            //    result = result.Where(entry => string.Compare(entry.Rdate, startAnswerDate.ToLatingDigit()) >= 0);
            //}
            //if (!string.IsNullOrEmpty(endAnswerDate))
            //{
            //    result = result.Where(entry => string.Compare(entry.Rdate, endAnswerDate.ToLatingDigit()) <= 0);
            //}

            if (sortOrder == 2)
                return result.OrderByDescending(c => c.Id);
            else
                return result.OrderBy(c => c.Id);
        }
    }
}
