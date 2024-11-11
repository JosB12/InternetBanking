﻿using InternetBanking.Core.Application.Interfaces.Services.Generic;
using InternetBanking.Core.Application.ViewModels.Beneficiario;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IBeneficiarioService : IGenericService<SaveBeneficiarioViewModel, BeneficiarioViewModel, Beneficiarios>
    {
        Task<BeneficiarioViewModel> AddBeneficiarioAsync(SaveBeneficiarioViewModel vm);
        Task<List<BeneficiarioViewModel>> GetBeneficiariosAsync();
        Task DeleteBeneficiarioAsync(int id);
    }
}