namespace DVLD.Core.Enums
{
    public enum enInternationalLicenseSaveStatus
    {
        DriverDoesNotExist = 0,
        ApplicationDoesNotExist = 1,
        LocalLicenseDoesNotExist = 2,
        LocalLicenseNotClass3 = 3,
        LocalLicenseNotActive = 4,
        LocalLicenseExpired = 5,
        DriverAlreadyHasActiveInternationalLicense = 6,
        RequiredDataMissing = 7,
        Success = 8
    }
}
