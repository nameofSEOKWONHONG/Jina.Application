using Jina.Base.Service.Abstract;
using Jina.Domain.Example;
using Jina.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Abstract.Example
{
    public interface IGetWeathersService : IServiceImplBase<PaginatedRequest<WeatherForecastRequest>, PaginatedResult<WeatherForecastRequest>>
        , IScopeService
    {
    }
}