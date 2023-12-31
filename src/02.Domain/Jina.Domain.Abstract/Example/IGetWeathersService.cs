﻿using Jina.Base.Service.Abstract;
using Jina.Domain.Example;
using Jina.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Abstract.Example
{
    public interface IGetWeathersService : IServiceImplBase<PaginatedRequest<WeatherForecastDto>, PaginatedResult<WeatherForecastDto>>
    {
    }
}