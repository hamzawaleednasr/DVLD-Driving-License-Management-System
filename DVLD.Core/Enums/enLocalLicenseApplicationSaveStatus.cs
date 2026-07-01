namespace DVLD.Core.Enums
{
    public enum enLocalLicenseApplicationSaveStatus
    {
        ApplicationDoesNotExist = 0,
        LicenseClassDoesNotExist = 1,
        PersonAlreadyHasActiveApplication = 2,
        PersonAlreadyHasLicenseOfSameClass = 3,
        AgeLessThanMinimum = 4,
        RequiredDataMissing = 5,
        Success = 6
    }
}
