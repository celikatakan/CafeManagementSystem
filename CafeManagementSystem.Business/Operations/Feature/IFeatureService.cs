using System;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.Business.Operations.Feature.Dtos;
using CafeManagementSystem.Business.Types;

namespace CafeManagementSystem.Business.Operations.Feature
{
	public interface IFeatureService
	{
        Task<ServiceMessage> AddFeature(AddFeatureDto feature);
        Task<List<FeatureDto>> GetFeatures();
    }
}

