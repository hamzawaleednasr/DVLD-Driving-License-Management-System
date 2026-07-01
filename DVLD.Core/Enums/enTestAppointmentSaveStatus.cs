namespace DVLD.Core.Enums
{
    public enum enTestAppointmentSaveStatus
    {
        LocalLicenseApplicationDoesNotExist = 0,
        TestTypeDoesNotExist = 1,
        UserDoesNotExist = 2,
        PersonAlreadyHasActiveAppointmentForThisTest = 3,
        PersonAlreadyPassedThisTest = 4,
        PreviousTestNotPassed = 5,
        RequiredDataMissing = 6,
        Success = 7
    }
}
