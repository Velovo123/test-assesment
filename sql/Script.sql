
CREATE DATABASE TestAssessment;


USE TestAssessment;


CREATE TABLE Trips (
    TripId INT IDENTITY(1,1) PRIMARY KEY, 
    PickupDatetime DATETIME NOT NULL,    
    DropoffDatetime DATETIME NOT NULL,   
    PassengerCount INT NOT NULL,        
    TripDistance FLOAT NOT NULL,         
    StoreAndFwdFlag NVARCHAR(3) NOT NULL, 
    PULocationID INT NOT NULL,           
    DOLocationID INT NOT NULL,          
    FareAmount DECIMAL(18, 2) NOT NULL, 
    TipAmount DECIMAL(18, 2) NOT NULL    
);


CREATE INDEX IX_Trips_PULocationID ON Trips (PULocationID);
CREATE INDEX IX_Trips_DOLocationID ON Trips (DOLocationID);
CREATE INDEX IX_Trips_TripDistance ON Trips (TripDistance);
CREATE INDEX IX_Trips_PickupDatetime ON Trips (PickupDatetime);