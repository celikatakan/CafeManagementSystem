using System;
namespace CafeManagementSystem.Business.Operations.Setting
{
	public interface ISettingService
	{
        Task ToggleMaintenance();
        bool GetMaintenanceState();
    }
}

