using System;
using CafeManagementSystem.Data.Entities;
using CafeManagementSystem.Data.Repositories;
using CafeManagementSystem.Data.UnitOfWork;

namespace CafeManagementSystem.Business.Operations.Setting
{
	public class SettingManager : ISettingService
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SettingEntity> _settingRepository;

        public SettingManager(IUnitOfWork unitOfWork, IRepository<SettingEntity> settingRepository)
        {
            _settingRepository = settingRepository;
            _unitOfWork = unitOfWork;
        }

        public bool GetMaintenanceState()
        {
            var maintenanceState = _settingRepository.GetById(1).MaintenanceMode;

            return maintenanceState;
        }

        public async Task ToggleMaintenance()
        {
            var setting = _settingRepository.GetById(1);

            setting.MaintenanceMode = !setting.MaintenanceMode;

            _settingRepository.Update(setting);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Bakım durumu güncellenirken bir hata ile karşılaşıldı.");
            }
        }
    }
}

