USE master;

CREATE DATABASE DVLD;

GO

USE DVLD;

GO

-- ============================
-- ==  Create Lookup Tables  ==
-- ============================

-- 1. Create Country Table
CREATE TABLE Countries (
	CountryID INT IDENTITY(1,1) PRIMARY KEY,
	CountryName NVARCHAR(100) UNIQUE NOT NULL,
);

-- 2. Insert all 195 recognized countries
INSERT INTO Countries (CountryName) 
VALUES 
    ('Afghanistan'), ('Albania'), ('Algeria'), ('Andorra'), ('Angola'), 
    ('Antigua and Barbuda'), ('Argentina'), ('Armenia'), ('Australia'), ('Austria'), 
    ('Azerbaijan'), ('Bahamas'), ('Bahrain'), ('Bangladesh'), ('Barbados'), 
    ('Belarus'), ('Belgium'), ('Belize'), ('Benin'), ('Bhutan'), 
    ('Bolivia'), ('Bosnia and Herzegovina'), ('Botswana'), ('Brazil'), ('Brunei'), 
    ('Bulgaria'), ('Burkina Faso'), ('Burundi'), ('Cabo Verde'), ('Cambodia'), 
    ('Cameroon'), ('Canada'), ('Central African Republic'), ('Chad'), ('Chile'), 
    ('China'), ('Colombia'), ('Comoros'), ('Congo (Congo-Brazzaville)'), ('Costa Rica'), 
    ('Croatia'), ('Cuba'), ('Cyprus'), ('Czechia (Czech Republic)'), ('Democratic Republic of the Congo'), 
    ('Denmark'), ('Djibouti'), ('Dominica'), ('Dominican Republic'), ('Ecuador'), 
    ('Egypt'), ('El Salvador'), ('Equatorial Guinea'), ('Eritrea'), ('Estonia'), 
    ('Eswatini (Swaziland)'), ('Ethiopia'), ('Fiji'), ('Finland'), ('France'), 
    ('Gabon'), ('Gambia'), ('Georgia'), ('Germany'), ('Ghana'), 
    ('Greece'), ('Grenada'), ('Guatemala'), ('Guinea'), ('Guinea-Bissau'), 
    ('Guyana'), ('Haiti'), ('Holy See'), ('Honduras'), ('Hungary'), 
    ('Iceland'), ('India'), ('Indonesia'), ('Iran'), ('Iraq'), 
    ('Ireland'), ('Israel'), ('Italy'), ('Jamaica'), ('Japan'), 
    ('Jordan'), ('Kazakhstan'), ('Kenya'), ('Kiribati'), ('Kuwait'), 
    ('Kyrgyzstan'), ('Laos'), ('Latvia'), ('Lebanon'), ('Lesotho'), 
    ('Liberia'), ('Libya'), ('Liechtenstein'), ('Lithuania'), ('Luxembourg'), 
    ('Madagascar'), ('Malawi'), ('Malaysia'), ('Maldives'), ('Mali'), 
    ('Malta'), ('Marshall Islands'), ('Mauritania'), ('Mauritius'), ('Mexico'), 
    ('Micronesia'), ('Moldova'), ('Monaco'), ('Mongolia'), ('Montenegro'), 
    ('Morocco'), ('Mozambique'), ('Myanmar (Burma)'), ('Namibia'), ('Nauru'), 
    ('Nepal'), ('Netherlands'), ('New Zealand'), ('Nicaragua'), ('Niger'), 
    ('Nigeria'), ('North Korea'), ('North Macedonia'), ('Norway'), ('Oman'), 
    ('Pakistan'), ('Palau'), ('Palestine State'), ('Panama'), ('Papua New Guinea'), 
    ('Paraguay'), ('Peru'), ('Philippines'), ('Poland'), ('Portugal'), 
    ('Qatar'), ('Romania'), ('Russia'), ('Rwanda'), ('Saint Kitts and Nevis'), 
    ('Saint Lucia'), ('Saint Vincent and the Grenadines'), ('Samoa'), ('San Marino'), ('Sao Tome and Principe'), 
    ('Saudi Arabia'), ('Senegal'), ('Serbia'), ('Seychelles'), ('Sierra Leone'), 
    ('Singapore'), ('Slovakia'), ('Slovenia'), ('Solomon Islands'), ('Somalia'), 
    ('South Africa'), ('South Korea'), ('South Sudan'), ('Spain'), ('Sri Lanka'), 
    ('Sudan'), ('Suriname'), ('Sweden'), ('Switzerland'), ('Syria'), 
    ('Tajikistan'), ('Tanzania'), ('Thailand'), ('Timor-Leste'), ('Togo'), 
    ('Tonga'), ('Trinidad and Tobago'), ('Tunisia'), ('Turkey'), ('Turkmenistan'), 
    ('Tuvalu'), ('Uganda'), ('Ukraine'), ('United Arab Emirates'), ('United Kingdom'), 
    ('United States of America'), ('Uruguay'), ('Uzbekistan'), ('Vanuatu'), ('Venezuela'), 
    ('Vietnam'), ('Yemen'), ('Zambia'), ('Zimbabwe');

-- 3. Create ApplicationTypes Table
CREATE TABLE ApplicationTypes (
    ApplicationTypeID INT IDENTITY(1,1) PRIMARY KEY,
    ApplicationTypeTitle NVARCHAR(100) UNIQUE NOT NULL,
    ApplicationTypeFees SMALLMONEY NOT NULL,
);

-- 4. Insert all Application Types
INSERT INTO ApplicationTypes (ApplicationTypeTitle, ApplicationTypeFees)
VALUES 
       ('New International License', 50.00),
       ('New Local Driving License Service', 15.00),
       ('Release Detained Driving License', 15.00),
       ('New Driving License Service', 5.00),
       ('Replacement for a Damaged Driving License', 5.00),
       ('Replacement for a Lost Driving License', 10.00);

-- 5. Create TestTypes Table
CREATE TABLE TestTypes (
    TestTypeID INT IDENTITY(1,1) PRIMARY KEY,
    TestTypeTitle NVARCHAR(100) UNIQUE NOT NULL,
    TestTypeDescription NVARCHAR(500) NULL,
    TestTypeFees SMALLMONEY NOT NULL,
);

-- 6. Insert all Test Types
INSERT INTO TestTypes (TestTypeTitle, TestTypeDescription, TestTypeFees)
VALUES 
    ('Vision Test', 'This assesses the applicant''s visual acuity to ensure the ability to see clearly.', 10.00),
    ('Written (Theory) Test', 'This test assesses the applicant''s knowledge of traffic rules and signs.', 20.00),
    ('Practical (Street) Test', 'This test evaluates the applicant''s driving skills and ability to operate a vehicle safely.', 30.00);

-- 7. Create LicenseClasses Table
CREATE TABLE LicenseClasses (
    LicenseClassID INT IDENTITY(1,1) PRIMARY KEY,
    LicenseClassTitle NVARCHAR(200) UNIQUE NOT NULL,
    LicenseClassDescription NVARCHAR(500) NULL,
    LicenseClassFees SMALLMONEY NOT NULL,
    MinimumAllowedAge TINYINT NOT NULL,
    ValidityLength TINYINT NOT NULL,
);

-- 8. Insert all License Classes
INSERT INTO LicenseClasses 
    (LicenseClassTitle, LicenseClassDescription, LicenseClassFees, MinimumAllowedAge, ValidityLength)
VALUES 
    (
        'Class 1: Small Motorcycle License', 
        'Allows the driver to ride small motorcycles with limited capacity and power.', 
        15.00, 18, 5
    ),
    (
        'Class 2: Heavy Motorcycle License', 
        'Allows the driver to ride large and powerful motorcycles.', 
        30.00, 21, 5
    ),
    (
        'Class 3: Ordinary Driving License (Car)', 
        'Allows the driver to drive light vehicles and private passenger cars.', 
        20.00, 18, 10
    ),
    (
        'Class 4: Commercial Driving License (Taxi/Limousine)', 
        'Allows the driver to drive taxis or limousine cars.', 
        200.00, 21, 10
    ),
    (
        'Class 5: Agricultural Vehicle License', 
        'Allows the driver to drive all agricultural vehicles, such as tractors and plowing machinery.', 
        50.00, 21, 10
    ),
    (
        'Class 6: Small and Medium Bus License', 
        'Allows the driver to drive small and medium-sized passenger buses.', 
        250.00, 21, 10
    ),
    (
        'Class 7: Trucks and Heavy Vehicles License', 
        'Allows the driver to drive heavy vehicles and large trucks, including large transport buses.', 
        300.00, 21, 10
    );

GO

-- ============================
-- ==  Create Other Tables   ==
-- ============================

-- Create People Table
CREATE TABLE People (
	PersonID INT IDENTITY(1,1) PRIMARY KEY,
	NationalNumber VARCHAR(50) UNIQUE NOT NULL,
	FirstName NVARCHAR(100) NOT NULL,
	SecondName NVARCHAR(100) NULL,
	ThirdName NVARCHAR(100) NULL,
	LastName NVARCHAR(100) NOT NULL,
	BirthDate DATETIME2 NOT NULL,
	[Address] NVARCHAR(500) NOT NULL,
	Gender BIT NOT NULL,
	Phone VARCHAR(50) NOT NULL,
	Email VARCHAR(100),
	PersonalPhoto VARCHAR(1000) NULL,
	CountryID INT NOT NULL,

	CONSTRAINT CHK_Person_BirthDate CHECK (BirthDate <= DATEADD(year, -18, SYSDATETIME())),
	CONSTRAINT CHK_Person_Email CHECK (Email LIKE '%_@__%.__%' AND Email NOT LIKE '% %'),
	CONSTRAINT FK_People_Countries FOREIGN KEY (CountryID) REFERENCES Countries(CountryID)
);

-- Create Users Table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    PersonID INT UNIQUE NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DEF_Users_IsActive DEFAULT 1,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME2 CONSTRAINT DEF_Users_CreatedAt DEFAULT SYSDATETIME(),

    CONSTRAINT FK_Users_People FOREIGN KEY (PersonID) REFERENCES People(PersonID),
);

-- Create Drivers Table
CREATE TABLE Drivers (
    DriverID INT IDENTITY(1,1) PRIMARY KEY,
    PersonID INT UNIQUE NOT NULL,
    CreatedByUserID INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DEF_Drivers_CreatedAt DEFAULT SYSDATETIME(),

    CONSTRAINT FK_Drivers_People FOREIGN KEY (PersonID) REFERENCES People(PersonID),
    CONSTRAINT FK_Drivers_Users FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT CHK_Drivers_CreatedAt CHECK (CreatedAt <= SYSDATETIME()),
);

-- Create Applications Table
CREATE TABLE Applications (
    ApplicationID INT IDENTITY(1,1) PRIMARY KEY,
    ApplicationStatus NVARCHAR(50) NOT NULL,
    ApplicationLastStatusDate DATETIME2 NOT NULL CONSTRAINT DEF_Applications_ApplicationLastStatusDate DEFAULT SYSDATETIME(),
    ApplicationPaidFees SMALLMONEY NOT NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DEF_Applications_CreatedAt DEFAULT SYSDATETIME(),
    PersonID INT NOT NULL,
    ApplicationTypeID INT NOT NULL,
    CreatedByUserID INT NOT NULL,

    CONSTRAINT FK_Applications_People FOREIGN KEY (PersonID) REFERENCES People(PersonID),
    CONSTRAINT FK_Applications_ApplicationTypes FOREIGN KEY (ApplicationTypeID) REFERENCES ApplicationTypes(ApplicationTypeID),
    CONSTRAINT FK_Applications_Users FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
);

-- Create Licenses Table
CREATE TABLE Licenses (
    LicenseID INT IDENTITY(1,1) PRIMARY KEY,
    LicenseIssueDate DATETIME2 NOT NULL CONSTRAINT DEF_Licenses_IssueDate DEFAULT SYSDATETIME(),
    LicenseExpirationDate DATETIME2 NOT NULL,
    LicensePaidFees SMALLMONEY NOT NULL,
    LicenseIssueReason TINYINT NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DEF_Licenses_IsActive DEFAULT 1,
    Notes NVARCHAR(500) NULL,
    DriverID INT NOT NULL,
    ApplicationID INT UNIQUE NOT NULL,
    LicenseClassID INT NOT NULL,
    
    CONSTRAINT FK_Licenses_Drivers FOREIGN KEY (DriverID) REFERENCES Drivers(DriverID),
    CONSTRAINT FK_Licenses_Applications FOREIGN KEY (ApplicationID) REFERENCES Applications(ApplicationID),
    CONSTRAINT FK_Licenses_LicenseClasses FOREIGN KEY (LicenseClassID) REFERENCES LicenseClasses(LicenseClassID),
);

-- Create LocalLicenseApplications Table
CREATE TABLE LocalLicenseApplications (
    LocalLicenseApplicationID INT IDENTITY(1,1) PRIMARY KEY,
    ApplicationID INT UNIQUE NOT NULL,
    LicenseClassID INT NOT NULL,

    CONSTRAINT FK_LocalLicenseApplications_Applications FOREIGN KEY (ApplicationID) REFERENCES Applications(ApplicationID),
    CONSTRAINT FK_LocalLicenseApplications_LicenseClasses FOREIGN KEY (LicenseClassID) REFERENCES LicenseClasses(LicenseClassID),
);

-- Create DetainedLicenses Table
CREATE TABLE DetainedLicenses (
    DetainedLicenseID INT IDENTITY(1,1) PRIMARY KEY,
    DetainDate DATETIME2 NOT NULL CONSTRAINT DEF_DetainedLicenses_DetainDate DEFAULT SYSDATETIME(),
    FineFees SMALLMONEY NOT NULL,
    ReleaseDate DATETIME2 NULL,
    IsReleased BIT NOT NULL CONSTRAINT DEF_DetainedLicenses_IsReleased DEFAULT 0,
    LicenseID INT NOT NULL,
    CreatedByUserID INT NOT NULL,
    ReleasedByUserID INT NULL,
    ReleaseApplicationID INT NULL,

    CONSTRAINT FK_DetainedLicenses_Licenses FOREIGN KEY (LicenseID) REFERENCES Licenses(LicenseID),
    CONSTRAINT FK_DetainedLicenses_CreatedByUserID FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_DetainedLicenses_ReleasedByUserID FOREIGN KEY (ReleasedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_DetainedLicenses_ReleaseApplicationID FOREIGN KEY (ReleaseApplicationID) REFERENCES Applications(ApplicationID),
);

-- Create TestAppointments Table
CREATE TABLE TestAppointments (
    TestAppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    TestAppointmentDate DATETIME2 NOT NULL,
    TestAppointmentPaidFees SMALLMONEY NOT NULL,
    IsLocked BIT NOT NULL CONSTRAINT DEF_TestAppointments_IsLocked DEFAULT 0,
    LocalLicenseApplicationID INT NOT NULL,
    TestTypeID INT NOT NULL,
    CreatedByUserID INT NOT NULL,
    RetakeTestApplicationID INT NULL,

    CONSTRAINT FK_TestAppointments_LocalLicenseApplications FOREIGN KEY (LocalLicenseApplicationID) REFERENCES LocalLicenseApplications(LocalLicenseApplicationID),
    CONSTRAINT FK_TestAppointments_TestTypes FOREIGN KEY (TestTypeID) REFERENCES TestTypes(TestTypeID),
    CONSTRAINT FK_TestAppointments_Users FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_TestAppointments_Applications FOREIGN KEY (RetakeTestApplicationID) REFERENCES Applications(ApplicationID),
);

-- Create Tests Table
CREATE TABLE Tests (
    TestID INT IDENTITY(1,1) PRIMARY KEY,
    TestResult BIT NOT NULL,
    TestNotes NVARCHAR(500) NULL,
    CreatedByUserID INT NOT NULL,
    TestAppointmentID INT NOT NULL,

    CONSTRAINT FK_Tests_Users FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Tests_TestAppointments FOREIGN KEY (TestAppointmentID) REFERENCES TestAppointments(TestAppointmentID),
);

-- Create InternationalLicenses Table
CREATE TABLE InternationalLicenses (
    InternationalLicenseID INT IDENTITY(1,1) PRIMARY KEY,
    IssueDate DATETIME2 NOT NULL CONSTRAINT DEF_InternationalLicenses_IssueDate DEFAULT SYSDATETIME(),
    ExpirationDate DATETIME2 NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DEF_InternationalLicenses_IsActive DEFAULT 1,
    DriverID INT NOT NULL,
    ApplicationID INT UNIQUE NOT NULL,
    IssueUsingLocalLicenseID INT NOT NULL,
    CreatedByUserID INT NOT NULL,
    
    CONSTRAINT FK_InternationalLicenses_Drivers FOREIGN KEY (DriverID) REFERENCES Drivers(DriverID),
    CONSTRAINT FK_InternationalLicenses_Applications FOREIGN KEY (ApplicationID) REFERENCES Applications(ApplicationID),
    CONSTRAINT FK_InternationalLicenses_Licenses FOREIGN KEY (IssueUsingLocalLicenseID) REFERENCES Licenses(LicenseID),
    CONSTRAINT FK_InternationalLicenses_Users FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
);