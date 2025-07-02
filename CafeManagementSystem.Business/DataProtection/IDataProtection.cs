using System;
namespace CafeManagementSystem.Business.DataProtection
{
	public interface IDataProtection
	{
        string Protect(string text);
        string Unprotect(string protectedText);
    }
}

