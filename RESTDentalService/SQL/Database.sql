CREATE TABLE Employees (
	Id INT IDENTITY(1,1),
	Name NVARCHAR(255) NOT NULL,
	Surname NVARCHAR(255) NOT NULL,
	Phone NVARCHAR(25) NOT NULL,
	Email NVARCHAR(25) NOT NULL,
	ClinicId INT NOT NULL,
	CONSTRAINT PK__Employees_Id PRIMARY KEY CLUSTERED (Id),
);
GO

CREATE UNIQUE NONCLUSTERED INDEX IX__Employees_NameSurnameEmail
ON Employees (Name, Surname, Email)
GO

CREATE TABLE Clinics (
	Id INT IDENTITY(1,1),
	UniqueNumber NVARCHAR(10) NOT NULL,
	Name NVARCHAR(30) NOT NULL,
	Location NVARCHAR(25) NOT NULL,
	ManagerId INT NOT NULL,
	CONSTRAINT UQ__Clinics_ManagerId UNIQUE (ManagerId),
	CONSTRAINT PK__Clinics_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT FK__Clinics_ManagerId FOREIGN KEY (ManagerId) REFERENCES Employees(Id)
);
GO

CREATE NONCLUSTERED INDEX IX__Clinics_NameUniqueNumber
ON Clinics (UniqueNumber, Name)
GO

CREATE TABLE Operations (
	Id INT IDENTITY(1,1),
	Name NVARCHAR(200) NOT NULL,
	ClinicId INT NOT NULL,
	Cost DECIMAL NOT NULL,
	Term DATE NOT NULL,
	EmployeeId INT NOT NULL,
	CONSTRAINT PK__Operations_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT FK__Operations_EmployeeId FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
	CONSTRAINT FK__Operations_ClinicId FOREIGN KEY (ClinicId) REFERENCES Clinics(Id)
);
GO

CREATE NONCLUSTERED INDEX IX__Operations_NameCost
ON Operations (Name, Cost)
GO

INSERT INTO Employees VALUES
('Wlodzimierz', 'Kowjako', '575-234-123', 'kowyako@dental.com'),
('Konrad', 'Kuczynski', '248-337-123', 'kuczynski@dental.com'),
('Maciej', 'Spaleniak', '111-234-123', 'spaleniak@dental.com')
GO

INSERT INTO Clinics VALUES
('RXQ5671234', 'Pomorska Clinic', 'ul. Pomorska, 32', 1),
('ABC8651234', 'Miserante Dental Clinic', 'ul. Pomorska, 32', 2)
GO

ALTER TABLE Employees
ADD ClinicId INT
GO

ALTER TABLE Employees
ADD CONSTRAINT FK__Employees_ClinicId FOREIGN KEY (ClinicId) REFERENCES Clinics(Id)
GO

CREATE TABLE Roles (
	Id INT IDENTITY(1,1),
	Name NVARCHAR(100) NOT NULL,
	CONSTRAINT PK__Roles_Id PRIMARY KEY CLUSTERED (Id)
);
GO

CREATE TABLE Users (
	Id INT IDENTITY(1,1),
	FirstName NVARCHAR(255),
	LastName NVARCHAR(255),
	DateOfBirth DATE,
	PasswordHash NVARCHAR(MAX),
	RoleId INT NOT NULL,
	CONSTRAINT PK__Users_Id PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT FK__Users_RoleId FOREIGN KEY (RoleId) REFERENCES Roles (Id)
);
GO

INSERT INTO Roles VALUES
('Admin'), ('Employee'), ('User')
GO