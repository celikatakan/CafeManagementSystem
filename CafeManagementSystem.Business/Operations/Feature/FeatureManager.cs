using System;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.Business.Operations.Feature.Dtos;
using CafeManagementSystem.Business.Types;
using CafeManagementSystem.Data.Entities;
using CafeManagementSystem.Data.Repositories;
using CafeManagementSystem.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CafeManagementSystem.Business.Operations.Feature
{
	public class FeatureManager : IFeatureService
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<FeatureEntity> _repository;

        public FeatureManager(IUnitOfWork unitOfWork, IRepository<FeatureEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;

        }
        public async Task<ServiceMessage> AddFeature(AddFeatureDto feature)
        {
            var hasFeature = _repository.GetAll(x => x.Title.ToLower() == feature.Title.ToLower()).Any();

            if (hasFeature)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Özellik zaten bulunuyor."
                };
            }
            var featureEntity = new FeatureEntity
            {
                Title = feature.Title
            };
            _repository.Add(featureEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Özellik sırasında bir hata oluştu.");
            }
            return new ServiceMessage
            {
                IsSucceed = true
            };
        }

        public async Task<List<FeatureDto>> GetFeatures()
        {
            var features = await _repository.GetAll()
                  .Select(x => new FeatureDto
                  {
                      Id=x.Id,
                      Title =x.Title

                  }).ToListAsync();
            return features;
        }
    }
}

