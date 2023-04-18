using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Application.Common.InfraServices;
using Vekalat.Core.Entities;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Vekalat.Application.Features
{
    public class SettingFeature
    {
        #region CQRS

        #region Edit Setting

        public class EditSettingCommand : IRequest
        {
            public Setting Setting { get; set; }
         }
        public class EditSettingCommandHandler : IRequestHandler<EditSettingCommand>
        {
            private readonly ISettingRepository _repository;
            public EditSettingCommandHandler(ISettingRepository repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(EditSettingCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    await _repository.Update(request.Setting);
                    await _repository.SaveChangesAsync();
                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Server Error", "خطا رخ داده است"));
                }
            }
        }

        #endregion

        
        #region Get Setting

        public class GetSettingQuery : IRequest<Setting>
        {
            public int Id { get; set; }
        }
        public class GetSettingQueryHandler : IRequestHandler<GetSettingQuery, Setting>
        {

            private readonly ISettingRepository _repository;

            public GetSettingQueryHandler(ISettingRepository repository)
            {
                _repository = repository;
            }

            public async Task<Setting> Handle(GetSettingQuery request, CancellationToken cancellationToken)
            {
                return await _repository.GetById(request.Id);
            }
        }

        #endregion

                

        #endregion


        #region Mapper And Repo

        public interface ISettingRepository : IRepository<Setting>
        {

           

        }

        public class SettingProfile : Profile
        {

        }

        #endregion


        #region Serialization

        public class SettingDto
        {
            public int Id { get; set; }
            public string tel { get; set; }
            public string about { get; set; }
            public string ContactUs { get; set; }
            public string ghavanin { get; set; }
            public string Address { get; set; }
        }



        #endregion


    }
}
