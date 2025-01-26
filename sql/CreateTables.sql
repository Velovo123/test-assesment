use ETLProject;

CREATE TABLE Trip (
    TripId INT PRIMARY KEY IDENTITY,
    PickupDatetime DATETIME NOT NULL,
    DropoffDatetime DATETIME NOT NULL,
    PassengerCount INT NOT NULL,
    TripDistance FLOAT NOT NULL,
    StoreAndFwdFlag NVARCHAR(3) NOT NULL
);

CREATE TABLE Location (
    LocationId INT PRIMARY KEY IDENTITY,
    PULocationID INT NOT NULL,
    DOLocationID INT NOT NULL,
    TripId INT FOREIGN KEY REFERENCES Trip(TripId)
);

CREATE TABLE Fare (
    FareId INT PRIMARY KEY IDENTITY,
    FareAmount MONEY NOT NULL,
    TipAmount MONEY NOT NULL,
    TripId INT FOREIGN KEY REFERENCES Trip(TripId)
);

CREATE TABLE ProcessedTrip (
    ProcessedTripId INT PRIMARY KEY IDENTITY,
    TripId INT FOREIGN KEY REFERENCES Trip(TripId),
    IsDuplicate BIT NOT NULL,
    UTCConversionDatetime DATETIME
);
