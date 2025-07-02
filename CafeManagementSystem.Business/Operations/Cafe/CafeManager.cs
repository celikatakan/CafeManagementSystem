using System;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.Business.Types;
using CafeManagementSystem.Data.Entities;
using CafeManagementSystem.Data.Repositories;
using CafeManagementSystem.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CafeManagementSystem.Business.Operations.Cafe
{
    public class CafeManager : ICafeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CafeEntity> _cafeRepository;
        private readonly IRepository<CafeFeatureEntity> _cafeFeatureRepository;

        public CafeManager(IUnitOfWork unitOfWork, IRepository<CafeEntity> cafeRepository, IRepository<CafeFeatureEntity> cafeFeatureRepository)
        {
            _unitOfWork = unitOfWork;
            _cafeRepository = cafeRepository;
            _cafeFeatureRepository = cafeFeatureRepository;
        }

        public async Task<ServiceMessage> AddCafe(AddCafeDto cafe)
        {
            var hasCafe = _cafeRepository.GetAll(x => x.Name.ToLower() == cafe.Name.ToLower()).Any();

            if (hasCafe)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Kafe zaten bulunuyor."
                };
            }

            await _unitOfWork.BeginTransaction();

            var cafeEntity = new CafeEntity
            {
                Name = cafe.Name,
                Stars = cafe.Stars,
                Location = cafe.Location
            };
            _cafeRepository.Add(cafeEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Kafe kaydı sırasında bir hata ile karşılaşıldı.");
            }

            foreach (var featureId in cafe.FeatureIds)
            {
                var cafeFeature = new CafeFeatureEntity
                {
                    CafeId = cafeEntity.Id,
                    FeatureId = featureId
                };
                _cafeFeatureRepository.Add(cafeFeature);
            }
            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("Özellik sırasında bir hata ile karşılaşıldı.");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Kafe başarıyla eklendi."
            };
        }

        public async Task<ServiceMessage> AddjustCafeStars(int id, int changeTo)
        {
            var cafe = _cafeRepository.GetById(id);

            if (cafe == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Kafe bulunamadı."
                };
            }
            cafe.Stars = changeTo;

            _cafeRepository.Update(cafe);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Yıldız sayısı değiştirirken bir hata oluştu.");
            }
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Yıldız sayısı başarıyla değiştirildi."
            };
        }

        public async Task<ServiceMessage> DeleteCafe(int id)
        {
            var cafe = _cafeRepository.GetById(id);
            if (cafe == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Kafe bulunamadı."
                };
            }
            _cafeRepository.Delete(id);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Kafe silinirken bir hata oluştu.");
            }
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Kafe başarıyla silindi."
            };
        }

        public async Task<CafeDto> GetCafe(int id)
        {
            var cafe = await _cafeRepository.GetAll(x => x.Id == id)
                  .Select(x => new CafeDto
                  {
                      Id = x.Id,
                      Name = x.Name,
                      Stars = x.Stars,
                      Location = x.Location,
                      Features = x.CafeFeatures.Select(f => new CafeFeatureDto
                      {
                          Id = f.Id,
                          Title = f.Feature.Title
                      }).ToList()

                  }).FirstOrDefaultAsync();

            return cafe;
        }

        public async Task<List<CafeDto>> GetCafes()
        {
            var cafes = await _cafeRepository.GetAll()
                  .Select(x => new CafeDto
                  {
                      Id = x.Id,
                      Name = x.Name,
                      Stars = x.Stars,
                      Location = x.Location,
                      Features = x.CafeFeatures.Select(f => new CafeFeatureDto
                      {
                          Id = f.Id,
                          Title = f.Feature.Title
                      }).ToList()

                  }).ToListAsync();

            return cafes;
        }

        public async Task<ServiceMessage> UpdateCafe(UpdateCafeDto cafe)
        {
            var cafeEntity = _cafeRepository.GetById(cafe.Id);

            if (cafeEntity == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Kafe bulunamadı."
                };
            }

            await _unitOfWork.BeginTransaction();

            cafeEntity.Name = cafe.Name;
            cafeEntity.Stars = cafe.Stars;
            cafeEntity.Location = cafe.Location;

            _cafeRepository.Update(cafeEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("Kafe bilgileri güncellenirken bir hata ile karşılaşıldı.");
            }

            var cafeFeatures = _cafeFeatureRepository.GetAll(x => x.CafeId == cafe.Id).ToList();

            foreach (var feature in cafeFeatures)
            {
                _cafeFeatureRepository.Delete(feature, false);
            }

            foreach (var featureId in cafe.FeatureIds)
            {
                var cafeFeature = new CafeFeatureEntity
                {
                    CafeId = cafeEntity.Id,
                    FeatureId = featureId,
                };
                _cafeFeatureRepository.Add(cafeFeature);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("Kafe özellikleri güncellenirken bir hata ile karşılaşıldı.");
            }
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Kafe başarıyla güncellendi."
            };
        }
    }
}

