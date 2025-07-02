using System;
using Microsoft.AspNetCore.DataProtection;

namespace CafeManagementSystem.Business.DataProtection
{
	public class DataProtection : IDataProtection
	{
        private readonly IDataProtector _protector;
        public DataProtection(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("CafeApp-secrity-v1");
        }

        public string Protect(string text)
        {
            return _protector.Protect(text);
        }

        public string Unprotect(string protectedText)
        {
            return _protector.Unprotect(protectedText);
        }
    }
}

