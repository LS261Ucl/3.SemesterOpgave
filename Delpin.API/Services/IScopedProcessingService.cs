﻿using System.Threading.Tasks;

namespace Delpin.API.Services
{
    public interface IScopedProcessingService
    {
        Task DoWork();
    }
}